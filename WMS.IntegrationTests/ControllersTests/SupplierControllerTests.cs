using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using WMS.IntegrationTests.Helpers;
using WMS.Models;
using WMS.Models.Dtos.SupplierDtos;
using WMS.Models.Entities;
using Xunit;

namespace WMS.IntegrationTests.ControllersTests
{
    public class SupplierControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private HttpClient _client;
        private WebApplicationFactory<Program> _factory;


        public SupplierControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services
                            .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<WMSDbContext>));

                        services.Remove(dbContextOptions);

                        services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                        services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));


                        services
                         .AddDbContext<WMSDbContext>(options => options.UseInMemoryDatabase("RestaurantDb"));

                    });
                });

            _client = _factory.CreateClient();
        }


        private int SeedSupplier(Supplier supplier)
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetService<WMSDbContext>();

            _dbContext.Suppliers.Add(supplier);
            _dbContext.SaveChanges();
            return _dbContext.Suppliers.First().Id;
        }
        private int SeedProducts(Product product)
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetService<WMSDbContext>();

            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();
            return _dbContext.Suppliers.First().Id;
        }

        [Fact]
        public async Task GetSupplierProduct_WithValidModel_ReturnsCreatedStatus()
        {
            // arrange


            var model = new Supplier()
            {
                Email = "test@test.com",
                Name = "test",
                PhoneNumber = "111222333",
                Products = new List<Product>()
                { 
                    new Product()
                    {
                        Size = "small",
                        Name = "TestName",
                        ProductionDate = DateTime.Now,
                        Quantity = 1,
                    }
                }
            };
            var id = SeedSupplier(model);


            // act
            var response = await _client.GetAsync($"/api/supplier/{id}/products");

            // arrange 

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetById_WithValidModel_ReturnsCreatedStatus()
        {
            // arrange

            var model = new Supplier()
            {
                Email = "test@test.com",
                Name = "test",
                PhoneNumber = "111222333",
            };
            var id = SeedSupplier(model);


            // act
            var response = await _client.GetAsync($"/api/supplier/{id}");

            // arrange 

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }


        [Fact]
        public async Task GetById_WithInvalidModel_ReturnsBadRequest()
        {
            // arrange

            // act
            var response = await _client.GetAsync("/api/supplier/111");

            // arrange

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateSupplier_WithValidModel_ReturnsCreatedStatus()
        {
            // arrange

            var model = new Supplier()
            {
                Email = "test@test.com",
                Name = "test",
                PhoneNumber = "111222333",
            };
            var id = SeedSupplier(model);

            var updatedSupplier = new UpdateSupplierDto()
            {
                Email = "test@test.com",
                Name = "test",
                PhoneNumber = "111222333",
                City = "TestCity",
                Street = "test",
                Country = "Test",
                PostalCode = "22-222"
            };

            var httpContent = updatedSupplier.ToJsonHttpContent();

            // act
            var response = await _client.PutAsync($"/api/supplier/{id}", httpContent);

            // arrange 

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }


        [Fact]
        public async Task UpdateSupplier_WithInvalidModel_ReturnsBadRequest()
        {
            // arrange
            var model = new Supplier()
            {
                Email = "test@test.com",
                Name = "test",
                PhoneNumber = "111222333",
            };
            SeedSupplier(model);

            var updatedSupplier = new UpdateSupplierDto()
            {
            };

            var httpContent = model.ToJsonHttpContent();

            // act
            var response = await _client.PostAsync("/api/supplier", httpContent);

            // arrange

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("pageSize=5&pageNumber=1")]
        [InlineData("pageSize=15&pageNumber=2")]
        [InlineData("pageSize=10&pageNumber=3")]
        public async Task GetAll_WithQueryParameters_ReturnsOkResult(string queryParams)
        {
            // act

            var response = await _client.GetAsync("/api/supplier?" + queryParams);
            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("pageSize=100&pageNumber=3")]
        [InlineData("pageSize=11&pageNumber=1")]
        [InlineData(null)]
        [InlineData("")]
        public async Task GetAll_WithInvalidQueryParams_ReturnsBadRequest(string queryParams)
        {
            // act

            var response = await _client.GetAsync("/api/supplier?" + queryParams);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AddSupplier_WithValidModel_ReturnsCreatedStatus()
        {
            // arrange

            var model = new SupplierDetailDto()
            {
                Email = "test@test.com",
                Name = "test",
                PhoneNumber = "111222333",
                City = "TestCity",
                Street = "test",
                Country = "Test",
                PostalCode = "22-222"
            };

            var httpContent = model.ToJsonHttpContent();

            // act
            var response = await _client.PostAsync("/api/supplier", httpContent);

            // arrange 

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();
        }

        [Fact]
        public async Task AddSupplier_WithInvalidModel_ReturnsBadRequest()
        {
            // arrange
            var model = new AddSupplierDto()
            {          
            };

            var httpContent = model.ToJsonHttpContent();

            // act
            var response = await _client.PostAsync("/api/supplier", httpContent);

            // arrange

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Delete_ForExistingSupplier_ReturnsOk()
        {
            // arrange

            var supplier = new Supplier()
            {
                Email = "test@test.com",
                Name = "test",
                PhoneNumber = "111222333",
            };

            SeedSupplier(supplier);

            // act
            var response = await _client.DeleteAsync("/api/supplier/" + supplier.Id);


            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_ForNonExistingSupplier_ReturnsNotFound()
        {
            // act

            var response = await _client.DeleteAsync("/api/supplier/987");

            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.InternalServerError);

        }
    }
}
