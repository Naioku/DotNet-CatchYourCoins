using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Expenses.Commands.Create;
using Application.Tests.Factories;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Commands.Create.CreateExpense;

[TestSubject(typeof(HandlerCreateExpense))]
public class TestHandlerCreateExpense
    : TestHandlerCreate<
        HandlerCreateExpense,
        Expense,
        CommandCreateExpense,
        IRepositoryExpense,
        TestFactoryExpense
    >
{
    protected override HandlerCreateExpense CreateHandler()
    {
        return new HandlerCreateExpense(
            GetMock<IRepositoryExpense>().Object,
            GetMock<IServiceCurrentUser>().Object,
            GetMock<IUnitOfWork>().Object
        );
    }
    
    protected override CommandCreateExpense GetCommand() =>
        new()
        {
            Amount = 100,
            Date = DateTime.Now,
            Description = "Test",
            CategoryId = 1,
            PaymentMethodId = 1
        };

    protected override Expression<Func<Expense, bool>> GetRepositoryMatch(CommandCreateExpense command) =>
        e =>
            e.Amount == command.Amount &&
            e.Date == command.Date &&
            e.UserId == TestFactoryUsers.DefaultUser1Authenticated.Id &&
            e.Description == command.Description &&
            e.CategoryId == command.CategoryId &&
            e.PaymentMethodId == command.PaymentMethodId;

    [Fact]
    public async Task Create_ValidData_EntityCreated() =>
        await Create_ValidData_EntityCreated_Base();
}