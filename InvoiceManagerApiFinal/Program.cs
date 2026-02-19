using FluentValidation;
using FluentValidation.AspNetCore;
using InvoiceManagerApi.Data;
using InvoiceManagerApi.DTOs.CustomerDTOs;
using InvoiceManagerApi.Mappings;
using InvoiceManagerApi.Services.Implementations;
using InvoiceManagerApi.Services.Interfaces;
using InvoiceManagerApi.Validators.CustomerValidators;
using InvoiceManagerApiFinal.Middlewares;
using InvoiceManagerApiFinal.Models;
using InvoiceManagerApiFinal.Services.Implementations;
using InvoiceManagerApiFinal.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(o =>
    o.JsonSerializerOptions.Converters.Add(
        new JsonStringEnumConverter(
            JsonNamingPolicy.CamelCase,
            allowIntegerValues: false
        )
    ));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddSwaggerGen(
    options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Invoice Manager API",
            Description = "API for managing invoices. API allows create, change, assign invoices to costumers, as well as adjust their rows",
            Contact = new OpenApiContact
            {
                Name = "Invoice Manager Individia",
                Email = "sahib.quliyev000@gmail.com"
            },
            License = new OpenApiLicense
            {
                Name = "MIT License",
                Url = new Uri("https://mit-license.org/")
            }
        });

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

        if (File.Exists(xmlPath))
        {
            options.IncludeXmlComments(xmlPath);
        }
        
    });

var connectionString = builder.Configuration
    .GetConnectionString("Default");

builder.Services.AddDbContext<InvoiceManagerDbContext>(option => 
    option.UseSqlServer(connectionString));

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<InvoiceManagerDbContext>();

builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IInvoiceRowService, InvoiceRowService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(
        options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Invoice Manager API v1");
            //options.RoutePrefix = string.Empty;
            options.DisplayRequestDuration();
            options.EnableFilter();
            options.EnableTryItOutByDefault();
        });
    app.MapOpenApi();
}

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
