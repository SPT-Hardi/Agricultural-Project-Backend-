using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.serevices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProductInventoryContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public AccountRepository (IConfiguration configuration,ITokenService tokenService )
        {
            _configuration = configuration;
            _tokenService = tokenService;
        }

        
        //To add new role
        public async Task<string> AddRole(RoleModel roleModel )
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                Role role = new Role();
                role.RoleName = roleModel.RoleName;
                var check = context.Roles.FirstOrDefault(x => x.RoleName == roleModel.RoleName);
                if(string .IsNullOrEmpty (role.RoleName))
                {
                    return "Enter Role Name";
                }
                if(check != null)
                {
                    return "Role already exist";
                }
                else
                {
                    context.Roles.InsertOnSubmit(role);
                    context.SubmitChanges();
                    return "New Role Added Successfully";
                }
            }
        }
       
        //To register user details
        public async Task<IEnumerable> RegisterUser(UserModel userModel)
        {
            ProductInventoryDataContext context = new ProductInventoryDataContext();
            User user = new User();
            Role role = new Role();
            var query = (from user1 in context.Users
                         join r1 in context.Roles
                         on user1.RoleID equals r1.RoleID
                         where user1.EmailAddress  == userModel.EmailAddress  && user1.Password == userModel.Password 
                         select new
                         {
                             r1.RoleName
                         }).Count();
            if (query != 0)
            {
                return "User Already exist";
            }
            else
            {
                user.UserName = userModel.UserName;
                var pcheck = context.Users.SingleOrDefault(x => x.Password == userModel.Password);
                if(pcheck != null)
                {
                    throw new MethodAccessException("Write Another Password");
                }
                user.Password = userModel.Password;
                user.RoleID = 2;
                user.EmailAddress = userModel.EmailAddress;
                context.Users.InsertOnSubmit(user);
                context.SubmitChanges();
                return $"{userModel.UserName}  Register as User Successfully";
            }
        }

        public async Task<string> LoginUser(LoginModel loginModel )
        {
            ProductInventoryDataContext context = new ProductInventoryDataContext();
            User user = new User();
            Role role = new Role();
            var res = (from u1 in context.Users
                       where u1.EmailAddress == loginModel.EmailAddress && u1.Password == loginModel.Password
                       select new
                       {
                           UserID = u1.UserID,
                           RoleID = u1.RoleID,
                         RoleName = u1.Role.RoleName
                       }).FirstOrDefault();
            if(res != null)
            {
                var authclaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,loginModel.EmailAddress),
                    new Claim (ClaimTypes.Role,res.RoleName),
                    new Claim (ClaimTypes .Sid ,res.UserID .ToString()),
                    new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid ().ToString ()),
                };
                var jwtToken = _tokenService.GenerateAccessToken(authclaims);
                var refreshToken = _tokenService.GenerateRefreshToken();
                RefreshToken refreshToken1 = new RefreshToken();
                refreshToken1.RToken  = refreshToken;
                context.RefreshTokens.InsertOnSubmit(refreshToken1);
                context.SubmitChanges();

                UserRefreshToken userRefreshToken = new UserRefreshToken();
                userRefreshToken.UserID = res.UserID;
                userRefreshToken.RefreshID = refreshToken1.RefreshID;
                context.UserRefreshTokens.InsertOnSubmit(userRefreshToken);
                context.SubmitChanges();

                var token = jwtToken;
                return $"{jwtToken}";

                //return new ObjectResult(new
                //{
                //    token = jwtToken,
                //    refreshToken = refreshToken
                //});

            }
            else
            {
                return "Please Enter valid login details";
            }
        }
    }
}
