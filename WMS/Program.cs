using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
using WMS.Models.Validators.AccountValidator;
using WMS.Models.Validators.ProductValidator;
using WMS.Models.Validators.SupplierValidator;
using WMS.Services;
using WMS.Startup;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.RegisterAuthentication();

builder.RegisterLogging();

builder.Services.AddAuthorization();

builder.Services.RegisterValidators();

builder.Services.RegisterMiddleware();

builder.Services.RegisterSwagger();

builder.Services.AddDbContext<WMSDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("WMSConnection")));

builder.Services.AddScoped<Seeder>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.RegisterServices();

builder.Services.RegisterHelpers();

builder.Services.RegisterCors(builder.Configuration);


var app = builder.Build();

app.RegisterRequestPipeline();

app.GenerateFakeData();

app.UseStaticFiles();

app.ConfigureMiddleware();

app.UseAuthentication();

app.UseHttpsRedirection();

app.ConfigureSwagger();

app.UseRouting();

app.UseAuthorization();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

public partial class Program { }