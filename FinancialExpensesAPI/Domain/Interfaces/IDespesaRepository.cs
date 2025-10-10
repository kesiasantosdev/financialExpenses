using FinancialExpensesAPI.Domain.Entities;

namespace FinancialExpensesAPI.Domain.Interfaces
{
    public interface IDespesaRepository
    {
        Task<Despesa> GetByIdAsync(int id);
        Task<IEnumerable<Despesa>> GetAllAsync();
        Task AddAsync(Despesa despesa);
        Task<int> SaveChangesAsync();
        void Update(Despesa despesa);
        void Delete(Despesa despesa);
    }
}
