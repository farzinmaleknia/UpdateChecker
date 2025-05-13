global using Api.Models;
global using Api.Services.Updates;
global using Api.Models.ResultClass;
global using Api.DTOs.Updates;
global using Api.Resources;
using Api.Services.Browser;
using System.Text.Json.Serialization;
using Api.Services.Resources;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
    
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUpdateService, UpdateService>();
builder.Services.AddSingleton<BrowserService>();
builder.Services.AddSingleton<ResourcesService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
