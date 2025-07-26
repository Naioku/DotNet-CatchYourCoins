using Application.Requests.Commands.Create;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;

namespace Application.Expenses.Commands.Create;

public class CommandCreatePaymentMethod : IRequest
{
    public required string Name { get; init; }
    public decimal? Limit { get; init; }
}

[UsedImplicitly]
public class TestValidatorCreatePaymentMethod : AbstractValidator<CommandCreatePaymentMethod>
{
    public TestValidatorCreatePaymentMethod()
    {
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}

public class TestHandlerCreatePaymentMethod(
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