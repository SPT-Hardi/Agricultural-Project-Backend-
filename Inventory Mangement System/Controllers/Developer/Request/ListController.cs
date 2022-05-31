using Inventory_Mangement_System.Controllers.Developer.Filters;
using Inventory_Mangement_System.Repository.Developer;
using Inventory_Mangement_System.Repository.Developer.Contact;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using TeamIN.ReckonIN.API.Helpers.Kendo;

namespace Inventory_Mangement_System.Controllers.Developer.Request
{
    [Route("api/Common/[controller]")]
    [ApiController]
    public class ListController : ControllerBase
    {
        [Route("General/Data")]
        [HttpGet]
        public IActionResult General(string Con,[DataSourceRequest] DataSourceRequest value, string parentCon = null)
        {
            try
            {
                var exists = new Repository.Developer.Schema.Controllers().One(Con);
                if (exists is null)
                {
                    throw new HttpResponseException() { Status = 404, Value = "Controller doesn't exists!" };
                }

                if (exists.ControllerType.Id != 12)
                {
                    throw new HttpResponseException() { Status = 405, Value = "Controller not allowed to perform this operation!" };
                }

                if (exists.NeedLogin == true)
                {
                    if (Current.IsLoggedIn == true)
                    {
                        Current.SetCId(exists.CId);
                        if (!Current.IsAuthorised(Con, parentCon))
                        {
                            throw new HttpResponseException() { Status = 403, Value = "You are not authorised to access this feature!" };
                        }
                    }
                    else
                    {
                        var access_token = Current.httpContext.Request.Headers["Authorization"].ToString();
                        if (String.IsNullOrEmpty(access_token))
                        {
                            throw new HttpResponseException() { Status = 401, Value = "invalid_token" };
                        }
                        else
                        {
                            throw new HttpResponseException() { Status = 401, Value = "token_expired" };
                        }
                    }
                }
                var whereClause = value.WhereClause();

                return Ok(new Lists().General(Con, value.PageSize, value.Page, whereClause.Clause, whereClause.SqlParameters, value.SortClause()));
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                throw new HttpResponseException() { Status = 400, Value = ex.Message };
            }
        }

        [Route("General/Schema")]
        [HttpGet]
        public IActionResult GeneralSchema(string Con, [DataSourceRequest] DataSourceRequest value, string parentCon = null)
        {
            try
            {
                var exists = new Repository.Developer.Schema.Controllers().One(Con);
                if (exists is null)
                {
                    throw new HttpResponseException() { Status = 404, Value = "Controller doesn't exists!" };
                }

                if (exists.ControllerType.Id != 12)
                {
                    throw new HttpResponseException() { Status = 405, Value = "Controller not allowed to perform this operation!" };
                }

                if (exists.NeedLogin == true)
                {
                    if (Current.IsLoggedIn == true)
                    {
                        Current.SetCId(exists.CId);
                        if (!Current.IsAuthorised(Con, parentCon))
                        {
                            throw new HttpResponseException() { Status = 403, Value = "You are not authorised to access this feature!" };
                        }
                    }
                    else
                    {
                        var access_token = Current.httpContext.Request.Headers["Authorization"].ToString();
                        if (String.IsNullOrEmpty(access_token))
                        {
                            throw new HttpResponseException() { Status = 401, Value = "invalid_token" };
                        }
                        else
                        {
                            throw new HttpResponseException() { Status = 401, Value = "token_expired" };
                        }
                    }
                }
                var whereClause = value.WhereClause();

                return Ok(new Lists().GeneralSchema(Con, value.WhereClause().Clause, value.WhereClause().SqlParameters, value.SortClause()));
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                throw new HttpResponseException() { Status = 400, Value = ex.Message };
            }
        }
    }
}
