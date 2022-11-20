using Microsoft.AspNetCore.Mvc;
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
            _accountService.RegisterUser(dto);
            return Ok();
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody]LoginDto dto)
        {
            string token = _accountService.GenerateJwt(dto);
            return Ok(token);
        }
    }

}
