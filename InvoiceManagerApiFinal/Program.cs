using FluentValidation.AspNetCore;
using InvoiceManagerApiFinal.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSwagger()
    .AddIdentityAndDb()
    .AddAuthenticationAndAuthorization(builder.Configuration)
    .AddInvoiceManagerDbContext(builder.Configuration)
    .AddFluentValidation()
    .AddAutoMapperAndOtherServices();

var app = builder.Build();

app.UseInvoiceManagerPipeline();

app.Run();
