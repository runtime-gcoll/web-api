﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwirlTheoryApi.Middleware
{
    public class OptionsMiddleware
    {
        private readonly RequestDelegate _next;
        private IHostingEnvironment _environment;

        public OptionsMiddleware(RequestDelegate next, IHostingEnvironment environment) {
            _next = next;
            _environment = environment;
        }

        public async Task Invoke(HttpContext context) {
            // If it's not an OPTIONS request, then just pass the context on 
            // to the next part of the request pipeline
            if (context.Request.Method != "OPTIONS") {
                await this._next.Invoke(context);
            }
            // Otherwise, mess with the headers and such to respond
            // to preflight requests properly
            this.BeginInvoke(context);
        }

        private async void BeginInvoke(HttpContext context) {
            // Only get involved if we're dealing with an OPTIONS request
            if (context.Request.Method == "OPTIONS") {
                // Set all the required headers
                context.Response.Headers.Add("Access-Control-Allow-Origin", new[] { (string)context.Request.Headers["Origin"] });
                context.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "Origin, X-Requested-With, Content-Type, Accept" });
                context.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "GET, POST, PUT, DELETE, OPTIONS" });
                context.Response.Headers.Add("Access-Control-Allow-Credentials", new[] { "true" });
                // Set the response code to 200 OK
                context.Response.StatusCode = 200;
                // Write a response back to the HttpContext
                // This will end the request here, which is why we need
                // to check OPTIONS Invoke, because before it would try to write
                // the reponse twice and the browser complained about chunked response
                await context.Response.WriteAsync("OK");
            }
        }
    }

    // This allows us to call app.UseOptions() in Startup.cs > Configure() to enable the middleware for *all* requests
    // This might have a minor impact on request pipeline performance, but since it allows the API to actually work I'm just going to have to stomach it
    public static class OptionsMiddlewareExtensions
    {
        public static IApplicationBuilder UseOptions(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<OptionsMiddleware>();
        }
    }
}
