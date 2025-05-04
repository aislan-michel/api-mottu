using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Mottu.Api.Extensions;
using Mottu.Api.Infrastructure.Identity;
using Mottu.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite("Data Source=Data/mottu.db"));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddResponseCompression();
builder.Services.AddOpenApi();

builder.Services.AddSwagger();
builder.Services.AddValidators();
builder.Services.AddInfrastructure();
builder.Services.AddUseCases();
builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.AddControllers();

var app = builder.Build();

await app.ApplyMigrations();
await app.SeedRoles();
app.Seed(app.Configuration);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
}

app.UseCors("AllowAll");

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseResponseCompression();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();