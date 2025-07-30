using Application.DTOs.InputDTOs.Expenses;
using Application.Requests.Commands;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using JetBrains.Annotations;

namespace Application.Expenses.Commands.Create;

public class CommandCreateCategory : CommandCRUDCreate<InputDTOExpenseCategory>;

[UsedImplicitly]
public class ValidatorCreateCategory
    : ValidatorCRUDCreate<
        CommandCreateCategory,
        InputDTOExpenseCategory,
        ValidatorInputDTOExpenseCategory
    >;

public class HandlerCreateCategory(
    IRepositoryExpenseCategory repository,
    IServiceCurrentUser serviceCurrentUser,
    IUnitOfWork unitOfWork)
    : HandlerCRUDCreate<ExpenseCategory, CommandCreateCategory, InputDTOExpenseCategory>(repository, unitOfWork)
{
    protected override ExpenseCategory MapDTOToEntity(InputDTOExpenseCategory request) =>
        new()
        {
            Name = request.Name,
            Limit = request.Limit,
            UserId = serviceCurrentUser.User.Id
        };
}