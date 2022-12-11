namespace WMS.Startup
{
    public static class CorsSetup
    {
        public static IServiceCollection RegisterCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("FrontEndClient", builder =>

                    builder.AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithOrigins(configuration["AllowedOrigins"])

                    );
            });

            return services;
        }
    }
}
