using WMS.Helpers;

namespace WMS.Startup
{
    public static class HelpersSetup
    {
        public static IServiceCollection RegisterHelpers(this IServiceCollection services)
        {
            services.AddScoped<IJournalHelper, JournalHelper>();
            services.AddScoped<IQRHelper, QRHelper>();
            services.AddScoped<IPdfGenerator, PdfGenerator>();
            services.AddScoped<IProductHelper, ProductHelper>();
            services.AddScoped<IProductPlacementHelper, ProductPlacementHelper>();

            return services;
        }
    }
}
