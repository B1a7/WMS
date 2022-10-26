using Microsoft.AspNetCore.Identity;
using WMS.Models;
using WMS.Models.Dtos;
using WMS.Models.Entities;

namespace WMS.Services
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto dto);
    }


    public class AccountService : IAccountService
    {
        private readonly WMSDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountService(WMSDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }
        public void RegisterUser(RegisterUserDto dto)
        {
            var newUser = new User()
            {
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                Nationality = dto.Nationality,
                RoleId = dto.RoleId,
            };

            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
            newUser.PasswordHash = hashedPassword;

            _context.Users.Add(newUser);
            _context.SaveChanges();
        }
    }
}
