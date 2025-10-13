using FinancialExpensesAPI.Application.DTOs;
using FinancialExpensesAPI.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinancialExpensesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;
        private readonly TokenService _tokenService;
        public AuthController(UsuarioService usuarioService, TokenService tokenService)
        {
            _usuarioService = usuarioService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUsuarioDto usuarioDto)
        {
            var usuarioResposta = await _usuarioService.RegistrarAsync(usuarioDto);
            if (usuarioResposta == null)
            {
                return Conflict("Email já está em uso.");
            }
            return Ok(usuarioResposta);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDto loginDto)
        {
            var usuarioResposta = await _usuarioService.AutenticarAsync(loginDto);
            if (usuarioResposta == null)
            {
                return Unauthorized("Credenciais inválidas.");
            }
            var token = _tokenService.GenerateToken(usuarioResposta);
            return Ok(new
            {
                token = token
            });
        }
    }
}
