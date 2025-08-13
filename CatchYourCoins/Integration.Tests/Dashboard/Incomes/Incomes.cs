using Application.Dashboard.Commands;
using Application.Dashboard.DTOs.CreateDTOs.Incomes;
using Application.Dashboard.DTOs.OutputDTOs.Incomes;
using Application.Dashboard.DTOs.UpdateDTOs;
using Application.Dashboard.DTOs.UpdateDTOs.Incomes;
using Application.Dashboard.Queries;
using Domain;
using Domain.Dashboard.Entities.Incomes;
using Domain.Dashboard.Specifications.Incomes;
using Domain.Interfaces.Services;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Dashboard.Incomes;

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
    
        CommandCRUDCreate<CreateDTOIncome> command = new()
        {
            Data = new CreateDTOIncome
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
        
        CommandCRUDCreate<CreateDTOIncome> command = new()
        {
            Data = new CreateDTOIncome
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
        CommandCRUDCreate<CreateDTOIncome> command = new()
        {
            Data = new CreateDTOIncome
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

        QueryCRUDGet<Income, OutputDTOIncome> query = new()
        {
            Specification = SpecificationIncome.GetBuilder()
                .WithId(entity.Id)
                .Build(),
        };
    
        // Act
        Result<IReadOnlyList<OutputDTOIncome>> result = await _mediator.Send(query);
    
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Value.Should().NotBeNull();
    
        OutputDTOIncome dto = result.Value[0];
        
        dto.Should().NotBeNull();
        dto.Id.Should().Be(entity.Id);
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

        Income entity = new()
        {
            Amount = 100,
            Date = DateTime.Now,
            Description = "Test",
            UserId = user2.Id,
            CategoryId = _categoryUser2.Id,
        };
        await dbContext.Set<Income>().AddAsync(entity);
        await dbContext.SaveChangesAsync();
    
        QueryCRUDGet<Income, OutputDTOIncome> query = new()
        {
            Specification = SpecificationIncome.GetBuilder()
                .WithId(entity.Id)
                .Build(),
        };
    
        // Act
        Result<IReadOnlyList<OutputDTOIncome>> result = await _mediator.Send(query);
    
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Value.Should().BeNull();
    }
    
    [Fact]
    public async Task UpdateIncome_WithValidData_ShouldUpdateIncomeInDB()
    {
        // Arrange
        Assert.NotNull(_categoryUser1);
        IReadOnlyList<Income> entities =
        [
            new()
            {
                Amount = 100,
                Date = DateTime.Today,
                Description = "Test",
                UserId = _testServiceCurrentUser.User.Id,
                CategoryId = _categoryUser1.Id,
            }
        ];
        await dbContext.Set<Income>().AddRangeAsync(entities);
        await dbContext.SaveChangesAsync();
        dbContext.ChangeTracker.Clear();

        CommandCRUDUpdate<Income, UpdateDTOIncome> command = new()
        {
            Specification = SpecificationIncome.GetBuilder()
                .WithIdRange(entities.Select(e => e.Id).ToList())
                .Build(),
            Data =
            [
                new UpdateDTOIncome
                {
                    Id = entities[0].Id,
                    Amount = new Optional<decimal>(200),
                    Description = new Optional<string?>("Test2"),
                }
            ]
        };

        // Act
        Result result = await _mediator.Send(command);
        dbContext.ChangeTracker.Clear();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();

        IReadOnlyList<Income> entitiesUpdated = await dbContext.Set<Income>()
            .Include(c => c.Category)
            .ToListAsync();

        entitiesUpdated.Should().NotBeEmpty();

        for (var i = 0; i < entitiesUpdated.Count; i++)
        {
            Income expense = entitiesUpdated[i];
            expense.UserId.Should().Be(_testServiceCurrentUser.User.Id);
            expense.Amount.Should().Be(command.Data[i].Amount.Value);
            expense.Date.Should().Be(entities[i].Date);
            expense.Description.Should().Be(command.Data[i].Description.Value);
            expense.CategoryId.Should().Be(entities[i].CategoryId);
            expense.Category.Should().NotBeNull();
        }
    }
}