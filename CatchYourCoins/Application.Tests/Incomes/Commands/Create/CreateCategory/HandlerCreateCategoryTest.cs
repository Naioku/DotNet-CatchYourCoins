using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Incomes.Commands.Create;
using Application.Tests.Factories;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Incomes.Commands.Create.CreateCategory;

[TestSubject(typeof(HandlerCreateCategory))]
public class HandlerCreateCategoryTest
    : TestHandlerCreate<
        HandlerCreateCategory,
        IncomeCategory,
        CommandCreateCategory,
        IRepositoryIncomeCategory,
        TestFactoryCategoryIncomes
    >
{
    protected override HandlerCreateCategory CreateHandler()
    {
        return new HandlerCreateCategory(
            GetMock<IRepositoryIncomeCategory>().Object,
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

    protected override Expression<Func<IncomeCategory, bool>> GetRepositoryMatch(CommandCreateCategory command) =>
        c =>
            c.Name == command.Name &&
            c.Limit == command.Limit &&
            c.UserId == TestFactoryUsers.DefaultUser1Authenticated.Id;

    [Fact]
    public async Task Create_ValidData_EntityCreated() =>
        await Create_ValidData_EntityCreated_Base();
}