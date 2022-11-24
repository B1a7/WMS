using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using WMS.Models;
using WMS.Models.Dtos.ProductDtos;
using WMS.Models.Entities;
using WMS.Models.Validators.ProductValidator;
using Xunit;

namespace WMS.IntegrationTests.ValidatorsTests.ProductValidatorTests
{
    public class AddProductDtoValidatorTests
    {
        private WMSDbContext _dbContext;

        public AddProductDtoValidatorTests()
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
                Email= "test@test.com",
                Name = "test",
                PhoneNumber = "111222333"
            };

            _dbContext.Suppliers.AddRange(testSupplier);
            _dbContext.SaveChanges();
        }

        [Fact]
        public void Validate_ForValidModel_ReturnsSucces()
        {
            // arrange

            var model = new AddProductDto()
            {
                Size = "small",
                Name= "TestName",
                ProductionDate = DateTime.Now,
                HSCode= "111111111",
                Quantity = 1,
                CategoryName = "TestCategory",
                SupplierId = 1,
            };

            var validator = new AddProductDtoValidator(_dbContext);

            //act
            var result = validator.TestValidate(model);
            
            //assert

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_ForInvalidModel_ReturnsFailure()
        {
            // arrange

            var dto = new AddProductDto()
            {
                Size = "large",
                Name = "TesttName",
                ProductionDate = DateTime.Now,
                HSCode = "2111111111",
                Quantity = 1,
                CategoryName = "TesttCategory",
                SupplierId = 5,
            };

            var validator = new AddProductDtoValidator(_dbContext);

            //act
            var result = validator.TestValidate(dto);
            //assert

            result.ShouldHaveAnyValidationError();
        }
    }
}
