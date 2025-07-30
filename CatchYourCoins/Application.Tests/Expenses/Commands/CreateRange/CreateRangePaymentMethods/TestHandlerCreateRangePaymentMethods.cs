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

namespace Application.Tests.Expenses.Commands.CreateRange.CreateRangePaymentMethods;

[TestSubject(typeof(HandlerCreateRangePaymentMethods))]
public class TestHandlerCreateRangePaymentMethods
    : TestHandlerCreateRange<
        HandlerCreateRangePaymentMethods,
        ExpensePaymentMethod,
        InputDTOExpensePaymentMethod,
        CommandCreateRangePaymentMethods,
        IRepositoryExpensePaymentMethod,
        TestFactoryExpensePaymentMethod
    >
{
    protected override HandlerCreateRangePaymentMethods CreateHandler()
    {
        return new HandlerCreateRangePaymentMethods(
            GetMock<IRepositoryExpensePaymentMethod>().Object,
            GetMock<IServiceCurrentUser>().Object,
            GetMock<IUnitOfWork>().Object
        );
    }

    protected override CommandCreateRangePaymentMethods GetCommand() =>
        new()
        {
            Data = [
                new InputDTOExpensePaymentMethod
                {
                    Name = "Test1",
                    Limit = 1000
                },
                new InputDTOExpensePaymentMethod
                {
                    Name = "Test2",
                    Limit = 2000
                },
            ]
        };

    protected override Expression<Func<IList<ExpensePaymentMethod>, bool>> GetRepositoryMatch(CommandCreateRangePaymentMethods command) =>
        c =>
            c.Count == command.Data.Count &&
            Enumerable.Range(0, command.Data.Count).All(i => 
                c[i].Name == command.Data[i].Name &&
                c[i].Limit == command.Data[i].Limit &&
                c[i].UserId == TestFactoryUsers.DefaultUser1Authenticated.Id);

    [Fact]
    public async Task Create_ValidData_EntityCreated() =>
        await Create_ValidData_EntityCreated_Base();
}