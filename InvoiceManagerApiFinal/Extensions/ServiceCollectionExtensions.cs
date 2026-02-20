using InvoiceManagerApiFinal.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using InvoiceManagerApi.Mappings;
using InvoiceManagerApiFinal.Services.Implementations;
using InvoiceManagerApiFinal.Services.Interfaces;
using InvoiceManagerApi.Services.Implementations;
using InvoiceManagerApi.Services.Interfaces;
using FluentValidation.AspNetCore;
using FluentValidation;
using InvoiceManagerApi.Data;
using Microsoft.EntityFrameworkCore;

namespace InvoiceManagerApiFinal.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwagger(
         this IServiceCollection services
         )
    {
        services.AddControllers()
            .AddJsonOptions(o =>
            o.JsonSerializerOptions.Converters.Add(
                new JsonStringEnumConverter(
                    JsonNamingPolicy.CamelCase,
                    allowIntegerValues: false
                )
            ));

        services.AddOpenApi();

        services.AddSwaggerGen(
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

                // JWT configuration for swagger
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = """
                                    JWT Authorization header 
                                    using Bearer scheme.
                                    Example: Bearer {token}
                                  """,
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                }, Array.Empty<string>()
            }
                });
            }
            );

        return services;
    }

    public static IServiceCollection AddAuthenticationAndAuthorization(
        this IServiceCollection services,
        IConfiguration configuration
        )
    {
        // JWT Authentication
        var jwtConfig = new JwtConfig();
        configuration.GetSection(JwtConfig.SectionName).Bind(jwtConfig);
        services.Configure<JwtConfig>(configuration.GetSection(JwtConfig.SectionName));
        services
            .AddAuthentication(
            options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(
            options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidAudience = jwtConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey)),
                    ClockSkew = TimeSpan.Zero
                };
            }
            );

        services.AddAuthorization();

        return services;
    }

    public static IServiceCollection AddInvoiceManagerDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");

        services.AddDbContext<InvoiceManagerDbContext>(option =>
        option.UseSqlServer(connectionString));

        return services;
    }

    public static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<Program>();
        services.AddFluentValidationAutoValidation();

        return services;
    }

    public static IServiceCollection AddAutoMapperAndOtherServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));

        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<IInvoiceRowService, InvoiceRowService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
