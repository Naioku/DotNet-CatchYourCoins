using AutoMapper;
using Domain;
using Domain.Interfaces.Repositories;
using FluentValidation;
using MediatR;

namespace Application.Requests.Base.Commands;

public abstract class CommandCRUDCreate<TDTO> : IRequest<Result>
{
    public required TDTO Data { get; init; }
}

public abstract class ValidatorCRUDCreate<TCommand, TDTO, TDTOValidator> : AbstractValidator<TCommand>
    where TCommand : CommandCRUDCreate<TDTO>
    where TDTOValidator : AbstractValidator<TDTO>, new()
{
    protected ValidatorCRUDCreate()
    {
        RuleFor(x => x.Data)
            .NotNull()
            .SetValidator(new TDTOValidator());
    }
}

public abstract class HandlerCRUDCreate<TEntity, TCommand, TDTO>(
    IRepositoryCRUD<TEntity> repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<TCommand, Result>
    where TCommand : CommandCRUDCreate<TDTO>
{
    public async Task<Result> Handle(TCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await repository.CreateAsync(mapper.Map<TEntity>(request.Data));
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
