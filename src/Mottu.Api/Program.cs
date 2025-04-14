using Mottu.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddResponseCompression();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddSwagger();
builder.Services.AddInfrastructure();
builder.Services.AddUseCases();
builder.Services.Seed(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options => 
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
}

app.UseResponseCompression();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
