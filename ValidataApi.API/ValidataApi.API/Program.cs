using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using ValidataApi.Application.Validators;
using ValidataApi.Infrastructure.Data;
using ValidataApi.Infrastructure.UnitOfWork;
using ValidataApi.Infrastructure.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, config) =>
{
    config.WriteTo.Console();
});
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ValidataApi.Application.Commands.CreateCustomerCommand).Assembly));// Add FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Validata API", Version = "v1" });
   
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Validata API v1"));
}
app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();
