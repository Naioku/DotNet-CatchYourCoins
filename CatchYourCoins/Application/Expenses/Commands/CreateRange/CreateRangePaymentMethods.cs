using Application.DTOs.InputDTOs.Expenses;
using Application.Requests.Commands;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using JetBrains.Annotations;

namespace Application.Expenses.Commands.CreateRange;

public class CommandCreateRangePaymentMethods : CommandCRUDCreateRange<InputDTOExpensePaymentMethod>;

[UsedImplicitly]
public class ValidatorCreateRangePaymentMethods
    : ValidatorCRUDCreateRange<
        CommandCreateRangePaymentMethods,
        InputDTOExpensePaymentMethod,
        ValidatorInputDTOExpensePaymentMethod
    >;

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