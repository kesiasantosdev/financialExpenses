using FinancialExpensesAPI.Domain.Entities;
using FinancialExpensesAPI.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FinancialExpensesAPI.Application.DTOs;
using FinancialExpensesAPI.Application.Services;
using Microsoft.AspNetCore.Authorization;


namespace FinancialExpensesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DespesaController : ControllerBase
    {
        private readonly DespesaService _despesaService;

        public DespesaController(DespesaService despesaService)
        {
            _despesaService = despesaService;
        }

        [HttpPost]
        public async Task<IActionResult> CriarDespesa([FromBody] CreateUpdateDespesaDto despesaDto)
        {
            var despesaResposta = await _despesaService.AddAsync(despesaDto);
            return Ok(despesaResposta);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var despesasDto = await _despesaService.GetDtosAsync();
            return Ok(despesasDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarDespesa(int id, [FromBody] CreateUpdateDespesaDto despesaDto)
        {
            var despesaResposta = await _despesaService.AtualizarDespesa(id, despesaDto);
            if (despesaResposta == null)
            {
                return NotFound();
            }
            return Ok(despesaResposta);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var sucesso = await _despesaService.DeletarDespesa(id);

            if (!sucesso)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
