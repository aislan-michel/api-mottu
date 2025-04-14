using System.Reflection;
using Microsoft.OpenApi.Models;

using Mottu.Api.Domain.Entities;
using Mottu.Api.Infrastructure.Repositories;
using Mottu.Api.Infrastructure.Repositories.GenericRepository;
using Mottu.Api.Infrastructure.Services.Notifications;
using Mottu.Api.Infrastructure.Services.Storage;
using Mottu.Api.UseCases.DeliveryManUseCases;
using Mottu.Api.UseCases.MotorcycleUseCases;
using Mottu.Api.UseCases.RentUseCases;

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
                        Url = new Uri("https://github.com/aislan-michel"),
                        Email = "aislan.michel92@gmail.com"
                    },
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }

        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Motorcycle>, Repository<Motorcycle>>();
            services.AddScoped<IRepository<DeliveryMan>, Repository<DeliveryMan>>();
            services.AddScoped<IRepository<Rent>, Repository<Rent>>();

            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IStorageService, StorageService>();
        }

        public static void Seed(this IServiceCollection services, IConfiguration configuration)
        {
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                var motorcycleUseCase = serviceProvider.GetRequiredService<IMotorcycleUseCase>();

                motorcycleUseCase.Create(new Models.PostMotorcycleRequest()
                {
                    Year = 2025,
                    Model = "Honda",
                    Plate = "NAV9659"
                });

                var deliveryManUseCase = serviceProvider.GetRequiredService<IDeliveryManUseCase>();

                var driverLicenseImage = configuration.GetValue<string>("Seed:DriverLicenseModel");

                deliveryManUseCase.Create(new Models.PostDeliveryManRequest()
                {
                    Name = "João da Silva",
                    CompanyRegistrationNumber = "71069561000195",
                    DateOfBirth = new DateOnly(1999, 12, 28),
                    DriverLicense = "03503196070",
                    DriverLicenseType = "A",
                    DriverLicenseImage = driverLicenseImage
                });
            }
        }

        public static void AddUseCases(this IServiceCollection services)
        {
            services.AddScoped<IMotorcycleUseCase, MotorcycleUseCase>();
            services.AddScoped<IDeliveryManUseCase, DeliveryManUseCase>();
            services.AddScoped<IRentUseCase, RentUseCase>();
        }
    }
}

