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
            Result result = null;
            context.Response.ContentType = "application/json";
            var hasError = false;

            try
            {
                await _next(context);
            }
            catch (ArgumentException e)
            {
                hasError = true;
                var response = context.Response;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                result = new Result()
                {
                    Message = e.Message,
                    Status = Result.ResultStatus.warning,
                };
            }
            catch (MethodAccessException e)
            {
                hasError = true;

                var response = context.Response;
                response.StatusCode = (int)HttpStatusCode.NotModified;
                result = new Result()
                {
                    Message = e.Message,
                    Status = Result.ResultStatus.warning,
                };
            }
            catch (UnauthorizedAccessException e)
            {
                hasError = true;

                var response = context.Response;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                result = new Result()
                {
                    Message = e.Message,
                    Status = Result.ResultStatus.warning,
                };
            }
            catch (Exception e)
            {
                hasError = true;

                var response = context.Response;
               
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                result = new Result()
                {
                    Message = e.Message,
                    Status = Result.ResultStatus.warning,
                };
            }
            finally
            {
                if (hasError)
                {
                    var errorJson = JsonConvert.SerializeObject(result);
                    await context.Response.WriteAsync(errorJson);
                }
            }
        }
    }
}
