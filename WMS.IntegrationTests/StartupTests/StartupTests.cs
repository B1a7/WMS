using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using WMS.Controllers;
using Xunit;

namespace WMS.IntegrationTests.Startup
{
    public class StartupTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly List<Type> _controllerTypes;
        private readonly WebApplicationFactory<Program> _factory;

        public StartupTests(WebApplicationFactory<Program> factory)
        {
            _controllerTypes = typeof(Program)
                .Assembly
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(ControllerBase)))
                .ToList();

            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    _controllerTypes.ForEach(c => services.AddScoped(c));
                });
            });
        }

        //[Fact]
        //public void ConfigureServices_ForControllers_RegistersAllDependencies()
        //{
        //    //arrange
        //    var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        //    using var scope = scopeFactory.CreateScope();


        //    // assert
        //    _controllerTypes.ForEach(t =>
        //    {
        //        var controller = scope.ServiceProvider.GetService(t);
        //        controller.Should().NotBeNull();
        //    });
        //}
    }
}
