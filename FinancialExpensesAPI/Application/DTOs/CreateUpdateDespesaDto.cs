namespace FinancialExpensesAPI.Application.DTOs
{
    public class CreateUpdateDespesaDto
    {
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public string Categoria { get; set; } = string.Empty;
    }
}
