using Application.Requests.Commands;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using FluentValidation;
using JetBrains.Annotations;

namespace Application.Expenses.Commands.Create;

public class CommandCreateCategory : CommandCreateBase
{
    public required string Name { get; init; }
    public decimal? Limit { get; init; }
}

[UsedImplicitly]
public class ValidatorCreateCategory : ValidatorCreateBase<CommandCreateCategory>
{
    public ValidatorCreateCategory()
    {
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}

public class HandlerCreateCategory(
    IRepositoryExpenseCategory repository,
    IServiceCurrentUser serviceCurrentUser,
    IUnitOfWork unitOfWork) : HandlerCRUDCreate<ExpenseCategory, CommandCreateCategory>(repository, unitOfWork)
{
    protected override ExpenseCategory MapCommandToEntity(CommandCreateCategory request) =>
        new()
        {
            Name = request.Name,
            Limit = request.Limit,
            UserId = serviceCurrentUser.User.Id
        };
}