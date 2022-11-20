using FluentValidation;
using WMS.Models.Dtos.SupplierDtos;
using WMS.Models.Entities;

namespace WMS.Models.Validators.SupplierValidator
{
    public class SupplierQueryValidator : AbstractValidator<SupplierQuery>
    {
        private int[] pageSizes = new[] { 5, 10, 15, 30 };
        private string[] allowedSortByColumnNames =
            {nameof(Supplier.Name), nameof(Supplier.Email), nameof(Supplier.PhoneNumber) };


        public SupplierQueryValidator()
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
