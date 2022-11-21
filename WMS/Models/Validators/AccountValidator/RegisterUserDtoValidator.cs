using FluentValidation;
using WMS.Models.Dtos.AccountDtos;

namespace WMS.Models.Validators.Account
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator(WMSDbContext dbContext)
        {
            RuleFor(x => x.FirstName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty")
                .Length(2, 50).WithMessage("{PropertyName} invalid length")
                .Must(BeValidName).WithMessage("{PropertyName} invalid syntax");

            RuleFor(x => x.LastName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty")
                .Length(2, 50).WithMessage("{PropertyName} invalid length")
                .Must(BeValidName).WithMessage("{PropertyName} invalid syntax");

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .MinimumLength(8);

            RuleFor(x => x.ConfirmPassword)
                .Equal(y => y.Password);

            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    var typedEmail = dbContext.Users.Any(u => u.Email == value);
                    if (typedEmail)
                        context.AddFailure("Email", "That email is taken");
                });

        }

        private bool BeValidName(string name)
        {
            name = name.Replace("-", "");
            name = name.Replace(" ", "");

            return name.All(Char.IsLetter);
        }
    }
}
