using System;
using System.Collections.Generic;
using Application.Dashboard.DTOs.CreateDTOs.Incomes;
using Application.Dashboard.DTOs.OutputDTOs.Incomes;
using Application.Dashboard.DTOs.UpdateDTOs;
using Application.Dashboard.DTOs.UpdateDTOs.Incomes;
using Application.MappingProfiles.Incomes;
using AutoMapper;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Services;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.MappingProfiles.Incomes;

[TestSubject(typeof(MappingProfileIncome))]
public class TestMappingProfileIncome
    : TestMappingProfileFinancialOperation<
        Income,
        CreateDTOIncome,
        OutputDTOIncome,
        UpdateDTOIncome,
        IncomeCategory
    >
{
    private Income Entity => new()
    {
        Id = 1,
        Amount = 100,
        Date = DateTime.Today,
        Description = "Test",
        CategoryId = 1,
        Category = new IncomeCategory
        {
            Id = 1,
            Name = "Test",
            Limit = 100,
            UserId = FactoryUsers.DefaultUser1Anonymous.Id,
        },
        UserId = GetMock<IServiceCurrentUser>().Object.User.Id,
    };

    protected override void AddRequiredProfiles(IList<Profile> profiles)
    {
        base.AddRequiredProfiles(profiles);
        profiles.Add(new MappingProfileIncome());
    }

    [Fact]
    public void CheckMapping_InputDTOToEntity()
    {
        // Arrange
        CreateDTOIncome dto = new()
        {
            Amount = 100,
            Date = DateTime.Now,
            Description = "Test",
            CategoryId = 1,
        };
        
        // Act
        Income entity = Map_CreateDTOToEntity(dto);
        
        // Assert
        AssertBaseProperties_CreateDTOToEntity(dto, entity);
    }
    
    [Fact]
    public void CheckMapping_EntityToOutputDTO()
    {
        // Arrange
        Income entity = Entity;

        // Act
        OutputDTOIncome dto = Map_EntityToOutputDTO(entity);

        // Assert
        AssertBaseProperties_EntityToOutputDTO(entity, dto);
    }
    
    [Fact]
    public void CheckMapping_UpdateDTOToEntity_UpdateAllToValue()
    {
        // Arrange
        Income oldEntity = Entity;
        Income newEntity = Entity;
        UpdateDTOIncome dto = new()
        {
            Id = oldEntity.Id,
            Amount = new Optional<decimal>(200),
            Description = new Optional<string?>("Test2"),
            CategoryId = new Optional<int?>(2),
            Date = new Optional<DateTime>(oldEntity.Date - TimeSpan.FromDays(1)),
        };

        // Act
        Map_UpdateDTOToEntity(dto, newEntity);

        // Assert
        AssertBaseProperties_UpdateDTOToEntity_UpdateAllToValue(dto, oldEntity, newEntity);
    }

    [Fact]
    public void CheckMapping_UpdateDTOToEntity_UpdateAllPossibleToNull()
    {
        Income oldEntity = Entity;
        Income newEntity = Entity;
        UpdateDTOIncome dto = new()
        {
            Id = oldEntity.Id,
            Description = new Optional<string?>(null),
            CategoryId = new Optional<int?>(null),
        };

        // Act
        Map_UpdateDTOToEntity(dto, newEntity);

        // Assert
        AssertBaseProperties_UpdateDTOToEntity_UpdateAllPossibleToNull(dto, oldEntity, newEntity);
    }

    [Fact]
    public void CheckMapping_UpdateDTOToEntity_UpdateNone()
    {
        Income oldEntity = Entity;
        Income newEntity = Entity;
        UpdateDTOIncome dto = new()
        {
            Id = oldEntity.Id,
        };

        // Act
        Map_UpdateDTOToEntity(dto, newEntity);

        // Assert
        AssertBaseProperties_UpdateDTOToEntity_UpdateNone(dto, oldEntity, newEntity);
    }
}