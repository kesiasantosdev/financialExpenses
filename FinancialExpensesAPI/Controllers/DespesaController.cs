using FinancialExpensesAPI.Data;
using FinancialExpensesAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace FinancialExpensesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DespesaController : ControllerBase
    {
        private readonly AppDbContext _context;
        public DespesaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CriarDespesa([FromBody] Despesa novaDespesa)
        {
            _context.Despesas.Add(novaDespesa);
            _context.SaveChanges();
            return Ok("Despesa criada com sucesso!");
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var despesas = _context.Despesas.ToList();
            return Ok(despesas);
        }

        [HttpPut("{id}")]
        public IActionResult AtualizarDespesa(int id,[FromBody] Despesa despesaAtualizada)
        {
            var despesaExistente = _context.Despesas.Find(id);
            if (despesaExistente == null)
            {
                return NotFound();
            }
            despesaExistente.Descricao = despesaAtualizada.Descricao;
            despesaExistente.Valor = despesaAtualizada.Valor;
            despesaExistente.Data = despesaAtualizada.Data;
            despesaExistente.Categoria = despesaAtualizada.Categoria;

            _context.Despesas.Update(despesaExistente);
            return Ok(despesaAtualizada);

        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleteDespesa = _context.Despesas.Find(id);
            if (deleteDespesa == null)
            {
                return NotFound();
            }
            _context.Despesas.Remove(deleteDespesa);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
