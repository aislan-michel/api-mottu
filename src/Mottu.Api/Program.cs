using Mottu.Api.Domain.Entities;
using Mottu.Api.Infrastructure.Notifications;
using Mottu.Api.Infrastructure.Repositories;
using Mottu.Api.Infrastructure.Repositories.GenericRepository;
using Mottu.Api.UseCases.MotorcycleUseCases;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddScoped<IMotorcycleUseCase, MotorcycleUseCase>();

builder.Services.AddScoped<IRepository<Motorcycle>, Repository<Motorcycle>>();
builder.Services.AddScoped<INotificationService, NotificationService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
