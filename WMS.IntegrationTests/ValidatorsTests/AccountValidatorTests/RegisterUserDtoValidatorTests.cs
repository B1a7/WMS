using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using WMS.Models;
using WMS.Models.Dtos.AccountDtos;
using WMS.Models.Entities;
using WMS.Models.Validators.Account;
using Xunit;

namespace WMS.IntegrationTests.ValidatorsTests.AccountValidatorTests
{
    public class RegisterUserDtoValidatorTests
    {
        private WMSDbContext _dbContext;

        public RegisterUserDtoValidatorTests()
        {
            var builder = new DbContextOptionsBuilder<WMSDbContext>();
            builder.UseInMemoryDatabase("Test");

            _dbContext = new WMSDbContext(builder.Options);
            Seed();
        }

        public void Seed()
        {
            var testUsers = new List<User>()
            {
                new User()
                {
                    Email = "test2@test.com"
                },
                 new User()
                {
                    Email = "test3@test.com"
                },
            };

            _dbContext.Users.AddRange(testUsers);
            _dbContext.SaveChanges();
        }


        [Fact]
        public void Validate_ForValidModel_ReturnsSuccess()
        {
            // arrange

            var model = new RegisterUserDto()
            {
                Email = "test@test.com",
                Password = "Password123",
                ConfirmPassword = "Password123",
            };

            var validator = new RegisterUserDtoValidator(_dbContext);

            // act
            var result = validator.TestValidate(model);

            // assert

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_ForInvalidModel_ReturnsFailure()
        {
            // arrange

            var model = new RegisterUserDto()
            {
                Email = "test2@test.com",
                Password = "password123",
                ConfirmPassword = "password123",
            };

            var validator = new RegisterUserDtoValidator(_dbContext);

            // act
            var result = validator.TestValidate(model);

            // assert

            result.ShouldHaveAnyValidationError();
        }
    }
}
