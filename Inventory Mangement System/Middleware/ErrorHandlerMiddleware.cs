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
            Result result = new Result();
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                switch (error)
                {
                    case ArgumentException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        result = new Result()
                        {
                            Message = e.Message,
                            Status = Result.ResultStatus.warning,
                        };
                        break;
                    case MethodAccessException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        result = new Result()
                        {
                            Message = e.Message,
                            Status = Result.ResultStatus.warning,
                        };
                        break;
                    case UnauthorizedAccessException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        result = new Result()
                        {
                            Message = e.Message,
                            Status = Result.ResultStatus.warning,
                        };
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        result = new Result()
                        {
                            Message = "Something went wrong",
                            Status = Result.ResultStatus.warning,
                        };
                        break;
                }
                var errorJson = JsonConvert.SerializeObject(result);
                await context.Response.WriteAsync(errorJson);
            }
        }
    }
}

