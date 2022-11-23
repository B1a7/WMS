using FluentValidation.TestHelper;
using WMS.Models.Dtos.SupplierDtos;
using WMS.Models.Entities;
using WMS.Models.Validators.SupplierValidator;
using Xunit;

namespace WMS.IntegrationTests.ValidatorsTests.SupplierValidatorTests
{
    public class SupplierQueryValidatorTests
    {
        public static IEnumerable<object[]> GetSampleValidData()
        {
            var list = new List<SupplierQuery>()
            {
                new SupplierQuery()
                {
                    PageNumber = 1,
                    PageSize = 10
                },
                new SupplierQuery()
                {
                    PageNumber = 2,
                    PageSize = 15
                },
                new SupplierQuery()
                {
                    PageNumber = 22,
                    PageSize = 5,
                    SortBy = nameof(Supplier.Name)
                },
                new SupplierQuery()
                {
                    PageNumber = 12,
                    PageSize = 15,
                    SortBy = nameof(Supplier.PhoneNumber)
                }
            };

            return list.Select(q => new object[] { q });
        }

        public static IEnumerable<object[]> GetSampleInvalidData()
        {
            var list = new List<SupplierQuery>()
            {
                new SupplierQuery()
                {
                    PageNumber = 0,
                    PageSize = 10
                },
                new SupplierQuery()
                {
                    PageNumber = 2,
                    PageSize = 13
                },
                new SupplierQuery()
                {
                    PageNumber = 22,
                    PageSize = 5,
                    SortBy = nameof(Supplier.Products)
                },
                new SupplierQuery()
                {
                    PageNumber = 12,
                    PageSize = 15,
                    SortBy = nameof(Supplier.Address)
                }
            };

            return list.Select(q => new object[] { q });
        }

        [Theory]
        [MemberData(nameof(GetSampleValidData))]
        public void Validate_ForCorrectModel_ReturnsSuccess(SupplierQuery model)
        {
            // arrange
            var validator = new SupplierQueryValidator();


            // act
            var result = validator.TestValidate(model);

            // assert
            result.ShouldNotHaveAnyValidationErrors();
        }


        [Theory]
        [MemberData(nameof(GetSampleInvalidData))]
        public void Validate_ForIncorrectModel_ReturnsFailure(SupplierQuery model)
        {
            // arrange
            var validator = new SupplierQueryValidator();


            // act
            var result = validator.TestValidate(model);

            // assert
            result.ShouldHaveAnyValidationError();
        }
    }
}
