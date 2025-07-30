using Application.DTOs.InputDTOs.Incomes;
using Application.DTOs.OutputDTOs.Incomes;
using Application.Incomes.Commands.Create;
using Application.Incomes.Queries.GetById;
using Domain;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Incomes;

public class Incomes(TestFixture fixture) : TestBase(fixture)
{
    private readonly IMediator _mediator = fixture.ServiceProvider.GetRequiredService<IMediator>();
    private readonly IServiceCurrentUser _testServiceCurrentUser = fixture.ServiceProvider.GetRequiredService<IServiceCurrentUser>();

    private IncomeCategory? _categoryUser1;
    private IncomeCategory? _categoryUser2;

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        _categoryUser1 = await AddCategory(new IncomeCategory
        {
            Name = "Test1",
            Limit = 1000,
            UserId = user1.Id,
        });
        
        _categoryUser2 = await AddCategory(new IncomeCategory
        {
            Name = "Test2",
            Limit = 2000,
            UserId = user2.Id,
        });
        
        await dbContext.SaveChangesAsync();
    }

    private async Task<IncomeCategory> AddCategory(IncomeCategory category)
        => (await dbContext.Set<IncomeCategory>().AddAsync(category)).Entity;

    [Fact]
    public async Task CreateIncome_WithValidData_ShouldCreateIncomeInDB()
    {
        // Arrange
        Assert.NotNull(_categoryUser1);
    
        CommandCreateIncome command = new()
        {
            Data = new InputDTOIncome
            {
                Amount = 100,
                Date = DateTime.Now,
                Description = "Test",
                CategoryId = _categoryUser1.Id,
            }
        };
    
        // Act
        Result result = await _mediator.Send(command);
    
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        
        Income? entity = await dbContext.Set<Income>().FirstOrDefaultAsync();
    
        Assert.NotNull(entity);
        Assert.Equal(entity.Amount, command.Data.Amount);
        Assert.Equal(entity.Date, command.Data.Date);
        Assert.Equal(entity.Description, command.Data.Description);
        Assert.Equal(entity.CategoryId, command.Data.CategoryId);
    }

    [Fact]
    public async Task CreateIncome_WithoutCategoryId_ShouldCreateIncomeInDBWithNullCategoryId()
    {
        // Arrange
        Assert.NotNull(_categoryUser1);
        
        CommandCreateIncome command = new()
        {
            Data = new InputDTOIncome
            {
                Amount = 100,
                Date = DateTime.Now,
                Description = "Test",
            }
        };
    
        // Act
        Result result = await _mediator.Send(command);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        
        Income? entity = await dbContext.Set<Income>().FirstOrDefaultAsync();
        Assert.NotNull(entity);
        Assert.Equal(command.Data.Amount, entity.Amount);
        Assert.Equal(command.Data.Date, entity.Date);
        Assert.Equal(command.Data.Description, entity.Description);
        Assert.Equal(command.Data.CategoryId, entity.CategoryId);
    }

    [Fact]
    public async Task CreateIncome_WithInvalidCategoryIdAndPaymentMethodId_ShouldNotCreateIncomeInDB()
    {
        // Arrange
        CommandCreateIncome command = new()
        {
            Data = new InputDTOIncome
            {
                Amount = 100,
                Date = DateTime.Now,
                Description = "Test",
                CategoryId = -1,
            }
        };
    
        // Act
        Result result = await _mediator.Send(command);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);
    }

    [Fact]
    public async Task GetIncome_WithValidData_ShouldReturnIncomeForView()
    {
        // Arrange
        Assert.NotNull(_categoryUser1);

        Income income = new Income
        {
            Amount = 100,
            Date = DateTime.Now,
            Description = "Test",
            UserId = _testServiceCurrentUser.User.Id,
            CategoryId = _categoryUser1.Id,
        };
        await dbContext.Set<Income>().AddAsync(income);
        
        await dbContext.SaveChangesAsync();
    
        var query = new QueryGetIncomeById { Id = income.Id };
    
        // Act
        Result<OutputDTOIncome> result = await _mediator.Send(query);
    
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        Assert.NotNull(result.Value);
    
        OutputDTOIncome dto = result.Value;
        Assert.NotNull(dto);
        Assert.Equal(query.Id, dto.Id);
        Assert.Equal(dto.Amount, dto.Amount);
        Assert.Equal(dto.Date, dto.Date);
        Assert.Equal(dto.Description, dto.Description);
        Assert.Equal(_categoryUser1.Name, dto.Category);
    }
    
    [Fact]
    public async Task GetIncome_WithInvalidUser_ShouldReturnNull()
    {
        // Arrange
        Assert.NotNull(_categoryUser2);

        Income income = new Income
        {
            Amount = 100,
            Date = DateTime.Now,
            Description = "Test",
            UserId = user2.Id,
            CategoryId = _categoryUser2.Id,
        };
        await dbContext.Set<Income>().AddAsync(income);
        await dbContext.SaveChangesAsync();
    
        var query = new QueryGetIncomeById { Id = income.Id };
    
        // Act
        Result<OutputDTOIncome> result = await _mediator.Send(query);
    
        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);
        Assert.Null(result.Value);
    }
}