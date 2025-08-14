using System.Collections.Generic;
using Application.Dashboard.DTOs.CreateDTOs.Expenses;
using Application.Dashboard.DTOs.OutputDTOs.Expenses;
using Application.Dashboard.DTOs.UpdateDTOs.Expenses;
using Application.MappingProfiles.Expenses;
using AutoMapper;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Services;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.MappingProfiles.Expenses;

[TestSubject(typeof(MappingProfileExpensePaymentMethod))]
public class TestMappingProfileExpensePaymentMethod
    : TestMappingProfileFinancialCategory<
        ExpensePaymentMethod,
        CreateDTOExpensePaymentMethod,
        OutputDTOExpensePaymentMethod,
        UpdateDTOExpensePaymentMethod
    >
{
    private ExpensePaymentMethod Entity => new()
    {
        Id = 1,
        Name = "Test",
        Limit = 100,
        UserId = GetMock<IServiceCurrentUser>().Object.User.Id,
    };

    protected override void AddRequiredProfiles(IList<Profile> profiles)
    {
        base.AddRequiredProfiles(profiles);
        profiles.Add(new MappingProfileExpensePaymentMethod());
    }
    
    [Fact]
    public void CheckMapping_CreateDTOToEntity()
    {
        // Arrange
        CreateDTOExpensePaymentMethod dto = new()
        {
            Name = "Test",
            Limit = 100,
        };
        
        // Act
        ExpensePaymentMethod entity = Map_CreateDTOToEntity(dto);
        
        // Assert
        AssertBaseProperties_CreateDTOToEntity(dto, entity);
    }
    
    [Fact]
    public void CheckMapping_EntityToOutputDTO()
    {
        // Arrange
        ExpensePaymentMethod entity = Entity;

        // Act
        OutputDTOExpensePaymentMethod dto = Map_EntityToOutputDTO(entity);

        // Assert
        AssertBaseProperties_EntityToOutputDTO(entity, dto);
    }
    
    [Fact]
    public void CheckMapping_UpdateDTOToEntity_UpdateAllToValue()
    {
        // Arrange
        ExpensePaymentMethod oldEntity = Entity;
        ExpensePaymentMethod newEntity = Entity;
        UpdateDTOExpensePaymentMethod dto = new()
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
        ExpensePaymentMethod oldEntity = Entity;
        ExpensePaymentMethod newEntity = Entity;
        UpdateDTOExpensePaymentMethod dto = new()
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
        ExpensePaymentMethod oldEntity = Entity;
        ExpensePaymentMethod newEntity = Entity;
        UpdateDTOExpensePaymentMethod dto = new()
        {
            Id = oldEntity.Id,
        };

        // Act
        Map_UpdateDTOToEntity(dto, newEntity);

        // Assert
        AssertBaseProperties_UpdateDTOToEntity_UpdateNone(dto, oldEntity, newEntity);
    }
}