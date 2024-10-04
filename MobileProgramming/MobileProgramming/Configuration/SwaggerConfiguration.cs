﻿using Microsoft.OpenApi.Models;

namespace MobileProgramming.API
{
    public static class SwaggerConfiguration
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new() { Title = "Event Management", Version = "v1" });
                swagger.EnableAnnotations();
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token using the Bearer scheme (\"bearer {token}\")",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                            },
                            new string[]{}
                        }
                    });

            });
        }
    }

    public static class SwaggerUIExtension
    {
        public static void ConfigureSwaggerUI(this IApplicationBuilder app)
        {
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Event Management API v1");
                c.RoutePrefix = "swagger";
                c.EnableFilter();
                c.EnableValidator();
            });
        }
    }
}
