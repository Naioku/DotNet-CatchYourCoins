using System.Collections.Generic;
using Application.Dashboard.DTOs.CreateDTOs.Incomes;
using Application.Dashboard.DTOs.OutputDTOs.Incomes;
using Application.Dashboard.DTOs.UpdateDTOs.Incomes;
using Application.MappingProfiles.Incomes;
using AutoMapper;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Services;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.MappingProfiles.Incomes;

[TestSubject(typeof(MappingProfileIncomeCategory))]
public class TestMappingProfileIncomeCategory
    : TestMappingProfileFinancialCategory<
        IncomeCategory,
        CreateDTOIncomeCategory,
        OutputDTOIncomeCategory,
        UpdateDTOIncomeCategory
    >
{
    private IncomeCategory Entity => new()
    {
        Id = 1,
        Name = "Test",
        Limit = 100,
        UserId = GetMock<IServiceCurrentUser>().Object.User.Id,
    };

    protected override void AddRequiredProfiles(IList<Profile> profiles)
    {
        base.AddRequiredProfiles(profiles);
        profiles.Add(new MappingProfileIncomeCategory());
    }

    [Fact]
    public void CheckMapping_CreateDTOToEntity()
    {
        // Arrange
        CreateDTOIncomeCategory dto = new()
        {
            Name = "Test",
            Limit = 100,
        };
        
        // Act
        IncomeCategory entity = Map_CreateDTOToEntity(dto);
        
        // Assert
        AssertBaseProperties_CreateDTOToEntity(dto, entity);
    }
    
    [Fact]
    public void CheckMapping_EntityToOutputDTO()
    {
        // Arrange
        IncomeCategory entity = Entity;

        // Act
        OutputDTOIncomeCategory dto = Map_EntityToOutputDTO(entity);

        // Assert
        AssertBaseProperties_EntityToOutputDTO(entity, dto);
    }
    
    [Fact]
    public void CheckMapping_UpdateDTOToEntity_UpdateAllToValue()
    {
        // Arrange
        IncomeCategory oldEntity = Entity;
        IncomeCategory newEntity = Entity;
        UpdateDTOIncomeCategory dto = new()
        {
            Id = oldEntity.Id,
            SetName = "Test2",
            SetLimit = 200,
        };

        // Act
        Map_UpdateDTOToEntity(dto, newEntity);

        // Assert
        AssertBaseProperties_UpdateDTOToEntity_UpdateAllToValue(dto, newEntity);
    }

    [Fact]
    public void CheckMapping_UpdateDTOToEntity_UpdateAllPossibleToNull()
    {
        IncomeCategory oldEntity = Entity;
        IncomeCategory newEntity = Entity;
        UpdateDTOIncomeCategory dto = new()
        {
            Id = oldEntity.Id,
            SetLimit = 200,
        };

        // Act
        Map_UpdateDTOToEntity(dto, newEntity);

        // Assert
        AssertBaseProperties_UpdateDTOToEntity_UpdateAllPossibleToNull(dto, oldEntity, newEntity);
    }

    [Fact]
    public void CheckMapping_UpdateDTOToEntity_UpdateNone()
    {
        IncomeCategory oldEntity = Entity;
        IncomeCategory newEntity = Entity;
        UpdateDTOIncomeCategory dto = new()
        {
            Id = oldEntity.Id,
        };

        // Act
        Map_UpdateDTOToEntity(dto, newEntity);

        // Assert
        AssertBaseProperties_UpdateDTOToEntity_UpdateNone(dto, oldEntity, newEntity);
    }
}