using FinancialExpensesAPI.Application.DTOs;
using FinancialExpensesAPI.Domain.Entities;
using FinancialExpensesAPI.Domain.Interfaces;
using BCrypt.Net;

namespace FinancialExpensesAPI.Application.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<UsuarioResponseDto> RegistrarAsync(CreateUsuarioDto usuarioDto)
        {
            var usuarioExiste = await _usuarioRepository.GetByEmailAsync(usuarioDto.Email);
            if (usuarioExiste != null)
            {
                return null;
            }
            var senhaHash = BCrypt.Net.BCrypt.HashPassword(usuarioDto.Senha);

            var usuario = new Usuario
            {
                Nome = usuarioDto.Nome,
                Email = usuarioDto.Email,
                SenhaHash = senhaHash,
                CriadoEm = DateTime.Now
            };

            await _usuarioRepository.AddAsync(usuario);
            await _usuarioRepository.SaveChangesAsync();

            var responseDto = new UsuarioResponseDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                CriadoEm = usuario.CriadoEm
            };
            return responseDto;
        }

        public async Task<UsuarioResponseDto> AutenticarAsync(UsuarioLoginDto loginDto)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(loginDto.Email);
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(loginDto.Senha, usuario.SenhaHash))
            {
                return null;
            }
            var responseDto = new UsuarioResponseDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                CriadoEm = usuario.CriadoEm
            };
            return responseDto;
        }

    }
}
