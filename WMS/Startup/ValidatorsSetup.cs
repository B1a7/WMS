using FluentValidation;
using FluentValidation.AspNetCore;
using WMS.Models.Dtos.AccountDtos;
using WMS.Models.Dtos.ProductDtos;
using WMS.Models.Dtos.SupplierDtos;
using WMS.Models.Validators.Account;
using WMS.Models.Validators.AccountValidator;
using WMS.Models.Validators.ProductValidator;
using WMS.Models.Validators.SupplierValidator;

namespace WMS.Startup
{
    public static class ValidatorsSetup
    {
        public static IServiceCollection RegisterValidators(this IServiceCollection services)
        {
            services.AddControllers().AddFluentValidation();

            services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
            services.AddScoped<IValidator<AddProductDto>, AddProductDtoValidator>();
            services.AddScoped<IValidator<ProductQuery>, ProductQueryValidatior>();
            services.AddScoped<IValidator<UpdateProductDto>, UpdateProductDtoValidator>();
            services.AddScoped<IValidator<SupplierQuery>, SupplierQueryValidator>();
            services.AddScoped<IValidator<AddSupplierDto>, AddSupplierDtoValidator>();
            services.AddScoped<IValidator<UpdateSupplierDto>, AddSupplierDtoValidator>();
            services.AddScoped<IValidator<UserRoleDto>, UserRoleDtoValidator>();

            return services;
        }
    }
}
