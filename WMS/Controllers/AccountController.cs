using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WMS.Models.Dtos.AccountDtos;
using WMS.Services;

namespace WMS.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    { 
        private IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        [HttpPost("register")]
        public ActionResult RegisterUser([FromBody] RegisterUserDto dto)
        {
            string loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _accountService.RegisterUser(dto, loggedUserId);
            return Ok();
        }

        [HttpPut("user/{id}")]
        [Authorize(Roles = "admin")]
        public ActionResult ChangeUserRole([FromRoute] int id, [FromBody]UserRoleDto dto)
        {
            string loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _accountService.ChangeUserRole(id, dto, loggedUserId);

            return Ok();
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody]LoginDto dto)
        {
            string token = _accountService.GenerateJwt(dto);
            return Ok(token);
        }


        [HttpDelete("user/{id}")]
        public ActionResult DeleteUser([FromRoute] int id)
        {
            string loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _accountService.DeleteUser(id, loggedUserId);

            return NoContent();
        }
    }

}
