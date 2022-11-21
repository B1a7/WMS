using System.ComponentModel.DataAnnotations;

namespace WMS.Models.Dtos.AccountDtos
{
    public class RegisterUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public int RoleId { get; } = 1;


    }
}
