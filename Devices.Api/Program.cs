using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Devices.Application;
using Devices.Api.ExceptionHandling;
using Devices.Api.Extensions;
using Devices.Api.ModelBinding;
using Devices.Api.Validators.Devices;
using Devices.Infrastructure;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers(options =>
    {
        options.ModelBinderProviders.Insert(0, new DeviceStateModelBinderProvider());
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter(JsonNamingPolicy.KebabCaseLower));
    });

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateDeviceRequestValidator>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "Devices API",
            Version = "v1",
            Description = "REST API for persisting and managing devices."
        });

    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

await app.ApplyMigrationsAsync();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Devices API v1");
        options.DocumentTitle = "Devices API Documentation";
    });
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

public partial class Program;
