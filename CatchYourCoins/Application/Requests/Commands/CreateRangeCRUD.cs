using Domain;
using Domain.Interfaces.Repositories;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;

namespace Application.Requests.Commands;

public abstract class CommandCRUDCreateRange<TDTO> : IRequest<Result>
{
    public required IList<TDTO> Data { get; init; }
}

[UsedImplicitly]
public abstract class ValidatorCRUDCreateRange<TCommand, TDTO>
    : AbstractValidator<TCommand> where TCommand : CommandCRUDCreateRange<TDTO>;

public abstract class HandlerCRUDCreateRange<TEntity, TCommand, TDTO>(
    IRepositoryCRUD<TEntity> repository,
    IUnitOfWork unitOfWork) : IRequestHandler<TCommand, Result>
    where TCommand : CommandCRUDCreateRange<TDTO>
{
    protected abstract TEntity MapDTOToEntity(TDTO dto);

    public async Task<Result> Handle(TCommand request, CancellationToken cancellationToken)
    {
        try
        {
            IList<TEntity> entities = request.Data.Select(MapDTOToEntity).ToList();
            await repository.CreateRangeAsync(entities);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(new Dictionary<string, string>()
            {
                {"Create", "Could not create entity"}
            });
        }
    }
}