using FinancialExpensesAPI.Domain.Entities;

namespace FinancialExpensesAPI.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario> GetByIdAsync(int id);
        Task<Usuario> GetByEmailAsync(string email);
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task AddAsync(Usuario usuario);
        Task<int> SaveChangesAsync();
        void Update(Usuario usuario);
        void Delete(Usuario usuario);

    }
}
