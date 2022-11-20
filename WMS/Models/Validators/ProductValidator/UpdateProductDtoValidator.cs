using FluentValidation;
using WMS.Models.Dtos.ProductDtos;

namespace WMS.Models.Validators.ProductValidator
{
    public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductDtoValidator()
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty")
                .Length(2, 50).WithMessage("{PropertyName} invalid length")
                .Must(BeValidName).WithMessage("{PropertyName} invalid syntax");

            RuleFor(x => x.Quantity)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("{PropertyName} cannot be empty")
                .GreaterThanOrEqualTo(-1).WithMessage("{PropertyName} cannot be less than zero");

            RuleFor(x => x.CategoryName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty")
                .Length(2, 50).WithMessage("{PropertyName} invalid length");

            RuleFor(x => x.HSCode)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty")
                .Length(2, 50).WithMessage("{PropertyName} invalid length");
        }

        private bool BeValidName(string name)
        {
            name = name.Replace("-", "");
            name = name.Replace(" ", "");

            return name.All(Char.IsLetter);
        }

    }
}
