using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;

namespace Application.Expenses.Commands;

public class CommandAddPaymentMethod : IRequest
{
    public required string Name { get; init; }
    public decimal? Limit { get; init; }
}

[UsedImplicitly]
public class ValidatorAddPaymentMethod : AbstractValidator<CommandAddPaymentMethod>
{
    public ValidatorAddPaymentMethod()
    {
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}

public class HandlerAddPaymentMethod(
    IRepositoryPaymentMethod repositoryCategory,
    IServiceCurrentUser serviceCurrentUser,
    IUnitOfWork unitOfWork) : IRequestHandler<CommandAddPaymentMethod>
{
    public async Task Handle(CommandAddPaymentMethod request, CancellationToken cancellationToken)
    {
        await repositoryCategory.CreatePaymentMethodAsync(new PaymentMethod
        {
            Name = request.Name,
            Limit = request.Limit,
            UserId = serviceCurrentUser.User.Id
        });

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}