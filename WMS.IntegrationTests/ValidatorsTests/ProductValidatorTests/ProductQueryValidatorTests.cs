using FluentValidation.TestHelper;
using WMS.Models.Dtos.ProductDtos;
using WMS.Models.Entities;
using WMS.Models.Validators.ProductValidator;
using Xunit;

namespace WMS.IntegrationTests.ValidatorsTests.ProductValidatorTests
{
    public class ProductQueryValidatorTests
    {
        public static IEnumerable<object[]> GetSampleValidData()
        {
            var list = new List<ProductQuery>()
            {
                new ProductQuery()
                {
                    PageNumber = 1,
                    PageSize = 10
                },
                new ProductQuery()
                {
                    PageNumber = 2,
                    PageSize = 15
                },
                new ProductQuery()
                {
                    PageNumber = 22,
                    PageSize = 5,
                    SortBy = nameof(Product.Size)
                },
                new ProductQuery()
                {
                    PageNumber = 12,
                    PageSize = 15,
                    SortBy = nameof(Product.Name)
                }
            };

            return list.Select(q => new object[] { q });
        }

        public static IEnumerable<object[]> GetSampleInvalidData()
        {
            var list = new List<ProductQuery>()
            {
                new ProductQuery()
                {
                    PageNumber = 0,
                    PageSize = 10
                },
                new ProductQuery()
                {
                    PageNumber = 2,
                    PageSize = 13
                },
                new ProductQuery()
                {
                    PageNumber = 22,
                    PageSize = 5,
                    SortBy = nameof(Product.LayoutId)
                },
                new ProductQuery()
                {
                    PageNumber = 12,
                    PageSize = 15,
                    SortBy = nameof(Product.Id)
                }
            };

            return list.Select(q => new object[] { q });
        }

        [Theory]
        [MemberData(nameof(GetSampleValidData))]
        public void Validate_ForCorrectModel_ReturnsSuccess(ProductQuery model)
        {
            // arrange
            var validator = new ProductQueryValidatior();


            // act
            var result = validator.TestValidate(model);

            // assert
            result.ShouldNotHaveAnyValidationErrors();
        }


        [Theory]
        [MemberData(nameof(GetSampleInvalidData))]
        public void Validate_ForIncorrectModel_ReturnsFailure(ProductQuery model)
        {
            // arrange
            var validator = new ProductQueryValidatior();


            // act
            var result = validator.TestValidate(model);

            // assert
            result.ShouldHaveAnyValidationError();
        }
    }
}
