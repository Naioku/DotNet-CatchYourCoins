using System.Threading;
using System.Threading.Tasks;
using Application.DTOs.Expenses;
using Application.Expenses.Queries;
using Domain;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace Application.Tests.Expenses.Queries;

[TestSubject(typeof(HandlerGetCategoryById))]
public class HandlerGetCategoryByIdTest : CQRSHandlerTestBase<HandlerGetCategoryById>
{
    public override Task InitializeAsync()
    {
        RegisterMock<IRepositoryCategory>();
        return base.InitializeAsync();
    }

    protected override HandlerGetCategoryById CreateHandler() =>
        new(GetMock<IRepositoryCategory>().Object);

    [Fact]
    public async Task GetOne_ValidData_ReturnOne()
    {
        // Arrange
        Category category = FactoryCategory.CreateEntity(TestFactoryUsers.DefaultUser1Authenticated);
        GetMock<IRepositoryCategory>()
            .Setup(m => m.GetByIdAsync(It.Is<int>(
                id => id == category.Id
            )))
            .ReturnsAsync(category);
        
        QueryGetCategoryById query = new() { Id = category.Id };

        // Act
        Result<CategoryDTO> result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        Assert.NotNull(result.Value);

        CategoryDTO categoryDTO = result.Value;
        Assert.Equal(query.Id, categoryDTO.Id);
        Assert.Equal(category.Name, categoryDTO.Name);
        Assert.Equal(category.Limit, categoryDTO.Limit);
    }

    [Fact]
    public async Task GetOne_NoEntryAtPassedID_ReturnNull()
    {
        // Arrange
        QueryGetCategoryById query = new() { Id = 1 };

        GetMock<IRepositoryCategory>()
            .Setup(m => m.GetByIdAsync(It.Is<int>(
                id => id == query.Id
            )))
            .ReturnsAsync((Category)null);

        // Act
        Result<CategoryDTO> result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);
        Assert.Null(result.Value);
    }
}