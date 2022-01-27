using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Repository;
using Inventory_Mangement_System.serevices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IConfiguration _configuration;

        public AccountController (IAccountRepository accountRepository, IConfiguration configuration, ITokenService tokenService )
        {
            _accountRepository = accountRepository;
            _configuration = configuration;
        }

        [HttpPost ("addRole")]
        public async Task<IActionResult> RoleAdded(RoleModel roleModel)
        {
            var result =  _accountRepository.AddRole(roleModel);
            return Ok(result);
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody]UserModel userModel)
        {
            var result =  _accountRepository.RegisterUser(userModel);
            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> SignIn([FromBody]LoginModel loginModel)
        {
            var result =  _accountRepository.LoginUser(loginModel);
            return Ok(result);
        }

        [HttpGet("gettoken")]
        public ActionResult<string> GetToken()
        {
            int uid = (int)HttpContext.Items["UserId"];
            return Ok(uid);
            //return (new
            //{
            //    ""=,
            //    ""
            //});
            //var isClaim = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals(ClaimTypes.Name, StringComparison.InvariantCultureIgnoreCase));
            //if (isClaim != null)
            //{
            //    var id = isClaim.Value;
            //    var name1 = User.Identity.Name;

            //    return Ok(name1);
            //}
            //else
            //{
            //    return BadRequest("no");
            //}
        }
    }
}
