namespace FinancialExpensesAPI.Application.DTOs
{
    public class DespesaResponseDto
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public DateTime CriadoEm { get; set; }
    }
}
