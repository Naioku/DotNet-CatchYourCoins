using Domain;
using Domain.Interfaces.Repositories;
using FluentValidation;
using MediatR;

namespace Application.Requests.Commands;

public class CommandCreateBase : IRequest<Result>;

public abstract class ValidatorCreateBase<T> : AbstractValidator<T> where T : CommandCreateBase;

public abstract class HandlerCRUDCreate<TEntity, TCommand>(
    IRepositoryCRUD<TEntity> repository,
    IUnitOfWork unitOfWork) : IRequestHandler<TCommand, Result>
    where TCommand : CommandCreateBase
{
    protected abstract TEntity MapCommandToEntity(TCommand request);

    public async Task<Result> Handle(TCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await repository.CreateAsync(MapCommandToEntity(request));
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

