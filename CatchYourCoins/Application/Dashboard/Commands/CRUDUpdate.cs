using Application.Extensions;
using AutoMapper;
using Domain;
using Domain.Dashboard.Entities;
using Domain.Dashboard.Specifications;
using Domain.Interfaces.Repositories;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;

namespace Application.Dashboard.Commands;

public class CommandCRUDUpdate<TEntity, TDTO> : IRequest<Result>
    where TEntity : DashboardEntity
{
    public required ISpecificationDashboardEntity<TEntity> Specification { get; init; }
    public required IReadOnlyList<TDTO> Data { get; init; }
}

[UsedImplicitly]
public class ValidatorCRUDUpdate<TEntity, TDTO, TDTOValidator> : AbstractValidator<CommandCRUDUpdate<TEntity, TDTO>>
    where TEntity : DashboardEntity
    where TDTOValidator : AbstractValidator<TDTO>, new()
{
    public ValidatorCRUDUpdate()
    {
        RuleFor(x => x.Data)
            .NotEmpty();
        
        RuleForEach(x => x.Data)
            .SetValidator(new TDTOValidator());
    }
}

public class HandlerCRUDUpdate<TEntity, TDTO>(
    IRepositoryCRUD<TEntity> repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<CommandCRUDUpdate<TEntity, TDTO>, Result>
    where TEntity : DashboardEntity
    where TDTO : class
{
    public async Task<Result> Handle(CommandCRUDUpdate<TEntity, TDTO> request, CancellationToken cancellationToken)
    {
        try
        {
            ICollection<TEntity> entities = await repository.GetAsync(request.Specification);
            if (!entities.Any())
            {
                return Result.Failure(new Dictionary<string, string>
                {
                    {"Update", "Could not find entity"}
                });
            }
            
            mapper.MapEnumerable(request.Data, entities);
            repository.Update(entities);
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