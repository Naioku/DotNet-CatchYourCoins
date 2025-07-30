using Application.DTOs.InputDTOs.Expenses;
using Application.Requests.Commands;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using FluentValidation;
using JetBrains.Annotations;

namespace Application.Expenses.Commands.CreateRange;

public class CommandCreateRangePaymentMethods : CommandCRUDCreateRange<InputDTOExpensePaymentMethod>;

[UsedImplicitly]
public class ValidatorCreateRangePaymentMethods : ValidatorCRUDCreateRange<CommandCreateRangePaymentMethods, InputDTOExpensePaymentMethod>
{
    public ValidatorCreateRangePaymentMethods()
    {
        RuleFor(c => c.Data)
            .NotEmpty();
        
        RuleForEach(x => x.Data)
            .SetValidator(new ValidatorInputDTOExpensePaymentMethod());
    }
}

public class HandlerCreateRangePaymentMethods(
    IRepositoryExpensePaymentMethod repository,
    IServiceCurrentUser serviceCurrentUser,
    IUnitOfWork unitOfWork)
    : HandlerCRUDCreateRange<ExpensePaymentMethod, CommandCreateRangePaymentMethods, InputDTOExpensePaymentMethod>(repository, unitOfWork)
{
    protected override ExpensePaymentMethod MapDTOToEntity(InputDTOExpensePaymentMethod dto) =>
        new()
        {
            Name = dto.Name,
            Limit = dto.Limit,
            UserId = serviceCurrentUser.User.Id
        };
}