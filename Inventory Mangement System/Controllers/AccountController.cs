using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Repository;
using Inventory_Mangement_System.serevices;
using Microsoft.AspNetCore.Authorization;
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
            var result = await _accountRepository.AddRole(roleModel);
            return Ok(result);
        }

        [HttpPost("SignUp")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> AddUser([FromBody]UserModel userModel)
        {
            string rname = (string)HttpContext.Items["Rolename"];
            if (rname == "SuperAdmin")
            {
                var result = await _accountRepository.RegisterUser(userModel);
                return Ok(result);
            }
            return Unauthorized();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> SignIn([FromBody]LoginModel loginModel)
        {
            var result = await _accountRepository.LoginUser(loginModel);
            if(string .IsNullOrEmpty (result))
            {
                return Unauthorized();
            }
            return Ok(result);
        }

        [HttpGet("gettoken")]
        public ActionResult<string> GetToken()
        {
            string rname = (string)HttpContext.Items["Rolename"];//context.Items["Rolename"] = RName;
            int uid = (int)HttpContext.Items["UserId"];
            return Ok(rname);
            //var isClaim = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals(Claimstype.StringComparison.InvariantCultureIgnoreCase));
            //if (isClaim != null)
            //{
            //    var id = isClaim.Value;
            //   // var name1 = User.Identity.Name;

            //    return Ok(id);
            //}
            //else
            //{
            //    return BadRequest("no");
            //}
        }
    }
}
