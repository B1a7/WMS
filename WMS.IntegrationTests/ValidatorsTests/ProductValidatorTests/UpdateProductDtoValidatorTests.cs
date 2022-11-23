using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using WMS.Models;
using WMS.Models.Dtos.ProductDtos;
using WMS.Models.Entities;
using WMS.Models.Validators.ProductValidator;
using Xunit;

namespace WMS.IntegrationTests.ValidatorsTests.AccountValidatorTests
{
    public class UpdateProductDtoValidatorTests
    {
        private WMSDbContext _dbContext;

        public UpdateProductDtoValidatorTests()
        {
            var builder = new DbContextOptionsBuilder<WMSDbContext>();
            builder.UseInMemoryDatabase("Test");

            _dbContext = new WMSDbContext(builder.Options);
            Seed();
        }

        public void Seed()
        {
            var testSupplier = new Supplier()
            {
                Id = 1,
                Email = "test@test.com",
                Name = "test",
                PhoneNumber = "111222333"
            };

            var testProduct = new Product()
            {
                Size = "small",
                Name = "TestName",
                ProductionDate = DateTime.Now,
                Quantity = 1,
                SupplierId = 1,
                IsAvaiable = true,         
            };

            _dbContext.Suppliers.Add(testSupplier);
            _dbContext.Products.Add(testProduct);
            _dbContext.SaveChanges();
        }

        [Fact]
        public void Validate_ForValidModel_ReturnsSucces()
        {
            // arrange

            var model = new UpdateProductDto()
            {
                Name = "TestName",
                HSCode = "111111111",
                Quantity = 1,
                CategoryName = "TestCategory",
            };

            var validator = new UpdateProductDtoValidator();

            //act
            var result = validator.TestValidate(model);

            //assert

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_ForInvalidModel_ReturnsSucces()
        {
            // arrange

            var model = new UpdateProductDto()
            {
                Name = "",
                HSCode = "111111111",
                Quantity = 1,
                CategoryName = "TestCategory",
            };

            var validator = new UpdateProductDtoValidator();

            //act
            var result = validator.TestValidate(model);

            //assert

            result.ShouldHaveAnyValidationError();
        }
    }
}
