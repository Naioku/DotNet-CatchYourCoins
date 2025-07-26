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

namespace Application.Tests.Expenses.Commands.Create.CreateCategory;

[TestSubject(typeof(Application.Expenses.Commands.Create.TestHandlerCreateCategory))]
public class TestHandlerCreateCategory
    : TestHandlerCreate<
        Application.Expenses.Commands.Create.TestHandlerCreateCategory,
        Category,
        CommandCreateCategory,
        IRepositoryCategory,
        TestFactoryCategory
    >
{
    protected override Application.Expenses.Commands.Create.TestHandlerCreateCategory CreateHandler()
    {
        return new Application.Expenses.Commands.Create.TestHandlerCreateCategory(
            GetMock<IRepositoryCategory>().Object,
            GetMock<IServiceCurrentUser>().Object,
            GetMock<IUnitOfWork>().Object
        );
    }

    protected override CommandCreateCategory GetCommand() =>
        new()
        {
            Name = "Test",
            Limit = 1000
        };

    protected override Expression<Func<Category, bool>> GetRepositoryMatch(CommandCreateCategory command) =>
        c =>
            c.Name == command.Name &&
            c.Limit == command.Limit &&
            c.UserId == TestFactoryUsers.DefaultUser1Authenticated.Id;

    [Fact]
    public async Task Create_ValidData_EntityCreated() =>
        await Create_ValidData_EntityCreated_Base();
}