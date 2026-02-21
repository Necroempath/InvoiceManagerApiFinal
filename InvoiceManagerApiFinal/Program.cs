using FluentValidation.AspNetCore;
using InvoiceManagerApiFinal.Extensions;
using InvoiceManagerApiFinal.Middlewares;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSwagger()
    .AddIdentityAndDb()
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
