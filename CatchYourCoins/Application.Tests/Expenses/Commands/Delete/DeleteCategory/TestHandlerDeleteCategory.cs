using System.Threading.Tasks;
using Application.Expenses.Commands.Delete;
using Application.Tests.Factories;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Commands.Delete.DeleteCategory;

[TestSubject(typeof(Application.Expenses.Commands.Delete.TestHandlerDeleteCategory))]
public class TestHandlerDeleteCategory
    : TestHandlerDelete<Application.Expenses.Commands.Delete.TestHandlerDeleteCategory, Category, CommandDeleteCategory, IRepositoryCategory, TestFactoryCategory, IUnitOfWork>
{
    public override Task InitializeAsync()
    {
        RegisterMock<IRepositoryCategory>();
        RegisterMock<IUnitOfWork>();
        return base.InitializeAsync();
    }

    protected override Application.Expenses.Commands.Delete.TestHandlerDeleteCategory CreateHandler()
    {
        return new Application.Expenses.Commands.Delete.TestHandlerDeleteCategory(
            GetMock<IRepositoryCategory>().Object,
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