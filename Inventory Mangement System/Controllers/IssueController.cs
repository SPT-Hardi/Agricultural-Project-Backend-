using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Repository;
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
        private readonly IIssueRepository _isueRepository;

        public IssueController (IIssueRepository isueRepository)
        {
            _isueRepository = isueRepository;
        }

        //All Issue Details
        [HttpGet("ViewAllIssue")]
        public async Task<IActionResult> ViewAllIssueAsync()
        {
            var result = _isueRepository.ViewAllIssue();
            return Ok(result);
        }

        //Issue Products
        [HttpPost("IssueProduct")]
        public async Task<IActionResult> IssueProductDetails([FromBody]IssueModel issueModel)
        {
            int LoginId = (int)HttpContext.Items["LoginId"];
            var result = _isueRepository.IssueProduct(issueModel,LoginId);
            return Ok(result);
        }

        //Edit Issue Details
        [HttpPut("EditIssue/{ID}")]
        public async Task<IActionResult> EditIssueAsync([FromBody] IssueModel issueModel,[FromRoute] int ID)
        {
            int LoginId = (int)HttpContext.Items["LoginId"];
            var result =  _isueRepository.EditIssue(issueModel,ID,LoginId);
            return Ok(result);
        }

        //Get MainArea Dropdown
        [HttpGet("getmainarea")]
        public async Task<IActionResult> MainAreaGet()
        {
            var result = await _isueRepository.GetMainArea();
            return Ok(result);
        }

        //Get SubArea Dropdown
        [HttpGet("getsubarea/{id}")]
        public async Task<IActionResult> SubAreaGet(int id)
        {
            var result = await _isueRepository.GetSubArea(id);
            return Ok(result);
        }

        //Get Product Dropdown with Unit And Quantity
        [HttpGet("getproduct")]
        public async Task<IActionResult> ProductGet()
        {
            var result = await _isueRepository.GetProduct();
            return Ok(result);
        }
                
    }
}