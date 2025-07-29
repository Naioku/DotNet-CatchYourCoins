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

namespace Application.Tests.Incomes.Commands.Create.CreateIncome;

[TestSubject(typeof(HandlerCreateIncome))]
public class HandlerCreateIncomeTest
    : TestHandlerCreate<
        HandlerCreateIncome,
        Income,
        CommandCreateIncome,
        IRepositoryIncome,
        TestFactoryIncome
    >
{
    protected override HandlerCreateIncome CreateHandler()
    {
        return new HandlerCreateIncome(
            GetMock<IRepositoryIncome>().Object,
            GetMock<IServiceCurrentUser>().Object,
            GetMock<IUnitOfWork>().Object
        );
    }
    
    protected override CommandCreateIncome GetCommand() =>
        new()
        {
            Amount = 100,
            Date = DateTime.Now,
            Description = "Test",
            CategoryId = 1,
        };

    protected override Expression<Func<Income, bool>> GetRepositoryMatch(CommandCreateIncome command) =>
        e =>
            e.Amount == command.Amount &&
            e.Date == command.Date &&
            e.UserId == TestFactoryUsers.DefaultUser1Authenticated.Id &&
            e.Description == command.Description &&
            e.CategoryId == command.CategoryId;

    [Fact]
    public async Task Create_ValidData_EntityCreated() =>
        await Create_ValidData_EntityCreated_Base();
}