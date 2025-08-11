using Domain;
using Domain.Dashboard.Entities;
using Domain.Dashboard.Specifications;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Dashboard.Commands;

public class CommandCRUDDelete<TEntity> : IRequest<Result>
    where TEntity : DashboardEntity
{
    public required ISpecificationDashboardEntity<TEntity> Specification { get; init; }
}

public class HandlerCRUDDelete<TEntity>(
    IRepositoryCRUD<TEntity> repository,
    IUnitOfWork unitOfWork) : IRequestHandler<CommandCRUDDelete<TEntity>, Result>
    where TEntity : DashboardEntity
{
    public async Task<Result> Handle(CommandCRUDDelete<TEntity> request, CancellationToken cancellationToken)
    {
        IReadOnlyList<TEntity> expense = await repository.GetAsync(request.Specification);
        if (!expense.Any())
        {
            return Result.Failure(GetFailureMessages());
        }
        
        repository.Delete(expense);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    private Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Delete", "Entity not found" }
        };
}