using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs.Expenses;
using Application.Expenses.Queries;
using Application.Tests.Factories;
using Domain;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace Application.Tests.Expenses.Queries;

[TestSubject(typeof(HandlerGetAllCategories))]
public class HandlerGetAllCategoriesTest : CQRSHandlerTestBase<HandlerGetAllCategories>
{
    public override Task InitializeAsync()
    {
        RegisterMock<IRepositoryCategory>();
        return base.InitializeAsync();
    }

    protected override HandlerGetAllCategories CreateHandler() =>
        new(GetMock<IRepositoryCategory>().Object);

    
    [Fact]
    public async Task GetAll_ValidData_ReturnsAll()
    {
        // Arrange
        List<Category> categories = TestFactoryCategory.CreateCategories(TestFactoryUsers.DefaultUser1Authenticated, 5);
        GetMock<IRepositoryCategory>()
            .Setup(m => m.GetAllAsync())
            .ReturnsAsync(categories);
        
        QueryGetAllCategories query = new();

        // Act
        Result<IReadOnlyList<CategoryDTO>> result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        Assert.NotNull(result.Value);

        // ReSharper disable once InconsistentNaming
        IReadOnlyList<CategoryDTO> categoryDTOs = result.Value;

        for (var i = 0; i < categoryDTOs.Count; i++)
        {
            var dto = categoryDTOs[i];
            Assert.Equal(categories[i].Name, dto.Name);
            Assert.Equal(categories[i].Limit, dto.Limit);
        }
    }
    
    [Fact]
    public async Task GetAll_NoEntryInDB_ReturnsNull()
    {
        // Arrange
        GetMock<IRepositoryCategory>()
            .Setup(m => m.GetAllAsync())
            .ReturnsAsync([]);
        
        QueryGetAllCategories query = new();

        // Act
        Result<IReadOnlyList<CategoryDTO>> result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);
        Assert.Null(result.Value);
    }
}