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
        public async Task<ActionResult> RegisterUserAsync([FromBody] RegisterUserDto dto)
        {
            string loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _accountService.RegisterUserAsync(dto, loggedUserId);
            return Ok(result);
        }

        [HttpPut("user/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> ChangeUserRoleAsync([FromRoute] int id, [FromBody]UserRoleDto dto)
        {
            string loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _accountService.ChangeUserRoleAsync(id, dto, loggedUserId);

            return Ok(result);
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody]LoginDto dto)
        {
            string token = _accountService.GenerateJwt(dto);
            return Ok(token);
        }


        [HttpDelete("user/{id}")]
        public async Task<ActionResult> DeleteUserAsync([FromRoute] int id)
        {
            string loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _accountService.DeleteUserAsync(id, loggedUserId);

            return Ok(result);
        }
    }

}
