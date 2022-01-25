﻿using Microsoft.AspNetCore.Http;
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
            
            catch (ArgumentException error)
            {
                var response = context.Response;
                //response.ContentType = "application/json";
                //response.StatusCode = (int)HttpStatusCode.AlreadyReported;
                var result = JsonSerializer.Serialize(new { 
                    message = error?.Message,
                    StatusCodes= (int)HttpStatusCode.AlreadyReported 
                });
                await response.WriteAsync(result);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                var result = JsonSerializer.Serialize(new { message = error?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}
