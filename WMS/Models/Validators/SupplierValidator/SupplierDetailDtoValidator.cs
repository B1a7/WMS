using FluentValidation;
using System.Text.RegularExpressions;
using WMS.Models.Dtos.SupplierDtos;

namespace WMS.Models.Validators.SupplierValidator
{
    public class SupplierDetailDtoValidator : AbstractValidator<SupplierDetailDto>
    {
        public SupplierDetailDtoValidator()
        {
            RuleFor(x => x.Name)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty().WithMessage("{PropertyName} cannot be empty")
               .Length(2, 50).WithMessage("{PropertyName} invalid length");

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty")
                .EmailAddress().WithMessage("{PropertyName} wrong format");

            RuleFor(x => x.PhoneNumber)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty")
                .MinimumLength(9).WithMessage("PhoneNumber must not be less than 9 characters.")
                .MaximumLength(20).WithMessage("PhoneNumber must not exceed 9 characters.")
                .Matches(new Regex(@"/\(?([0-9]{3})\)?([ .-]?)([0-9]{3})\2([0-9]{3})/"))
                .WithMessage("PhoneNumber not valid format");

            RuleFor(x => x.City)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty")
                .Length(2, 50).WithMessage("{PropertyName} invalid length")
                .Must(BeValidName).WithMessage("{PropertyName} invalid syntax");

            RuleFor(x => x.Street)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty")
                .Length(2, 50).WithMessage("{PropertyName} invalid length");

            RuleFor(x => x.Country)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty")
                .Length(2, 50).WithMessage("{PropertyName} invalid length");

            RuleFor(x => x.PostalCode)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty")
                .Matches(new Regex(@"([0-9]{2})-([0-9]{3})")).WithMessage("Invalid {PropertyName} (00-000)");
        }

        protected static bool BeValidName(string name)
        {
            name = name.Replace("-", "");
            name = name.Replace(" ", "");

            return name.All(Char.IsLetter);
        }
    }
}
