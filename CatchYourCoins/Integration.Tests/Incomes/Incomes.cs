using Application.DTOs.InputDTOs.Incomes;
using Application.DTOs.OutputDTOs.Incomes;
using Application.Requests.Commands;
using Application.Requests.Queries;
using Domain;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Services;
using FluentAssertions;
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
    
        CommandCRUDCreate<InputDTOIncome> command = new()
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
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        
        Income? entity = await dbContext.Set<Income>().FirstOrDefaultAsync();
    
        entity.Should().NotBeNull();
        entity.UserId.Should().Be(_testServiceCurrentUser.User.Id);
        entity.Amount.Should().Be(command.Data.Amount);
        entity.Date.Should().Be(command.Data.Date);
        entity.Description.Should().Be(command.Data.Description);
        entity.CategoryId.Should().Be(command.Data.CategoryId);
    }

    [Fact]
    public async Task CreateIncome_WithoutCategoryId_ShouldCreateIncomeInDBWithNullCategoryId()
    {
        // Arrange
        Assert.NotNull(_categoryUser1);
        
        CommandCRUDCreate<InputDTOIncome> command = new()
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
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        
        Income? entity = await dbContext.Set<Income>().FirstOrDefaultAsync();
        
        entity.Should().NotBeNull();
        entity.UserId.Should().Be(_testServiceCurrentUser.User.Id);
        entity.Amount.Should().Be(command.Data.Amount);
        entity.Date.Should().Be(command.Data.Date);
        entity.Description.Should().Be(command.Data.Description);
        entity.CategoryId.Should().BeNull();
    }

    [Fact]
    public async Task CreateIncome_WithInvalidCategoryIdAndPaymentMethodId_ShouldNotCreateIncomeInDB()
    {
        // Arrange
        CommandCRUDCreate<InputDTOIncome> command = new()
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
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetIncome_WithValidData_ShouldReturnIncomeForView()
    {
        // Arrange
        Assert.NotNull(_categoryUser1);

        Income entity = new()
        {
            Amount = 100,
            Date = DateTime.Now,
            Description = "Test",
            UserId = _testServiceCurrentUser.User.Id,
            CategoryId = _categoryUser1.Id,
        };
        await dbContext.Set<Income>().AddAsync(entity);
        
        await dbContext.SaveChangesAsync();
    
        QueryCRUDGetById<OutputDTOIncome> query = new() { Id = entity.Id };
    
        // Act
        Result<OutputDTOIncome> result = await _mediator.Send(query);
    
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Value.Should().NotBeNull();
    
        OutputDTOIncome dto = result.Value;
        
        dto.Should().NotBeNull();
        dto.Id.Should().Be(query.Id);
        dto.Amount.Should().Be(entity.Amount);
        dto.Date.Should().Be(entity.Date);
        dto.Description.Should().Be(entity.Description);
        dto.Category.Should().Be(_categoryUser1.Name);
    }
    
    [Fact]
    public async Task GetIncome_WithInvalidUser_ShouldReturnNull()
    {
        // Arrange
        Assert.NotNull(_categoryUser2);

        Income income = new()
        {
            Amount = 100,
            Date = DateTime.Now,
            Description = "Test",
            UserId = user2.Id,
            CategoryId = _categoryUser2.Id,
        };
        await dbContext.Set<Income>().AddAsync(income);
        await dbContext.SaveChangesAsync();
    
        QueryCRUDGetById<OutputDTOIncome> query = new() { Id = income.Id };
    
        // Act
        Result<OutputDTOIncome> result = await _mediator.Send(query);
    
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Value.Should().BeNull();
    }
}