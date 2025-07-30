using Application.DTOs.InputDTOs.Expenses;
using Application.Requests.Commands;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using JetBrains.Annotations;

namespace Application.Expenses.Commands.CreateRange;

public class CommandCreateRangeExpenses : CommandCRUDCreateRange<InputDTOExpense>;

[UsedImplicitly]
public class ValidatorCreateRangeExpenses
    : ValidatorCRUDCreateRange<
        CommandCreateRangeExpenses,
        InputDTOExpense,
        ValidatorInputDTOExpense
    >;

public class HandlerCreateRangeExpenses(
    IRepositoryExpense repository,
    IServiceCurrentUser serviceCurrentUser,
    IUnitOfWork unitOfWork)
    : HandlerCRUDCreateRange<Expense, CommandCreateRangeExpenses, InputDTOExpense>(repository, unitOfWork)
{
    protected override Expense MapDTOToEntity(InputDTOExpense dto) =>
        new()
        {
            Amount = dto.Amount,
            Date = dto.Date,
            Description = dto.Description,
            UserId = serviceCurrentUser.User.Id,
            CategoryId = dto.CategoryId,
            PaymentMethodId = dto.PaymentMethodId,
        };
}