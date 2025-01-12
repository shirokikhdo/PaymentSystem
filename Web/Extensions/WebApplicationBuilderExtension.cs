using Application.Abstractions;
using Application.Services;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Web.Extensions;

internal static class WebApplicationBuilderExtension
{
    internal static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "PaymentSystem API",
                Version = "v1"
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer",
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return builder;
    }

    internal static WebApplicationBuilder AddData(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<PaymentSystemDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection")));

        return builder;
    }

    internal static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ICartsService, CartsService>();
        builder.Services.AddScoped<IOrdersService, OrdersService>();
        
        return builder;
    }
}