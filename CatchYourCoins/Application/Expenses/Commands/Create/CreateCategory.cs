using Application.Requests.Commands.Create;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;

namespace Application.Expenses.Commands.Create;

public class CommandCreateCategory : IRequest
{
    public required string Name { get; init; }
    public decimal? Limit { get; init; }
}

[UsedImplicitly]
public class ValidatorCreateCategory : AbstractValidator<CommandCreateCategory>
{
    public ValidatorCreateCategory()
    {
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}

public class HandlerCreateCategory(
    IRepositoryCategoryExpenses repositoryCategory,
    IServiceCurrentUser serviceCurrentUser,
    IUnitOfWork unitOfWork) : HandlerCRUDCreate<CategoryExpenses, CommandCreateCategory>(repositoryCategory, unitOfWork)
{
    protected override CategoryExpenses MapCommandToEntity(CommandCreateCategory request) =>
        new()
        {
            Name = request.Name,
            Limit = request.Limit,
            UserId = serviceCurrentUser.User.Id
        };
}