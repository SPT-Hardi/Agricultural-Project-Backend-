﻿using Inventory_Mangement_System.Middleware;
using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Model.Common;
using Inventory_Mangement_System.serevices;
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
        Middleware.PasswordHasher passwordHasher = new PasswordHasher();
        public AccountRepository (IConfiguration configuration,ITokenService tokenService)
        {
            _configuration = configuration;
            _tokenService = tokenService;
        }

        //Add Role
        public Result AddRole(RoleModel roleModel )
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                Role role = new Role();
                role.RoleName = roleModel.RoleName;
                var check = context.Roles.FirstOrDefault(x => x.RoleName == roleModel.RoleName);

                if(check != null)
                {
                    return new Result()
                    {
                        Message = string.Format($"Role already exist"),
                        Status = Result.ResultStatus.none,
                    };
                }
                else
                {
                    context.Roles.InsertOnSubmit(role);
                    context.SubmitChanges();
                    return new Result()
                    {
                        Message = string.Format($"New Role Added Successfully"),
                        Status = Result.ResultStatus.success,
                        Data = roleModel.RoleName,
                    };
                }
            }
        }
        
        //View All User
        public async Task<IEnumerable> ViewAllUser()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                return (from u in context.Users
                        join r in context.Roles
                        on u.RoleID equals r.RoleID
                        select new
                        {
                            UserID = u.UserID,
                            UserName = u.UserName,
                            EmailAddress = u.EmailAddress,
                            RoleName = r.RoleName
                        }).ToList();
            }
        }

        //User Registration
        public Result RegisterUser(UserModel userModel)
        {
            ProductInventoryDataContext context = new ProductInventoryDataContext();
            User user = new User();
            Role role = new Role();
            var query = (from user1 in context.Users
                         join r1 in context.Roles
                         on user1.RoleID equals r1.RoleID
                         where user1.EmailAddress  == userModel.EmailAddress  && user1.UserName==userModel.UserName
                         select new
                         {
                             r1.RoleName
                         }).Count();
            if (query != 0)
            {
                throw new ArgumentException("User Already Exists With Same Name!");
            }
            else
            {
                user.UserName = char.ToUpper(userModel.UserName[0]) + userModel.UserName.Substring(1).ToLower();
                user.Password = passwordHasher.EncryptPassword(userModel.Password);
                user.RoleID = 2;
                user.EmailAddress = userModel.EmailAddress;
              
                user.DateTime = DateTime.Now;
                context.Users.InsertOnSubmit(user);
                context.SubmitChanges();
                return new Result()
                {
                    Message = string.Format($"{userModel.UserName}  Register as User Successfully"),
                    Status = Result.ResultStatus.success,
                    Data = userModel.UserName,
                };
             }
        }
       
        public Result LoginUser(LoginModel loginModel)
        {
            ProductInventoryDataContext context = new ProductInventoryDataContext();
            User user = new User();
            Role role = new Role();

            var res = (from u1 in context.Users
                       where u1.EmailAddress  == loginModel.EmailAddress  && u1.Password == loginModel.Password
                       select new
                       {
                           UserName = u1.UserName,
                           UserID = u1.UserID,
                           RoleID = u1.RoleID,
                           RoleName = u1.Role.RoleName
                       }).FirstOrDefault();
            if (res != null)
            {
                var qs = (from obj in context.Users
                          where obj.EmailAddress == loginModel.EmailAddress
                          select obj.UserName).FirstOrDefault();

                LoginDetail l = new LoginDetail();

                l.UserName = qs;
                l.DateTime = DateTime.Now;
                context.LoginDetails.InsertOnSubmit(l);
                context.SubmitChanges();
                var authclaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,loginModel.EmailAddress),
                    new Claim (ClaimTypes.Role,res.RoleName),
                    new Claim (ClaimTypes.Sid ,l.LoginID.ToString()),
                    new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid ().ToString ()),
                };

                var jwtToken = _tokenService.GenerateAccessToken(authclaims);
                var refreshToken = _tokenService.GenerateRefreshToken();
                RefreshToken refreshToken1 = new RefreshToken();
                refreshToken1.RToken = refreshToken;
                context.RefreshTokens.InsertOnSubmit(refreshToken1);
                context.SubmitChanges();

                UserRefreshToken userRefreshToken = new UserRefreshToken();
                userRefreshToken.UserID = res.UserID;
                userRefreshToken.RefreshID = refreshToken1.RefreshID;
                context.UserRefreshTokens.InsertOnSubmit(userRefreshToken);
                context.SubmitChanges();

                return new Result()
                {
                    Message = string.Format($"Login Successfully"),
                    Status = Result.ResultStatus.success,
                    Data = new
                    {
                        token = jwtToken,
                        refreshToken = refreshToken,
                        UserName = res.UserName,
                        EmailAddress = loginModel.EmailAddress,
                        RoleName = res.RoleName,
                    },
                };
            }
            else
            {
                throw new ArgumentException("Please Enter Valid Login Details..");
            }
        }
    }
}
