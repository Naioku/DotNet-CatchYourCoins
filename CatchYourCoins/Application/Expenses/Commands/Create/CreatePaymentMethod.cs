using Application.DTOs.InputDTOs.Expenses;
using Application.Requests.Commands;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using JetBrains.Annotations;

namespace Application.Expenses.Commands.Create;

public class CommandCreatePaymentMethod : CommandCRUDCreate<InputDTOExpensePaymentMethod>;

[UsedImplicitly]
public class ValidatorCreatePaymentMethod
    : ValidatorCRUDCreate<
        CommandCreatePaymentMethod,
        InputDTOExpensePaymentMethod,
        ValidatorInputDTOExpensePaymentMethod
    >;

public class HandlerCreatePaymentMethod(
    IRepositoryExpensePaymentMethod repository,
    IServiceCurrentUser serviceCurrentUser,
    IUnitOfWork unitOfWork)
    : HandlerCRUDCreate<ExpensePaymentMethod, CommandCreatePaymentMethod, InputDTOExpensePaymentMethod>(repository, unitOfWork)
{
    protected override ExpensePaymentMethod MapDTOToEntity(InputDTOExpensePaymentMethod dto) =>
        new()
        {
            Name = dto.Name,
            Limit = dto.Limit,
            UserId = serviceCurrentUser.User.Id
        };
}