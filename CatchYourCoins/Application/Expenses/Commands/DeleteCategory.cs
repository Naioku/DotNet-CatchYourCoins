using Domain;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;

namespace Application.Expenses.Commands;

public class CommandDeleteCategory : IRequest<Result>
{
    public required int Id { get; init; }
}

[UsedImplicitly]
public class ValidatorDeleteCategory : AbstractValidator<CommandDeleteCategory>
{
    public ValidatorDeleteCategory()
    {
        RuleFor(x => x.Id)
            .GreaterThanOrEqualTo(0);
    }
}

public class HandlerDeleteCategory(
    IRepositoryCategory repositoryCategory,
    IUnitOfWork unitOfWork) : IRequestHandler<CommandDeleteCategory, Result>
{
    public async Task<Result> Handle(CommandDeleteCategory request, CancellationToken cancellationToken)
    {
        Category? expense = await repositoryCategory.GetCategoryByIdAsync(request.Id);
        if (expense == null)
        {
            return Result.Failure(new Dictionary<string, string>()
            {
                {"Category", "Category not found"}
            });
        }
        
        repositoryCategory.DeleteCategory(expense);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}