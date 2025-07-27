using System.Threading.Tasks;
using Application.Expenses.Commands.Delete;
using Application.Tests.Factories;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Commands.Delete.DeleteCategory;

[TestSubject(typeof(HandlerDeleteCategory))]
public class TestHandlerDeleteCategory
    : TestHandlerDelete<HandlerDeleteCategory, CategoryExpenses, CommandDeleteCategory, IRepositoryCategoryExpenses, TestFactoryCategory, IUnitOfWork>
{
    public override Task InitializeAsync()
    {
        RegisterMock<IRepositoryCategoryExpenses>();
        RegisterMock<IUnitOfWork>();
        return base.InitializeAsync();
    }

    protected override HandlerDeleteCategory CreateHandler()
    {
        return new HandlerDeleteCategory(
            GetMock<IRepositoryCategoryExpenses>().Object,
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