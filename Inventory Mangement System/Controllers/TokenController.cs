
using Inventory_Mangement_System.Model.Common;
using Inventory_Mangement_System.serevices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductInventoryContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh(TokenModel tokenModel)
        {
            string token = tokenModel.Token;
            string refreshToken = tokenModel.RefreshToken;
            using (ProductInventoryDataContext _context=new ProductInventoryDataContext())
            {
                var principal = _tokenService.GetPrincipalFromExpiredToken(token);
                var emailid = principal.Identity.Name;
                var user = _context.Users.SingleOrDefault(x => x.EmailAddress == emailid);

                var userrefreshtoken = (from r in _context.RefreshTokens
                                        join ur in _context.UserRefreshTokens
                                        on r.RefreshID equals ur.RefreshID
                                        where r.RToken == refreshToken && ur.UserID == user.UserID
                                        select new
                                        { 
                                            Id=ur.RefreshID,
                                            T1=r.RToken
                                        }).FirstOrDefault();

                if (user == null || userrefreshtoken.T1 != refreshToken)
                    return BadRequest();

                var newJwtToken = _tokenService.GenerateAccessToken(principal.Claims);
                var newRefreshToken = _tokenService.GenerateRefreshToken();
                
                var r1 = _context.RefreshTokens.SingleOrDefault(x => x.RefreshID == userrefreshtoken.Id && x.RToken==refreshToken);

                r1.RToken = newRefreshToken;
                _context.SubmitChanges();
                return new ObjectResult(new
                {
                    token = newJwtToken,
                    refreshToken = newRefreshToken
                });
            }
        }

        [Authorize]
        [HttpPost("Revoke")]
        public async Task<IActionResult> Revoke()
        {
            using (ProductInventoryDataContext _context=new ProductInventoryDataContext())
            {
                var emailaddress = User.Identity.Name;

                var user = _context.Users.SingleOrDefault(u => u.EmailAddress == emailaddress);
                if (user == null)
                    return BadRequest();

                var _user = _context.UserRefreshTokens.SingleOrDefault(id => id.UserID==user.UserID); 
                var Token = _context.RefreshTokens.SingleOrDefault(id => id.RefreshID==_user.RefreshID); 
                
                Token.RToken = null;
                _context.SubmitChanges();

                return NoContent();
            }
        }
    }
}
