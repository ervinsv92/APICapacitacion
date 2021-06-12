using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace APICapacitacion.API.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class HeadsMiddleware
    {
        private readonly RequestDelegate _next;

        public HeadsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                string clave1 = httpContext.Request.Headers["Clave1"];
                string clave2 = httpContext.Request.Headers["Clave2"];
                string clave3 = httpContext.Request.Headers["Clave3"];

                if (clave1 != null && clave1.Trim() != "" && clave2 != null && clave2.Trim() != "" && clave3 != null && clave3.Trim() != "")
                {
                    await _next(httpContext);
                }
                else {
                    httpContext.Response.StatusCode = 401;
                    await httpContext.Response.WriteAsync("Acceso denegado");
                    return;
                }
            }
            catch (Exception)
            {
                httpContext.Response.StatusCode = 401;
                await httpContext.Response.WriteAsync("Acceso denegado");
                return;
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class HeadsMiddlewareExtensions
    {
        public static IApplicationBuilder UseHeadsMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HeadsMiddleware>();
        }
    }
}
