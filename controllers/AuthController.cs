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

        /// <summary>
        /// Realiza o login do usuario, dados enviados pelo corpo da requisição, montado manualmente, futuramente vou implementar para ser enviado por formdata
        /// </summary>
        /// <param name="loginData"> 
        /// Um objeto contendo:
        /// - email: Email do usuario
        /// - password: Senha do Usuario
        /// </param>
        /// <returns>Um objeto com o Token JWT do usuario, o nome dele, e sua ROLE (DEV ou MNG)</returns>
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