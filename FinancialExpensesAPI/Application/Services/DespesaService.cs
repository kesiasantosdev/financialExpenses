using FinancialExpensesAPI.Application.DTOs;
using FinancialExpensesAPI.Infrastructure.Data;
using FinancialExpensesAPI.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using FinancialExpensesAPI.Domain.Interfaces;

namespace FinancialExpensesAPI.Application.Services
{
    public class DespesaService
    {
        private readonly IDespesaRepository _despesaRepository;

        public DespesaService(IDespesaRepository despesaRepository)
        {
            _despesaRepository = despesaRepository;
        }

        public async Task<IEnumerable<DespesaResponseDto>> GetDtosAsync()
        {
            var despesas = await _despesaRepository.GetAllAsync();

            var despesasDto = despesas.Select(d => new DespesaResponseDto
            {
                Id = d.Id,
                Descricao = d.Descricao,
                Valor = d.Valor,
                Data = d.Data,
                Categoria = d.Categoria,
                CriadoEm = d.CriadoEm
            });
            return despesasDto;
        }

        public async Task<DespesaResponseDto> AddAsync(CreateUpdateDespesaDto novaDespesa)
        {
            var despesa = new Despesa
            {
                Descricao = novaDespesa.Descricao,
                Valor = novaDespesa.Valor,
                Data = novaDespesa.Data,
                Categoria = novaDespesa.Categoria,
                CriadoEm = DateTime.Now
            };
            await _despesaRepository.AddAsync(despesa);
            await _despesaRepository.SaveChangesAsync();

            var despesaResponse = new DespesaResponseDto
            {
                Id = despesa.Id,
                Descricao = despesa.Descricao,
                Valor = despesa.Valor,
                Data = despesa.Data,
                Categoria = despesa.Categoria,
                CriadoEm = despesa.CriadoEm
            };
            return despesaResponse;
        }

        public async Task<DespesaResponseDto> AtualizarDespesa(int id, CreateUpdateDespesaDto despesaAtualizada)
        {
            var despesaExistente = await _despesaRepository.GetByIdAsync(id);
            if (despesaExistente == null)
            {
                return null;
            }
            despesaExistente.Descricao = despesaAtualizada.Descricao;
            despesaExistente.Valor = despesaAtualizada.Valor;
            despesaExistente.Data = despesaAtualizada.Data;
            despesaExistente.Categoria = despesaAtualizada.Categoria;
            _despesaRepository.Update(despesaExistente);
            await _despesaRepository.SaveChangesAsync();

            var despesaResponse = new DespesaResponseDto
            {
                Id = despesaExistente.Id,
                Descricao = despesaExistente.Descricao,
                Valor = despesaExistente.Valor,
                Data = despesaExistente.Data,
                Categoria = despesaExistente.Categoria,
                CriadoEm = despesaExistente.CriadoEm
            };

            return despesaResponse;
        }

        public async Task<bool> DeletarDespesa(int id)
        {
            var despesaExistente = await _despesaRepository.GetByIdAsync(id);
            if (despesaExistente == null)
            {
                return false;
            }
            _despesaRepository.Delete(despesaExistente);
            await _despesaRepository.SaveChangesAsync();
            return true;
        }
    }
}
