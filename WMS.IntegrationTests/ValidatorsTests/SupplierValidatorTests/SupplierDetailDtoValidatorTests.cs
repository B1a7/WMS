using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using WMS.Models;
using WMS.Models.Dtos.SupplierDtos;
using WMS.Models.Entities;
using WMS.Models.Validators.SupplierValidator;
using Xunit;

namespace WMS.IntegrationTests.ValidatorsTests.SupplierValidatorTests
{
    public class SupplierDetailDtoValidatorTests
    {

        [Fact]
        public void Validate_ForValidModel_ReturnsSucces()
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

            var validator = new SupplierDetailDtoValidator();

            //act
            var result = validator.TestValidate(model);

            //assert

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_ForInvalidModel_ReturnsSucces()
        {
            // arrange

            var model = new SupplierDetailDto()
            {
                Email = "test@test.com",
                Name = "",
                PhoneNumber = "111222333"
            };

            var validator = new SupplierDetailDtoValidator();

            //act
            var result = validator.TestValidate(model);

            //assert

            result.ShouldHaveAnyValidationError();
        }
    }
}
