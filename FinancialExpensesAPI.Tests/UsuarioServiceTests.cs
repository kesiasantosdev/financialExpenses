using Moq;
using FluentAssertions;
using FinancialExpensesAPI.Domain.Interfaces;
using FinancialExpensesAPI.Application.Services;
using FinancialExpensesAPI.Domain.Entities;
using FinancialExpensesAPI.Application.DTOs;
using System.Threading.Tasks;
using Xunit;

namespace FinancialExpensesAPI.Tests
{
    public class UsuarioServiceTests
    {
        [Fact] 
        public async Task AutenticarAsync_QuandoUsuarioNaoExiste_DeveRetornarNull()
        {
            var loginDto = new UsuarioLoginDto { Email = "teste@email.com", Senha = "123" };

            var mockRepository = new Mock<IUsuarioRepository>();

            mockRepository.Setup(repo => repo.GetByEmailAsync(It.IsAny<string>()))
                          .ReturnsAsync((Usuario)null);

            var usuarioService = new UsuarioService(mockRepository.Object);

            var resultado = await usuarioService.AutenticarAsync(loginDto);

            resultado.Should().BeNull();
        }


        [Fact]
        public async Task AutenticarAsync_QuandoUsuarioExisteESenhaEstaCorreta_DeveRetornarUsuarioDto()
        {

            var loginDto = new UsuarioLoginDto { Email = "usuario.valido@email.com", Senha = "senha_correta_123" };
            var senhaHash = BCrypt.Net.BCrypt.HashPassword(loginDto.Senha);

            var usuarioDoBanco = new Usuario
            {
                Id = 1,
                Nome = "Usuário de Teste",
                Email = loginDto.Email,
                SenhaHash = senhaHash,
                CriadoEm = DateTime.UtcNow
            };
            var mockRepository = new Mock<IUsuarioRepository>();


            mockRepository.Setup(repo => repo.GetByEmailAsync(loginDto.Email))
                            .ReturnsAsync(usuarioDoBanco);


            var usuarioService = new UsuarioService(mockRepository.Object);


            var resultado = await usuarioService.AutenticarAsync(loginDto);


            resultado.Should().NotBeNull();
            resultado.Should().BeOfType<UsuarioResponseDto>(); 
            resultado.Id.Should().Be(usuarioDoBanco.Id); 
            resultado.Email.Should().Be(loginDto.Email); 
        }
    }
}
