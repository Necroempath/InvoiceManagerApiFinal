using FluentValidation.AspNetCore;
using InvoiceManagerApi.Data;
using InvoiceManagerApiFinal.Extensions;
using InvoiceManagerApiFinal.Middlewares;
using InvoiceManagerApiFinal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<InvoiceManagerDbContext>();

builder.Services.AddSwagger()
    .AddAuthenticationAndAuthorization(builder.Configuration)
    .AddInvoiceManagerDbContext(builder.Configuration)
    .AddFluentValidation()
    .AddAutoMapperAndOtherServices();

var app = builder.Build();

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
