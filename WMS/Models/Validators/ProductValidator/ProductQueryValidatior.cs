using FluentValidation;
using WMS.Models.Dtos.ProductDtos;
using WMS.Models.Entities;

namespace WMS.Models.Validators.ProductValidator
{
    public class ProductQueryValidatior : AbstractValidator<ProductQuery>
    {
        private int[] pageSizes = new[] { 5, 10, 15, 30 };
        private string[] allowedSortByColumnNames =
            {nameof(Product.Name), nameof(Status.PackageStatus), nameof(Product.Quantity), nameof(Product.Size) };


        public ProductQueryValidatior()
        {

            RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(r => r.PageSize).Custom((value, context) =>
            {
                if (!pageSizes.Contains(value))
                    context.AddFailure("PageSize", $"PageSize must be in [{string.Join(", ", pageSizes)}]");
            });

            RuleFor(r => r.SortBy)
                .Must(value => string.IsNullOrEmpty(value) || allowedSortByColumnNames.Contains(value))
               .WithMessage($"Sort by is optional, or must be in [{string.Join(",", allowedSortByColumnNames)}]");
        }
    }

}
