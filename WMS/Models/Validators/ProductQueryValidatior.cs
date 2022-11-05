using FluentValidation;
using WMS.Models.Dtos;

namespace WMS.Models.Validators
{
    public class ProductQueryValidatior : AbstractValidator<ProductQuery>
    {
        private int[] pageSizes = new[] { 5, 10, 15, 30 }; 
        public ProductQueryValidatior()
        {
            
            RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(r => r.PageSize).Custom((value, context) =>
            {
                if (!pageSizes.Contains(value))
                    context.AddFailure("PageSize", $"PageSize must be in [{string.Join(", ", pageSizes)}]");
            });
        }
    }
}
