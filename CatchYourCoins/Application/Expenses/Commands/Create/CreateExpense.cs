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