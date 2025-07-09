global using Api.Models;
global using Api.Services.Updates;
global using Api.Models.ResultClass;
global using Api.DTOs.Updates;
global using Api.Resources;
using Api.Services.Browser;
using System.Text.Json.Serialization;
using Api.Services.Resources;
using Microsoft.EntityFrameworkCore;
using Api.Data;
using Api.Services.SecurityQuestions;


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
      options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUpdateService, UpdateService>();
builder.Services.AddScoped<IUpdateLogin, UpdateLogin>();
builder.Services.AddScoped<IUpdateVerification, UpdateVerification>();
builder.Services.AddScoped<IPuppeteerSharpUtilities, PuppeteerSharpUtilities>();
builder.Services.AddScoped<ISecurityQuestionService, SecurityQuestionService>();
builder.Services.AddSingleton<BrowserService>();
builder.Services.AddSingleton<ResourcesService>();

builder.Services.AddCors(options =>
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
          policy.WithOrigins(builder.Configuration["ClientUrl:Url"])
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
        }
    )
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
