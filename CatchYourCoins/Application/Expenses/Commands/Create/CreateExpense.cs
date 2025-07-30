using Application.DTOs.InputDTOs.Expenses;
using Application.Requests.Commands;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using JetBrains.Annotations;

namespace Application.Expenses.Commands.Create;

public class CommandCreateExpense : CommandCRUDCreate<InputDTOExpense>;

[UsedImplicitly]
public class ValidatorCreateExpense
    : ValidatorCRUDCreate<
        CommandCreateExpense,
        InputDTOExpense,
        ValidatorInputDTOExpense
    >;

public class HandlerCreateExpense(
    IRepositoryExpense repository,
    IServiceCurrentUser serviceCurrentUser,
    IUnitOfWork unitOfWork) : HandlerCRUDCreate<Expense, CommandCreateExpense, InputDTOExpense>(repository, unitOfWork)
{
    protected override Expense MapDTOToEntity(InputDTOExpense request) =>
        new()
        {
            Amount = request.Amount,
            Date = request.Date,
            Description = request.Description,
            UserId = serviceCurrentUser.User.Id,
            CategoryId = request.CategoryId,
            PaymentMethodId = request.PaymentMethodId,
        };
}