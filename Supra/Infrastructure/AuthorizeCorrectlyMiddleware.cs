using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Supra.Infrastructure
{
    public class AuthorizeCorrectlyMiddleware
    {
        readonly RequestDelegate next;

        public AuthorizeCorrectlyMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await next(context);

            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.ContentType = "application/json";
                var ListHeaders = context.Response.Headers.Values.ToList();
                var autorizaStatus = ListHeaders[0].ToString();
                string Response = "";

                if (autorizaStatus.Contains("The token is expired"))
                {
                    Response = JsonConvert.SerializeObject("expired");
                }
                else if (!context.User.Identity.IsAuthenticated)
                {
                    Response = JsonConvert.SerializeObject("azzzzzzzzzzzz");
                }
                await context.Response.WriteAsync(Response, Encoding.UTF8);
                return;
            }
        }
    }
}
