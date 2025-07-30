using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.DTOs.InputDTOs.Incomes;
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
        InputDTOIncome,
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
            Data = new InputDTOIncome
            {
                Amount = 100,
                Date = DateTime.Now,
                Description = "Test",
                CategoryId = 1,
            }
        };

    protected override Expression<Func<Income, bool>> GetRepositoryMatch(CommandCreateIncome command) =>
        e =>
            e.Amount == command.Data.Amount &&
            e.Date == command.Data.Date &&
            e.UserId == TestFactoryUsers.DefaultUser1Authenticated.Id &&
            e.Description == command.Data.Description &&
            e.CategoryId == command.Data.CategoryId;

    [Fact]
    public async Task Create_ValidData_EntityCreated() =>
        await Create_ValidData_EntityCreated_Base();
}