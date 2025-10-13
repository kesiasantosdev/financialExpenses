using FinancialExpensesAPI.Application.DTOs;
using FluentValidation;

namespace FinancialExpensesAPI.Application.Validators
{
    public class CreateUpdateDespesaValidator : AbstractValidator<CreateUpdateDespesaDto>
    {
        public CreateUpdateDespesaValidator()
        {
            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("A descrição é obrigatória.")
                .MaximumLength(200).WithMessage("A descrição deve ter no máximo 200 caracteres.");

            RuleFor(x => x.Valor)
                .NotEmpty().WithMessage("O valor é obrigatório.")
                .GreaterThan(0).WithMessage("O valor deve ser maior que zero.");
        }
    }
}
