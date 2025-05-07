using DTOs;
using Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Controllers{
    
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase{

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService){
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResultDTO>> Login(LoginRequestDTO loginData){
            var response = await _authService.LoginAsync(loginData.Email, loginData.Password);

            if(response == null){
                return NotFound("User not found!");
            }

            return response;
        }

    }
}