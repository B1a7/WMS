using FluentValidation;
using WMS.Enums;
using WMS.Models.Dtos.AccountDtos;

namespace WMS.Models.Validators.AccountValidator
{
    public class UserRoleDtoValidator : AbstractValidator<UserRoleDto>
    {
        public UserRoleDtoValidator()
        {
            RuleFor(x => x.Role)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Must(IsValidRole);
              
        }

        private bool IsValidRole(string role)
        {
            return Enum<UserRoleEnum>.IsDefined(role.ToLower());
        }
    }
}
