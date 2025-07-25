using System.Threading;
using System.Threading.Tasks;
using Application.Expenses.Commands.Delete;
using Domain;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace Application.Tests.Expenses.Commands.Delete.DeleteCategory;

[TestSubject(typeof(HandlerDeleteCategory))]
public class HandlerDeleteCategoryTest : CQRSHandlerTestBase<HandlerDeleteCategory>
{
    public override Task InitializeAsync()
    {
        RegisterMock<IRepositoryCategory>();
        RegisterMock<IUnitOfWork>();
        return base.InitializeAsync();
    }

    protected override HandlerDeleteCategory CreateHandler()
    {
        return new HandlerDeleteCategory(
            GetMock<IRepositoryCategory>().Object,
            GetMock<IUnitOfWork>().Object
        );
    }

    [Fact]
    public async Task DeleteCategory_ValidData_DeleteCategory()
    {
        // Arrange
        Category entity = FactoryCategory.CreateEntity(TestFactoryUsers.DefaultUser1Authenticated);
        GetMock<IRepositoryCategory>()
            .Setup(m => m.GetByIdAsync(It.Is<int>(
                id => id == entity.Id
            )))
            .ReturnsAsync(entity);

        CommandDeleteCategory command = new() { Id = entity.Id };

        // Act
        Result result = await Handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        GetMock<IRepositoryCategory>().Verify(m => m.Delete(It.Is<Category>(e => e.Id == command.Id)));
        GetMock<IUnitOfWork>().Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteCategory_NoCategoryUnderPassedID_NotDeleteCategory()
    {
        // Arrange
        GetMock<IRepositoryCategory>()
            .Setup(m => m.GetByIdAsync(It.Is<int>(
                id => id == 1
            )))
            .ReturnsAsync((Category)null);

        CommandDeleteCategory command = new() { Id = 1 };

        // Act
        Result result = await Handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);
        GetMock<IRepositoryCategory>().Verify(m => m.Delete(It.IsAny<Category>()), Times.Never);
    }
}