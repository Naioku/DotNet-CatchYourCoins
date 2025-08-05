using AutoMapper;
using Domain;
using Domain.Interfaces.Repositories;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;

namespace Application.Requests.Commands;

public class CommandCRUDCreateRange<TDTO> : IRequest<Result>
{
    public required IList<TDTO> Data { get; init; }
}

[UsedImplicitly]
public class ValidatorCRUDCreateRange<TDTO, TDTOValidator>
    : AbstractValidator<CommandCRUDCreateRange<TDTO>>
    where TDTOValidator : AbstractValidator<TDTO>, new()
{
    public ValidatorCRUDCreateRange()
    {
        RuleFor(x => x.Data)
            .NotEmpty();
        
        RuleForEach(x => x.Data)
            .SetValidator(new TDTOValidator());
    }
}

public class HandlerCRUDCreateRange<TEntity, TDTO>(
    IRepositoryCRUD<TEntity> repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<CommandCRUDCreateRange<TDTO>, Result>
{
    public async Task<Result> Handle(CommandCRUDCreateRange<TDTO> request, CancellationToken cancellationToken)
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