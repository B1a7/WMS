using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Moq;
using WMS.IntegrationTests.Helpers;
using WMS.Models;
using WMS.Models.Dtos.AccountDtos;
using WMS.Services;
using Xunit;

namespace WMS.IntegrationTests.ControllersTests
{
    public class AccountControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private HttpClient _client;
        private Mock<IAccountService> _accountServiceMock = new Mock<IAccountService>();

        public AccountControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services
                        .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<WMSDbContext>));

                        services.Remove(dbContextOptions);

                        services.AddSingleton<IAccountService>(_accountServiceMock.Object);

                        services
                        .AddDbContext<WMSDbContext>(option => option.UseInMemoryDatabase("WMSDb"));
                    });
                })
                .CreateClient();

        }


        [Fact]
        public async Task Login_ForRegisteredUser_
            ReturnsOk()
        {
            //arrange 

            _accountServiceMock
                .Setup(e => e.GenerateJwt(It.IsAny<LoginDto>()))
                .Returns("jwt");

            var loginDto = new LoginDto()
            {
                Email = "test@test.com",
                Password = "password123"
            };

            var httpContent = loginDto.ToJsonHttpContent();

            //act

            var response = await _client.PostAsync("api/account/login", httpContent);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        }

        [Fact]
        public async Task RegisterUser_ForValidModel_ReturnsOk()
        {
            // arrange

            var registerUser = new RegisterUserDto()
            {
                FirstName = "test",
                LastName = "test",
                Email = "test1@test.com",
                Password = "password123",
                ConfirmPassword = "password123"
            };

            var httpContent = registerUser.ToJsonHttpContent();

            // act

            var response = await _client.PostAsync("/api/account/register", httpContent);

            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }


        [Fact]
        public async Task Login_ForUnregisteredUser_ReturnsBadRequest()
        {
            //arrange 
            var registeredLoginDto = new LoginDto()
            {
                Email = "test@test.com",
                Password = "password123"
            };

            _accountServiceMock
                .Setup(e => e.GenerateJwt(registeredLoginDto))
                .Returns("jwt");

            var wrongLoginDto = new LoginDto()
            {
                Email = "test@test.com",
                Password = ""
            };

            var httpContent = wrongLoginDto.ToJsonHttpContent();

            //act

            var response = await _client.PostAsync("api/account/login", httpContent);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        }

        [Fact]
        public async Task RegisterUser_ForInvalidModel_ReturnsBadRequest()
        {
            // arrange

            var registerUser = new RegisterUserDto()
            {
                Password = "password123",
                ConfirmPassword = "123"
            };

            var httpContent = registerUser.ToJsonHttpContent();

            // act

            var response = await _client.PostAsync("/api/account/register", httpContent);

            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }
}
