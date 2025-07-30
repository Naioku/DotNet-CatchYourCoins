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

namespace Application.Tests.Incomes.Commands.CreateRange.CreateRangeCategories;

[TestSubject(typeof(HandlerCreateRangeCategories))]
public class TestHandlerCreateRangeCategories
    : TestHandlerCreateRange<
        HandlerCreateRangeCategories,
        IncomeCategory,
        InputDTOIncomeCategory,
        CommandCreateRangeCategories,
        IRepositoryIncomeCategory,
        TestFactoryCategoryIncomes
    >
{
    protected override HandlerCreateRangeCategories CreateHandler()
    {
        return new HandlerCreateRangeCategories(
            GetMock<IRepositoryIncomeCategory>().Object,
            GetMock<IServiceCurrentUser>().Object,
            GetMock<IUnitOfWork>().Object
        );
    }

    protected override CommandCreateRangeCategories GetCommand() =>
        new()
        {
            Data = [
                new InputDTOIncomeCategory
                {
                    Name = "Test1",
                    Limit = 1000
                },
                new InputDTOIncomeCategory
                {
                    Name = "Test2",
                    Limit = 2000
                },
            ]
        };

    protected override Expression<Func<IList<IncomeCategory>, bool>> GetRepositoryMatch(CommandCreateRangeCategories command) =>
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