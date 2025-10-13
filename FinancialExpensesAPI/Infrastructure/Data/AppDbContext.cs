using FinancialExpensesAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancialExpensesAPI.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        
        public DbSet<Despesa> Despesas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Despesa>(entity =>
            {
                
                entity.Property(e => e.Valor)
                      .HasPrecision(18, 2);
            });
        }
    }
}
