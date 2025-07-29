using Application.Requests.Commands.Create;
using Domain.Dashboard.Entities;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using FluentValidation;
using JetBrains.Annotations;

namespace Application.Incomes.Commands.Create;

public class CommandCreateIncome : CommandCreateBase
{
    public required decimal Amount { get; init; }
    public required DateTime Date { get; init; }
    public string? Description { get; init; }
    public int? CategoryId { get; init; }
}

[UsedImplicitly]
public class ValidatorCreateIncome : ValidatorCreateBase<CommandCreateIncome>
{
    public ValidatorCreateIncome()
    {
        RuleFor(x => x.Amount)
            .NotEmpty()
            .GreaterThanOrEqualTo(0);
        
        RuleFor(x => x.Date)
            .NotEmpty();
        
        RuleFor(x => x.Description)
            .MinimumLength(1)
            .MaximumLength(8000);
    }
}

public class HandlerCreateIncome(
    IRepositoryIncome repository,
    IServiceCurrentUser serviceCurrentUser,
    IUnitOfWork unitOfWork)
    : HandlerCRUDCreate<Income, CommandCreateIncome>(repository, unitOfWork)
{
    protected override Income MapCommandToEntity(CommandCreateIncome request) =>
        new()
        {
            Amount = request.Amount,
            Date = request.Date,
            Description = request.Description,
            UserId = serviceCurrentUser.User.Id,
            CategoryId = request.CategoryId,
        };
}