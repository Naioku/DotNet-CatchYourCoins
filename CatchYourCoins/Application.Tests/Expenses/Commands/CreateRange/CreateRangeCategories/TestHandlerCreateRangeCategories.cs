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

namespace Application.Tests.Expenses.Commands.CreateRange.CreateRangeCategories;

[TestSubject(typeof(HandlerCreateRangeCategories))]
public class TestHandlerCreateRangeCategories
    : TestHandlerCreateRange<
        HandlerCreateRangeCategories,
        ExpenseCategory,
        InputDTOExpenseCategory,
        CommandCreateRangeCategories,
        IRepositoryExpenseCategory,
        TestFactoryExpenseCategory
    >
{
    protected override HandlerCreateRangeCategories CreateHandler()
    {
        return new HandlerCreateRangeCategories(
            GetMock<IRepositoryExpenseCategory>().Object,
            GetMock<IServiceCurrentUser>().Object,
            GetMock<IUnitOfWork>().Object
        );
    }

    protected override CommandCreateRangeCategories GetCommand() =>
        new()
        {
            Data = [
                new InputDTOExpenseCategory
                {
                    Name = "Test1",
                    Limit = 1000
                },
                new InputDTOExpenseCategory
                {
                    Name = "Test2",
                    Limit = 2000
                },
            ]
        };

    protected override Expression<Func<IList<ExpenseCategory>, bool>> GetRepositoryMatch(CommandCreateRangeCategories command) =>
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