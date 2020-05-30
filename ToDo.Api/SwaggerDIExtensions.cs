using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace ToDo.Api
{
    /// <summary>
    /// Contains Swagger DI Extensions.
    /// </summary>
    public static class SwaggerDIExtensions
    {
        /// <summary>
        /// Adds swagger to DI Container.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="asm"></param>
        /// <param name="appName"></param>
        /// <param name="appVersion"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwagger(
            this IServiceCollection services,
            Assembly asm,
            string appName = null,
            string appVersion = null)
        {
            var asmName = asm.GetName();
            var actualName = appName ?? asmName.Name;
            var actualVersion = appVersion ?? asmName.Version?.ToString() ?? "v1";

            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc(actualVersion, new OpenApiInfo { Title = actualName, Version = actualVersion });

                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                    });

                    var xmlFile = $"{asm.GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);
                });

            return services;
        }

        /// <summary>
        /// Adds Swagger middleware with UI.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="asm"></param>
        /// <param name="appName"></param>
        /// <param name="appVersion"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSwaggerWithUI(
            this IApplicationBuilder app,
            Assembly asm,
            string appName = null,
            string appVersion = null)
        {
            var asmName = asm.GetName();
            var actualName = appName ?? asmName.Name;
            var actualVersion = appVersion ?? asmName.Version?.ToString() ?? "v1";

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{actualVersion}/swagger.json", $"{asmName} {actualVersion}");
            });

            return app;
        }
    }
}