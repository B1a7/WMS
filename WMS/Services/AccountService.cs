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
        Task<bool> RegisterUserAsync(RegisterUserDto dto, string loggedUserId);
        string GenerateJwt(LoginDto dto);
        Task<bool> ChangeUserRoleAsync(int id, UserRoleDto newRole, string loggedUserId);
        Task<bool> DeleteUserAsync(int id, string loggedUserId);
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
      
        
        public async Task<bool> RegisterUserAsync(RegisterUserDto dto, string loggedUserId)
        {
            var newUser = new User()
            {
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                RoleId = dto.RoleId
            };

            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
            newUser.PasswordHash = hashedPassword;


            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();

            var result = await _journalHelper.CreateJournalAsync(OperationTypeEnum.Register, newUser.GetType().Name.ToString(), newUser.Id, loggedUserId);

            return true;
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

        public  Task<bool> ChangeUserRoleAsync(int id, UserRoleDto newRole, string loggedUserId)
        {

            var userRoleId = (int)(UserRoleEnum)Enum.Parse(typeof(DocumentTypesEnum), newRole.Role.ToLower());

            var user = _dbContext.Users
                .First(x => x.Id == id);
        
            user.RoleId = userRoleId;

            var result =  _journalHelper.CreateJournalAsync(OperationTypeEnum.Add, user.GetType().Name.ToString(), user.Id, loggedUserId);

            return result;
        }

        public Task<bool> DeleteUserAsync(int id, string loggedUserId)
        {
            User user = new User()
            {
                Id = id
            };

            _dbContext.Entry(user).State = EntityState.Deleted;

            var result = _journalHelper.CreateJournalAsync(OperationTypeEnum.Delete, user.GetType().Name.ToString(), user.Id, loggedUserId);

            return result;
        }
    }
}
