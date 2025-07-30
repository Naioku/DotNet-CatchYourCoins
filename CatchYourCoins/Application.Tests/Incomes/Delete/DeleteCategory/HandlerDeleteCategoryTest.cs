using System.Threading.Tasks;
using Application.Incomes.Commands.Delete;
using Application.Tests.Factories;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Incomes.Delete.DeleteCategory;

[TestSubject(typeof(HandlerDeleteCategory))]
public class HandlerDeleteCategoryTest
    : TestHandlerDelete<
        HandlerDeleteCategory,
        IncomeCategory,
        CommandDeleteCategory,
        IRepositoryIncomeCategory,
        TestFactoryCategoryIncomes,
        IUnitOfWork
    >
{
    protected override HandlerDeleteCategory CreateHandler()
    {
        return new HandlerDeleteCategory(
            GetMock<IRepositoryIncomeCategory>().Object,
            GetMock<IUnitOfWork>().Object
        );
    }

    protected override CommandDeleteCategory GetCommand() => new() { Id = 1 };

    [Fact]
    public async Task DeleteOne_ValidData_DeletedEntity() =>
        await DeleteOne_ValidData_DeletedEntity_Base();

    [Fact]
    public async Task DeleteOne_NoEntryAtPassedID_DeletedNothing() =>
        await DeleteOne_NoEntryAtPassedID_DeletedNothing_Base();
}