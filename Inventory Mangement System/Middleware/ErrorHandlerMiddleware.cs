using Inventory_Mangement_System.Model;
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
            try
            {
                await _next(context);
            }
            catch (ArgumentException e)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                Result result = new Result()
                {
                    Message = e.Message,
                    Status = Result.ResultStatus.success,
                };
                await context.Response.WriteAsJsonAsync(result);
            }
            catch (MethodAccessException e)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.NotModified;
                Result result = new Result()
                {
                    Message = e.Message,
                    Status = Result.ResultStatus.success,
                };
                await context.Response.WriteAsJsonAsync(result);
            }
            catch (UnauthorizedAccessException e)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                Result result = new Result()
                {
                    Message = e.Message,
                    Status = Result.ResultStatus.success,
                };
                await context.Response.WriteAsJsonAsync(result);
            }
            catch (Exception e)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                Result result = new Result()
                {
                    Message = e.Message,
                    Status = Result.ResultStatus.success,
                };
                await context.Response.WriteAsJsonAsync(result);
            }
        }
    }
}
