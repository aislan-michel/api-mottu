using System.Reflection;
using Microsoft.OpenApi.Models;

using Mottu.Api.Domain.Entities;
using Mottu.Api.Infrastructure.Repositories;
using Mottu.Api.Infrastructure.Repositories.GenericRepository;
using Mottu.Api.Infrastructure.Services.Notifications;
using Mottu.Api.Infrastructure.Services.Storage;
using Mottu.Api.UseCases.DeliveryManUseCases;
using Mottu.Api.UseCases.MotorcycleUseCases;

namespace Mottu.Api.Extensions
{
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
                        Url = new Uri("https://github.com/aislan-michel")
                    },
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }

        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Motorcycle>, Repository<Motorcycle>>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IStorageService, StorageService>();
        }

        public static void AddUseCases(this IServiceCollection services)
        {
            services.AddScoped<IMotorcycleUseCase, MotorcycleUseCase>();
            services.AddScoped<IDeliveryManUseCase, DeliveryManUseCase>();
        }
    }
}

