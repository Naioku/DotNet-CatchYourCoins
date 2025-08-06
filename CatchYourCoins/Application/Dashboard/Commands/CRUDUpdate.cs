using AutoMapper;
using Domain;
using Domain.Interfaces.Repositories;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;

namespace Application.Dashboard.Commands;

public class CommandCRUDUpdate<TDTO> : IRequest<Result>
{
    public required int Id { get; init; }
    public required TDTO Data { get; init; }
}

[UsedImplicitly]
public class ValidatorCRUDUpdate<TDTO, TDTOValidator> : AbstractValidator<CommandCRUDUpdate<TDTO>>
    where TDTOValidator : AbstractValidator<TDTO>, new()
{
    public ValidatorCRUDUpdate()
    {
        RuleFor(x => x.Id)
            .GreaterThanOrEqualTo(0);
        
        RuleFor(x => x.Data)
            .NotNull()
            .SetValidator(new TDTOValidator());
    }
}

public class HandlerCRUDUpdate<TEntity, TDTO>(
    IRepositoryCRUD<TEntity> repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<CommandCRUDUpdate<TDTO>, Result>
{
    public async Task<Result> Handle(CommandCRUDUpdate<TDTO> request, CancellationToken cancellationToken)
    {
        try
        {
            TEntity? entity = await repository.GetByIdAsync(request.Id);
            if (entity == null)
            {
                return Result.Failure(new Dictionary<string, string>
                {
                    {"Update", "Could not find entity"}
                });
            }
            
            repository.Update(mapper.Map<TEntity>(request.Data));
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(new Dictionary<string, string>
            {
                {"Update", "Could not update entity"}
            });
        }
    }
}