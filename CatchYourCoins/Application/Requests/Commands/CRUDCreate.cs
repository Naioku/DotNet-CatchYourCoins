using AutoMapper;
using Domain;
using Domain.Interfaces.Repositories;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;

namespace Application.Requests.Commands;

public class CommandCRUDCreate<TDTO> : IRequest<Result>
{
    public required TDTO Data { get; init; }
}

[UsedImplicitly]
public class ValidatorCRUDCreate<TDTO, TDTOValidator> : AbstractValidator<CommandCRUDCreate<TDTO>>
    where TDTOValidator : AbstractValidator<TDTO>, new()
{
    protected ValidatorCRUDCreate()
    {
        RuleFor(x => x.Data)
            .NotNull()
            .SetValidator(new TDTOValidator());
    }
}

public class HandlerCRUDCreate<TEntity, TDTO>(
    IRepositoryCRUD<TEntity> repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<CommandCRUDCreate<TDTO>, Result>
{
    public async Task<Result> Handle(CommandCRUDCreate<TDTO> request, CancellationToken cancellationToken)
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
