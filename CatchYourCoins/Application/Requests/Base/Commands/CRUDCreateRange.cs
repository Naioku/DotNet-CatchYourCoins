using AutoMapper;
using Domain;
using Domain.Interfaces.Repositories;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;

namespace Application.Requests.Base.Commands;

public abstract class CommandCRUDCreateRange<TDTO> : IRequest<Result>
{
    public required IList<TDTO> Data { get; init; }
}

[UsedImplicitly]
public abstract class ValidatorCRUDCreateRange<TCommand, TDTO, TDTOValidator>
    : AbstractValidator<TCommand>
    where TCommand : CommandCRUDCreateRange<TDTO>
    where TDTOValidator : AbstractValidator<TDTO>, new()
{
    protected ValidatorCRUDCreateRange()
    {
        RuleFor(c => c.Data)
            .NotEmpty();
        
        RuleForEach(x => x.Data)
            .SetValidator(new TDTOValidator());
    }
}

public abstract class HandlerCRUDCreateRange<TEntity, TCommand, TDTO>(
    IRepositoryCRUD<TEntity> repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<TCommand, Result>
    where TCommand : CommandCRUDCreateRange<TDTO>
{
    public async Task<Result> Handle(TCommand request, CancellationToken cancellationToken)
    {
        try
        {
            IList<TEntity> entities = mapper.Map<IList<TEntity>>(request.Data);
            await repository.CreateRangeAsync(entities);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(new Dictionary<string, string>
            {
                {"Create", "Could not create entity"}
            });
        }
    }
}