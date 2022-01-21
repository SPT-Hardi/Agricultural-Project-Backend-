
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
        private readonly ProductInventoryDataContext _context;

        public TokenController(ITokenService tokenService, ProductInventoryDataContext context)
        {
            _tokenService = tokenService;
            _context = context;
        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh(string token, string refreshToken)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(token);
            var emailid = principal.Identity.Name;

            var user = _context.Users.SingleOrDefault(x => x.EmailAddress == emailid);
            var userrefreshtoken = _context .UserRefreshTokens .SingleOrDefault (x => x.UserID == user.UserID );
            var r1 = _context.RefreshTokens.SingleOrDefault(x => x.RefreshID == userrefreshtoken.RefreshID);
            
            if (user == null  || r1.RToken  != refreshToken )
                return BadRequest();

            var newJwtToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            r1.RToken = newRefreshToken;
            //user.RefreshToken = newRefreshToken;
             _context.SubmitChanges();
            return new ObjectResult(new
            {
                token = newJwtToken,
                refreshToken = newRefreshToken
            });
        }

        [Authorize]
        [HttpPost("Revoke")]
        public async Task<IActionResult> Revoke()
        {
            var emailaddress = User.Identity.Name;

            var user = _context.Users.SingleOrDefault(u => u.EmailAddress  == emailaddress );
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
