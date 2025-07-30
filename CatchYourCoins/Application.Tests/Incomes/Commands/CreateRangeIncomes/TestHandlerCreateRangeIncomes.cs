using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.DTOs.InputDTOs.Incomes;
using Application.Incomes.Commands.CreateRange;
using Application.Tests.Factories;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Incomes.Commands.CreateRangeIncomes;

[TestSubject(typeof(HandlerCreateRangeIncomes))]
public class TestHandlerCreateRangeIncomes
    : TestHandlerCreateRange<
        HandlerCreateRangeIncomes,
        Income,
        InputDTOIncome,
        CommandCreateRangeIncomes,
        IRepositoryIncome,
        TestFactoryIncome
    >
{
    protected override HandlerCreateRangeIncomes CreateHandler()
    {
        return new HandlerCreateRangeIncomes(
            GetMock<IRepositoryIncome>().Object,
            GetMock<IServiceCurrentUser>().Object,
            GetMock<IUnitOfWork>().Object
        );
    }

    protected override CommandCreateRangeIncomes GetCommand() =>
        new()
        {
            Data = [
                new InputDTOIncome
                {
                    Amount = 100,
                    Date = DateTime.Now,
                    Description = "Test1",
                    CategoryId = 1,
                },
                new InputDTOIncome
                {
                    Amount = 200,
                    Date = DateTime.Now,
                    Description = "Test2",
                    CategoryId = 2,
                },
            ]
        };

    protected override Expression<Func<IList<Income>, bool>> GetRepositoryMatch(CommandCreateRangeIncomes command) =>
        c =>
            c.Count == command.Data.Count &&
            Enumerable.Range(0, command.Data.Count).All(i => 
                c[i].Amount == command.Data[i].Amount &&
                c[i].Date == command.Data[i].Date &&
                c[i].Description == command.Data[i].Description &&
                c[i].CategoryId == command.Data[i].CategoryId &&
                c[i].UserId == TestFactoryUsers.DefaultUser1Authenticated.Id);

    [Fact]
    public async Task Create_ValidData_EntityCreated() =>
        await Create_ValidData_EntityCreated_Base();
}