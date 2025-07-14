using Domain.Dashboard.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;

namespace Application.Expenses.Commands;

public class CommandAddCategory : IRequest
{
    public required string Name { get; init; }
    public decimal? Limit { get; init; }
}

public class HandlerAddCategory(
    IRepositoryCategory repositoryCategory,
    IServiceCurrentUser serviceCurrentUser,
    IUnitOfWork unitOfWork) : IRequestHandler<CommandAddCategory>
{
    public async Task Handle(CommandAddCategory request, CancellationToken cancellationToken)
    {
        await repositoryCategory.CreateCategoryAsync(new Category
        {
            Name = request.Name,
            Limit = request.Limit,
            UserId = serviceCurrentUser.User.Id
        });

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

[UsedImplicitly]
public class ValidatorAddCategory : AbstractValidator<CommandAddCategory>
{
    public ValidatorAddCategory()
    {
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}