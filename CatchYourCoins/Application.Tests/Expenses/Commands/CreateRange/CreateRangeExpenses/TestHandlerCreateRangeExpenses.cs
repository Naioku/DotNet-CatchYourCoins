using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.DTOs.InputDTOs.Expenses;
using Application.Expenses.Commands.CreateRange;
using Application.Tests.Factories;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Commands.CreateRange.CreateRangeExpenses;

[TestSubject(typeof(HandlerCreateRangeExpenses))]
public class TestHandlerCreateRangeExpense
    : TestHandlerCreateRange<
        HandlerCreateRangeExpenses,
        Expense,
        InputDTOExpense,
        CommandCreateRangeExpenses,
        IRepositoryExpense,
        TestFactoryExpense
    >
{
    protected override HandlerCreateRangeExpenses CreateHandler()
    {
        return new HandlerCreateRangeExpenses(
            GetMock<IRepositoryExpense>().Object,
            GetMock<IServiceCurrentUser>().Object,
            GetMock<IUnitOfWork>().Object
        );
    }

    protected override CommandCreateRangeExpenses GetCommand() =>
        new()
        {
            Data = [
                new InputDTOExpense
                {
                    Amount = 100,
                    Date = DateTime.Now,
                    Description = "Test1",
                    CategoryId = 1,
                    PaymentMethodId = 1
                },
                new InputDTOExpense
                {
                    Amount = 200,
                    Date = DateTime.Now,
                    Description = "Test2",
                    CategoryId = 2,
                    PaymentMethodId = 2
                },
            ]
        };

    protected override Expression<Func<IList<Expense>, bool>> GetRepositoryMatch(CommandCreateRangeExpenses command) =>
        c =>
            c.Count == command.Data.Count &&
            Enumerable.Range(0, command.Data.Count).All(i => 
                c[i].Amount == command.Data[i].Amount &&
                c[i].Date == command.Data[i].Date &&
                c[i].Description == command.Data[i].Description &&
                c[i].CategoryId == command.Data[i].CategoryId &&
                c[i].PaymentMethodId == command.Data[i].PaymentMethodId &&
                c[i].UserId == TestFactoryUsers.DefaultUser1Authenticated.Id);

    [Fact]
    public async Task Create_ValidData_EntityCreated() =>
        await Create_ValidData_EntityCreated_Base();
}