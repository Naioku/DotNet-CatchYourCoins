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

[TestSubject(typeof(HandlerGetCategoryById))]
public class HandlerGetCategoryByIdTest : IAsyncLifetime
{
    private Mock<IRepositoryCategory> _mockRepository;
    
    public Task InitializeAsync()
    {
        _mockRepository = new Mock<IRepositoryCategory>();
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        _mockRepository = null;
        return Task.CompletedTask;
    }

    [Fact]
    public async Task GetCategory_ValidData_ReturnCategory()
    {
        // Arrange
        QueryGetCategoryById query = new() { Id = 1 };
        
        Category category = new Category
        {
            Id = query.Id,
            Limit = 100,
            Name = "Test",
            UserId = TestFactoryUsers.DefaultUser1Authenticated.Id,
        };
        _mockRepository.Setup(m => m.GetCategoryByIdAsync(It.IsAny<int>())).ReturnsAsync(category);
        
        HandlerGetCategoryById handler = new HandlerGetCategoryById(_mockRepository.Object);
        
        // Act
        Result<CategoryDTO> result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        
        CategoryDTO categoryDTO = result.Value;
        Assert.Equal(query.Id, categoryDTO.Id);
        Assert.Equal(category.Name, categoryDTO.Name);
        Assert.Equal(category.Limit, categoryDTO.Limit);
    }
    
    [Fact]
    public async Task GetCategory_NoCategory_ReturnNull()
    {
        // Arrange
        QueryGetCategoryById query = new() { Id = 1 };

        _mockRepository
            .Setup(m => m.GetCategoryByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Category)null);

        HandlerGetCategoryById handler = new(_mockRepository.Object);

        // Act
        Result<CategoryDTO> result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);

        CategoryDTO categoryDTO = result.Value;
        Assert.Null(categoryDTO);
    }
}