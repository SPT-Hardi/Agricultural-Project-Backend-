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
        public async Task<IActionResult> IssueProductDetails(IssueModel issueModel)
        {
            var result = _isueRepository.IssueProduct(issueModel);
            return Ok(result);
        }

        //Issue Detail By Id
        [HttpGet("ViewIssueById/{issueID}")]
        public async Task<IActionResult> ViewIssueByIdAsync([FromRoute] int issueID)
        {
            var result = await _isueRepository.ViewIssueById(issueID);
            return Ok(result);
        }

        [HttpGet("getmainarea")]
        public async Task<IActionResult> MainAreaGet()
        {
            var result = await _isueRepository.GetMainArea();
            return Ok(result);
        }

        [HttpGet("getsubarea/{id}")]
        public async Task<IActionResult> SubAreaGet(int id)
        {
            var result = await _isueRepository.GetSubArea(id);
            return Ok(result);
        }

        [HttpGet("getproduct")]
        public async Task<IActionResult> ProductGet()
        {
            var result = await _isueRepository.GetProduct();
            return Ok(result);
        }

        /*[HttpGet("GetProductTotalQuantity")]
        public async Task<IActionResult> GetProductTotalQuantityAsync()
        {
            var result = await _isueRepository.GetProductTotalQuantity();
            return Ok(result);
        }*/
    /*   //to view total quantity
        [HttpPost("total")]
        public async Task<IActionResult> totalcount(IssueModel issueModel)
        {
            var result = _isueRepository.total(issueModel);
            return Ok(result);
        }
    */
    }
}
