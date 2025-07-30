using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.DTOs.InputDTOs.Expenses;
using Application.Expenses.Commands.Create;
using Application.Tests.Factories;
using Domain.Dashboard.Entities.Expenses;
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
        InputDTOExpense,
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
            Data = new InputDTOExpense
            {
                Amount = 100,
                Date = DateTime.Now,
                Description = "Test",
                CategoryId = 1,
                PaymentMethodId = 1
            }
        };

    protected override Expression<Func<Expense, bool>> GetRepositoryMatch(CommandCreateExpense command) =>
        e =>
            e.Amount == command.Data.Amount &&
            e.Date == command.Data.Date &&
            e.Description == command.Data.Description &&
            e.CategoryId == command.Data.CategoryId &&
            e.PaymentMethodId == command.Data.PaymentMethodId &&
            e.UserId == TestFactoryUsers.DefaultUser1Authenticated.Id;

    [Fact]
    public async Task Create_ValidData_EntityCreated() =>
        await Create_ValidData_EntityCreated_Base();
}