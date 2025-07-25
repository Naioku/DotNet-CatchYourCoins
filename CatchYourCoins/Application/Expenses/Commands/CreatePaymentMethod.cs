using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;

namespace Application.Expenses.Commands;

public class CommandCreatePaymentMethod : IRequest
{
    public required string Name { get; init; }
    public decimal? Limit { get; init; }
}

[UsedImplicitly]
public class ValidatorCreatePaymentMethod : AbstractValidator<CommandCreatePaymentMethod>
{
    public ValidatorCreatePaymentMethod()
    {
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}

public class HandlerCreatePaymentMethod(
    IRepositoryPaymentMethod repositoryCategory,
    IServiceCurrentUser serviceCurrentUser,
    IUnitOfWork unitOfWork) : HandlerCRUDCreate<PaymentMethod, CommandCreatePaymentMethod>(repositoryCategory, unitOfWork)
{
    protected override PaymentMethod MapCommandToEntity(CommandCreatePaymentMethod request) =>
        new()
        {
            Name = request.Name,
            Limit = request.Limit,
            UserId = serviceCurrentUser.User.Id
        };
}