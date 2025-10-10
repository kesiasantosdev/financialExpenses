using FinancialExpensesAPI.Domain.Entities;
using FinancialExpensesAPI.Domain.Interfaces;
using FinancialExpensesAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinancialExpensesAPI.Infrastructure.Repositories
{
    public class DespesaRepository : IDespesaRepository
    {
        private readonly AppDbContext _context;

        public DespesaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Despesa> GetByIdAsync(int id)
        {
            return await _context.Despesas.FindAsync(id);
        }

        public async Task<IEnumerable<Despesa>> GetAllAsync()
        {
            return await _context.Despesas.ToListAsync();
        }

        public async Task AddAsync(Despesa despesa)
        {
            await _context.Despesas.AddAsync(despesa);
        }


        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Update(Despesa despesa)
        {
            _context.Despesas.Update(despesa);
        }

        public void Delete(Despesa despesa)
        {
            _context.Despesas.Remove(despesa);
        }
    }
}
