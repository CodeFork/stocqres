﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using Stocqres.Core.Exceptions;

namespace Stocqres.Core.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next)); ;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                var errorCode = "error";
                var statusCode = HttpStatusCode.BadRequest;
                var message = "There was an error.";
                switch (exception)
                {
                    case StocqresException e:
                        errorCode = e.Code;
                        message = e.Message;
                        statusCode = e.Status.HasValue
                            ? (HttpStatusCode)e.Status.Value
                            : HttpStatusCode.BadRequest;
                        break;
                }

                var response = new {code = errorCode, message = message};
                var payload = JsonConvert.SerializeObject(response);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)statusCode;

                Log.Error(payload);

                await context.Response.WriteAsync(payload);
            }
        }
    }
}
