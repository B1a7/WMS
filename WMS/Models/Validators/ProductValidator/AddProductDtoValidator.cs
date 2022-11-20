using FluentValidation;
using WMS.Enums;
using WMS.Models.Dtos.ProductDtos;

namespace WMS.Models.Validators.ProductValidator
{
    public class AddProductDtoValidator : AbstractValidator<AddProductDto>
    {
        private readonly WMSDbContext _dbContext;

        public AddProductDtoValidator(WMSDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty")
                .Length(2,50).WithMessage("{PropertyName} invalid length")
                .Must(BeValidName).WithMessage("{PropertyName} invalid syntax");

            RuleFor(x => x.Quantity)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("{PropertyName} cannot be empty")
                .GreaterThanOrEqualTo(-1).WithMessage("{PropertyName} cannot be less than zero");

            RuleFor(x => x.ProductionDate)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(BeValidDate).WithMessage("{PropertyName} wrong format")
                .LessThan(p => DateTime.Now).WithMessage("Future date?");

            RuleFor(x => x.Size)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(BeValidSize).WithMessage("Invalid size");

            RuleFor(x => x.SupplierId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(BeValidSupplier).WithMessage("No supplier with such ID in database");


            RuleFor(x => x.IsAvaiable)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("{PropertyName} cannot be null");

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

        private bool BeValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }

        private bool BeValidSize(string size)
        {
            return SpotSize.SpotSizes.Contains(size.ToLower());
        }

        private bool BeValidSupplier(int supplierId)
        {
            return _dbContext.Suppliers.Any(s => s.Id == supplierId);
        }

    }
}
