using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssueController : ControllerBase
    {
        private readonly IIssueRepository _issueRepository;

        public IssueController(IIssueRepository issueRepository)
        {
            _issueRepository = issueRepository;
        }

        //All Issue Details
        [HttpGet("ViewAllIssue")]
        public async Task<IActionResult> ViewAllIssueAsync([FromQuery]int? Id)
        {
            var result = _issueRepository.ViewAllIssue(Id);
            return Ok(result);
        }

        //Get dropdown of non-editable records of given Id
        [HttpGet("GetEditedIssue/{Id}")]
        public IActionResult GetEditedIssueDetails(int Id) 
        {
            return Ok(new IssueRepository().GetEditIssueDetails(Id));
        }

        //Issue Products
        [HttpPost("IssueProduct")]
        [Authorize]
        public async Task<IActionResult> IssueProductDetails([FromBody]IssueModel issueModel)
        {
            if ((int)HttpContext.Items["LoginId"] == 0)
            {
                throw new ArgumentException("JWT Token Not Found.");
            }
            int LoginId = (int)HttpContext.Items["LoginId"];
            var result = _issueRepository.IssueProduct(issueModel,LoginId);
            return Ok(result);
        }

        //Edit Issue Details
        [HttpPut("EditIssue/{ID}")]
        public async Task<IActionResult> EditIssueAsync([FromBody] IssueModel issueModel,[FromRoute] int ID)
        {
            if ((int)HttpContext.Items["LoginId"] == 0)
            {
                throw new ArgumentException("JWT Token Not Found.");
            }
            int LoginId = (int)HttpContext.Items["LoginId"];
            var result = _issueRepository.EditIssue(issueModel,ID,LoginId);
            return Ok(result);
        }

        //Get MainArea Dropdown
        [HttpGet("getarea")]
        public async Task<IActionResult> AreaGet()
        {
            var result = await _issueRepository.GetArea();
            return Ok(result);
        }

        //Get SubArea Dropdown
     /*   [HttpGet("getsubarea/{id}")]
        public async Task<IActionResult> SubAreaGet(int id)
        {
            var result = await _issueRepository.GetSubArea(id);
            return Ok(result);
        }*/

        //Get Product Dropdown with Unit And Quantity
        [HttpGet("getproduct")]
        public async Task<IActionResult> ProductGet()
        {
            var result = await _issueRepository.GetProduct();
            return Ok(result);
        }
                
    }
}