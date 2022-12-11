using WMS.Services;

namespace WMS.Startup
{
    public static class ServicesSetup
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IDocumentationService, DocumentationService>();
            services.AddScoped<ILayoutService, LayoutService>();

            return services;
        }
    }
}
