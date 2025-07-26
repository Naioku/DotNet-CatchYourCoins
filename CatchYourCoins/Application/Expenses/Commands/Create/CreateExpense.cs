using Application.Requests.Commands.Create;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;

namespace Application.Expenses.Commands.Create;

public class CommandCreateExpense : IRequest
{
    public required decimal Amount { get; init; }
    public required DateTime Date { get; init; }
    public string? Description { get; init; }
    public int? CategoryId { get; init; }
    public int? PaymentMethodId { get; init; }
}

[UsedImplicitly]
public class TestValidatorCreateExpense : AbstractValidator<CommandCreateExpense>
{
    public TestValidatorCreateExpense()
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

public class TestHandlerCreateExpense(
    IRepositoryExpense repositoryExpense,
    IServiceCurrentUser serviceCurrentUser,
    IUnitOfWork unitOfWork) : HandlerCRUDCreate<Expense, CommandCreateExpense>(repositoryExpense, unitOfWork)
{
    protected override Expense MapCommandToEntity(CommandCreateExpense request) =>
        new()
        {
            Amount = request.Amount,
            Date = request.Date,
            Description = request.Description,
            UserId = serviceCurrentUser.User.Id,
            CategoryId = request.CategoryId,
            PaymentMethodId = request.PaymentMethodId,
        };
}