using FinancialExpensesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialExpensesAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        
         public DbSet<Despesa> Despesas { get; set; }
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
