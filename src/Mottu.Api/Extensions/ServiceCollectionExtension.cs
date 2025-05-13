using System.Reflection;
using System.Text;

using FluentValidation;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using Mottu.Api.Application.Interfaces;
using Mottu.Api.Application.UseCases;
using Mottu.Api.Application.Validators;
using Mottu.Api.Domain.Interfaces;
using Mottu.Api.Infrastructure.Interfaces;
using Mottu.Api.Infrastructure.Repositories;
using Mottu.Api.Infrastructure.Services;

namespace Mottu.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options => 
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Mottu API",
                Contact = new OpenApiContact
                {
                    Name = "Aislan Michel Moreira Freitas",
                    Url = new Uri("https://github.com/aislan-michel"),
                    Email = "aislan.michel92@gmail.com"
                },
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Digite o token JWT no campo abaixo."
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
    }

    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddScoped<IStorageService, StorageService>();
    }

    public static void AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<IMotorcycleUseCase, MotorcycleUseCase>();
        services.AddScoped<IDeliveryManUseCase, DeliveryManUseCase>();
        services.AddScoped<IRentUseCase, RentUseCase>();
        services.AddScoped<IAdminUseCase, AdminUseCase>();
    }

    public static void AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<PatchDriverLicenseImageRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<PatchMotorcycleRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<PatchRentRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<PostMotorcycleRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<PostRentRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<RegisterDeliveryManRequestValidator>();
    }

    public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var key = configuration["Jwt:Key"];
        var issuer = configuration["Jwt:Issuer"];

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options => 
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = issuer,
                ValidAudience = issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
            };
        });

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ILoggedUserService, LoggedUserService>();
    }
}

