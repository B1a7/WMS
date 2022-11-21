using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WMS.Enums;
using WMS.Exceptions;
using WMS.Helpers;
using WMS.Middleware;
using WMS.Models;
using WMS.Models.Dtos.AccountDtos;
using WMS.Models.Entities;

namespace WMS.Services
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto dto, string loggedUserId);
        string GenerateJwt(LoginDto dto);
        void ChangeUserRole(int id, UserRoleDto newRole, string loggedUserId);
        void DeleteUser(int id, string loggedUserId);
    }


    public class AccountService : IAccountService
    {
        private readonly WMSDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly IJournalHelper _journalHelper;

        public AccountService(WMSDbContext dbContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings,
            IJournalHelper journalHelper)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
            _journalHelper = journalHelper;
        }
        public void RegisterUser(RegisterUserDto dto, string loggedUserId)
        {
            var newUser = new User()
            {
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
            };

            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
            newUser.PasswordHash = hashedPassword;


            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();
            _journalHelper.CreateJournal(OperationTypeEnum.Register, newUser.GetType().Name.ToString(), newUser.Id, loggedUserId);
        }

        public string GenerateJwt(LoginDto dto)
        {
            var user = _dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email == dto.Email);
            if (user is null)
                throw new BadRequestException("Invalid user name or password");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                throw new BadRequestException("Invalid user name or password");

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiresDate = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expiresDate,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public void ChangeUserRole(int id, UserRoleDto newRole, string loggedUserId)
        {

            var userRoleId = (int)(UserRoleEnum)Enum.Parse(typeof(DocumentTypesEnum), newRole.Role.ToLower());

            var user = _dbContext.Users.First(x => x.Id == id);
        
            user.RoleId = userRoleId;

            _journalHelper.CreateJournal(OperationTypeEnum.Add, user.GetType().Name.ToString(), user.Id, loggedUserId);
            _dbContext.SaveChanges();

        }

        public void DeleteUser(int id, string loggedUserId)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
                return ;

            _dbContext.Users.Remove(user);
            _journalHelper.CreateJournal(OperationTypeEnum.Delete, user.GetType().Name.ToString(), user.Id, loggedUserId);
            _dbContext.SaveChanges();


        }
    }
}
