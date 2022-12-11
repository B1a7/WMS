using WMS.Models;

namespace WMS.Startup
{
    public static class DataGeneratorSetup
    {
        public static void GenerateFakeData(this WebApplication app)
        {
            var scope = app.Services.CreateScope();
            var generator = scope.ServiceProvider.GetRequiredService<Seeder>();
            generator.GenerateData();
        }
    }
}
