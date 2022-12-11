using WMS.Middleware;

namespace WMS.Startup
{
    public static class MiddlewareSetup
    {
        public static IServiceCollection RegisterMiddleware(this IServiceCollection services)
        {
            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddScoped<RequestTimeMiddleware>();

            return services;
        }

        public static WebApplication ConfigureMiddleware(this WebApplication app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<RequestTimeMiddleware>();

            return app;
        }
    }
}
