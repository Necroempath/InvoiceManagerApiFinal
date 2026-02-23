using InvoiceManagerApiFinal.Middlewares;

namespace InvoiceManagerApiFinal.Extensions;

public static class PipelineExtensionis
{
    public static WebApplication UseInvoiceManagerPipeline(this WebApplication app)
    {

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

        return app;
    }
}
