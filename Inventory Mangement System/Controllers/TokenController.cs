
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

        //[HttpPost("Refresh")]
        //public async Task<IActionResult> Refresh(string token, string refreshToken)
        //{
        //    using (ProductInventoryDataContext _context = new ProductInventoryDataContext())
        //    {
        //        var principal = _tokenService.GetPrincipalFromExpiredToken(token);
        //        var emailid = principal.Identity.Name;
        //        var user = _context.Users.SingleOrDefault(x => x.EmailAddress == emailid);

        //        var u1 = (from ur in _context.UserRefreshTokens
        //                  join r in _context.RefreshTokens
        //                  on ur.RefreshID equals r.RefreshID
        //                  where r.RToken == refreshToken && ur.UserID == user.UserID
        //                  select new 
        //                  { 
        //                      Id = ur.RefreshID,
        //                      T1 = r.RToken 
        //                  }).FirstOrDefault ();

        //        if (user == null || u1.T1 != refreshToken)
        //                return BadRequest();

        //        var newJwtToken = _tokenService.GenerateAccessToken(principal.Claims);
        //        var newRefreshToken = _tokenService.GenerateRefreshToken();

        //        var r1 = _context.RefreshTokens.SingleOrDefault(x => x.RefreshID == u1.Id && x.RToken == refreshToken);
        //        r1.RToken = newRefreshToken;
        //        _context.SubmitChanges();
        //        return new ObjectResult(new
        //        {
        //            token = newJwtToken,
        //            refreshToken = newRefreshToken
        //        });
        //    }
        //}
        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh(TokenModel tokenModel )
        {
            using (ProductInventoryDataContext _context = new ProductInventoryDataContext())
            {
                var token = tokenModel.token;
                var refreshToken = tokenModel.refreshToken;
                var principal = _tokenService.GetPrincipalFromExpiredToken(token);
                var emailid = principal.Identity.Name;
                var user = _context.Users.SingleOrDefault(x => x.EmailAddress == emailid);

                var u1 = (from ur in _context.UserRefreshTokens
                          join r in _context.RefreshTokens
                          on ur.RefreshID equals r.RefreshID
                          where r.RToken == refreshToken && ur.UserID == user.UserID
                          select new
                          {
                              Id = ur.RefreshID,
                              T1 = r.RToken
                          }).FirstOrDefault();

                if (user == null || u1.T1 != refreshToken)
                    return BadRequest();

                var newJwtToken = _tokenService.GenerateAccessToken(principal.Claims);
                var newRefreshToken = _tokenService.GenerateRefreshToken();

                var r1 = _context.RefreshTokens.SingleOrDefault(x => x.RefreshID == u1.Id && x.RToken == refreshToken);
                r1.RToken = newRefreshToken;
                _context.SubmitChanges();
                return new ObjectResult(new
                {
                    token = newJwtToken,
                    refreshToken = newRefreshToken
                });
            }
        }

        /**//* [Authorize]
 */
        [HttpPost("Revoke")]
        public async Task<IActionResult> Revoke()
        {
            using (ProductInventoryDataContext _context = new ProductInventoryDataContext())
            {
                var emailaddress = User.Identity.Name;

                var user = _context.Users.SingleOrDefault(u => u.EmailAddress == emailaddress);
                if (user == null)
                    return BadRequest();
                var rt = _context.UserRefreshTokens.SingleOrDefault(id => id.UserID == user.UserID);
                var r1 = _context.RefreshTokens.SingleOrDefault(id => id.RefreshID == rt.RefreshID);
                r1.RToken = null;
                _context.SubmitChanges();

                return NoContent();
            }
        }
    }
}
