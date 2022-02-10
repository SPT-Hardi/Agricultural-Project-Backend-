using Inventory_Mangement_System.Model.Common;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Result result = new Result();

            try
            {
                await _next(context);
            }
            
            catch (ArgumentException e)
            {
               
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                result = new Result()
                {
                    Message = e.Message,
                    Status = Result.ResultStatus.warning,
                };
            }
            catch (MethodAccessException e)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.NotModified;
                result = new Result()
                {
                    Message = e.Message,
                    Status = Result.ResultStatus.warning,
                };
            }
            catch (UnauthorizedAccessException e)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                result = new Result()
                {
                    Message = e.Message,
                    Status = Result.ResultStatus.warning,
                };
            }
            catch (Exception e)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                result= new Result()
                {
                    Message = e.Message,
                    Status = Result.ResultStatus.warning,
                };
            }
            finally
            {
                var errorJson = JsonConvert.SerializeObject(result);
                await context.Response.WriteAsync(errorJson);
            }
        }
    }
}
