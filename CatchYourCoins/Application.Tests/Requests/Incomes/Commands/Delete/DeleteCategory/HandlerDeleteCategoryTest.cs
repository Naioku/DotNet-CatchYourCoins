using System.Threading.Tasks;
using Application.Requests.Incomes.Commands.Delete;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Requests.Incomes.Commands.Delete.DeleteCategory;

[TestSubject(typeof(HandlerDeleteCategory))]
public class HandlerDeleteCategoryTest
    : TestHandlerDelete<
        HandlerDeleteCategory,
        IncomeCategory,
        CommandDeleteCategory,
        IRepositoryIncomeCategory,
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