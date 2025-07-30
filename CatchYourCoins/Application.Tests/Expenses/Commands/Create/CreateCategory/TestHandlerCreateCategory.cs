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

namespace Application.Tests.Expenses.Commands.Create.CreateCategory;

[TestSubject(typeof(HandlerCreateCategory))]
public class TestHandlerCreateCategory
    : TestHandlerCreate<
        HandlerCreateCategory,
        ExpenseCategory,
        InputDTOExpenseCategory,
        CommandCreateCategory,
        IRepositoryExpenseCategory,
        TestFactoryExpenseCategory
    >
{
    protected override HandlerCreateCategory CreateHandler()
    {
        return new HandlerCreateCategory(
            GetMock<IRepositoryExpenseCategory>().Object,
            GetMock<IServiceCurrentUser>().Object,
            GetMock<IUnitOfWork>().Object
        );
    }

    protected override CommandCreateCategory GetCommand() =>
        new()
        {
            Data = new InputDTOExpenseCategory
            {
                Name = "Test1",
                Limit = 1000
            },
        };

    protected override Expression<Func<ExpenseCategory, bool>> GetRepositoryMatch(CommandCreateCategory command) =>
        c =>
            c.Name == command.Data.Name &&
            c.Limit == command.Data.Limit &&
            c.UserId == TestFactoryUsers.DefaultUser1Authenticated.Id;

    [Fact]
    public async Task Create_ValidData_EntityCreated() =>
        await Create_ValidData_EntityCreated_Base();
}