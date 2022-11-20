using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using System.Reflection;
using System.Text;
using WMS;
using WMS.Helpers;
using WMS.Middleware;
using WMS.Models;
using WMS.Models.Dtos.AccountDtos;
using WMS.Models.Dtos.ProductDtos;
using WMS.Models.Dtos.SupplierDtos;
using WMS.Models.Entities;
using WMS.Models.Validators.Account;
using WMS.Models.Validators.ProductValidator;
using WMS.Models.Validators.SupplierValidator;
using WMS.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var authenticationSetttings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authenticationSetttings);
builder.Services.AddSingleton(authenticationSetttings);
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = authenticationSetttings.JwtIssuer,
        ValidAudience = authenticationSetttings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSetttings.JwtKey)),
    };
});
builder.Services.AddAuthorization();

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog();
builder.Services.AddControllers().AddFluentValidation();

builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<RequestTimeMiddleware>();

builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<WMSDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("WMSConnection")));
builder.Services.AddScoped<Seeder>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IDocumentationService, DocumentationService>();
builder.Services.AddScoped<ILayoutService, LayoutService>();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
builder.Services.AddScoped<IValidator<AddProductDto>, AddProductDtoValidator>();
builder.Services.AddScoped<IValidator<ProductQuery>, ProductQueryValidatior>();
builder.Services.AddScoped<IValidator<UpdateProductDto>, UpdateProductDtoValidator>();
builder.Services.AddScoped<IValidator<SupplierQuery>, SupplierQueryValidator>();
builder.Services.AddScoped<IValidator<AddSupplierDto>, AddSupplierDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateSupplierDto>, AddSupplierDtoValidator>();

builder.Services.AddScoped<IJournalHelper, JournalHelper>();
builder.Services.AddScoped<IQRHelper, QRHelper>();
builder.Services.AddScoped<IPdfGenerator,PdfGenerator>();
builder.Services.AddScoped<IProductHelper, ProductHelper>();
builder.Services.AddScoped<IProductPlacementHelper, ProductPlacementHelper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var scope = app.Services.CreateScope();
var generator = scope.ServiceProvider.GetRequiredService<Seeder>();
//generator.GenerateData();


app.UseStaticFiles();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestTimeMiddleware>();
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WMS");
});
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
