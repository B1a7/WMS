using NLog.Web;

namespace WMS.Startup
{
    public static class LoggingSetup
    {
        public static WebApplicationBuilder RegisterLogging(this WebApplicationBuilder builder)
        {
            builder.Logging.ClearProviders();
            builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            builder.Host.UseNLog();

            return builder;
        }
    }
}
