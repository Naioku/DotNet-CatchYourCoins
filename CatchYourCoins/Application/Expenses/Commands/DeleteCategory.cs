using Domain;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using MediatR;

namespace Application.Expenses.Commands;

public class CommandDeleteCategory : CommandDeleteBase;

[UsedImplicitly]
public class ValidatorDeleteCategory : ValidatorDeleteBase<CommandDeleteCategory>;

public class HandlerDeleteCategory(
    IRepositoryCategory repositoryCategory,
    IUnitOfWork unitOfWork) : IRequestHandler<CommandDeleteCategory, Result>
{
    public async Task<Result> Handle(CommandDeleteCategory request, CancellationToken cancellationToken)
    {
        Category? expense = await repositoryCategory.GetByIdAsync(request.Id);
        if (expense == null)
        {
            return Result.Failure(new Dictionary<string, string>()
            {
                {"Category", "Category not found"}
            });
        }
        
        repositoryCategory.Delete(expense);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}