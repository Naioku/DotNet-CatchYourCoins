using System;
using System.Collections.Generic;
using Application.Dashboard.DTOs.CreateDTOs.Expenses;
using Application.Dashboard.DTOs.OutputDTOs.Expenses;
using Application.Dashboard.DTOs.UpdateDTOs.Expenses;
using Application.MappingProfiles.Expenses;
using AutoMapper;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Services;
using FluentAssertions;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.MappingProfiles.Expenses;

[TestSubject(typeof(MappingProfileExpense))]
public class TestMappingProfileExpense
    : TestMappingProfileFinancialOperation<
        Expense,
        CreateDTOExpense,
        OutputDTOExpense,
        UpdateDTOExpense,
        ExpenseCategory
    >
{
    private Expense Entity => new()
    {
        Id = 1,
        Amount = 100,
        Date = DateTime.Today,
        Description = "Test",
        CategoryId = 1,
        Category = new ExpenseCategory
        {
            Id = 1,
            Name = "Test",
            Limit = 100,
            UserId = FactoryUsers.DefaultUser1Anonymous.Id,
        },
        PaymentMethodId = 1,
        PaymentMethod = new ExpensePaymentMethod
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
        profiles.Add(new MappingProfileExpense());
    }

    [Fact]
    public void CheckMapping_CreateDTOToEntity()
    {
        // Arrange
        CreateDTOExpense dto = new()
        {
            Amount = 100,
            Date = DateTime.Today,
            Description = "Test",
            CategoryId = 1,
            PaymentMethodId = 1,
        };

        // Act
        Expense entity = Map_CreateDTOToEntity(dto);

        // Assert
        AssertBaseProperties_CreateDTOToEntity(dto, entity);
    }
    
    [Fact]
    public void CheckMapping_EntityToOutputDTO()
    {
        // Arrange
        Expense entity = Entity;

        // Act
        OutputDTOExpense dto = Map_EntityToOutputDTO(entity);

        // Assert
        AssertBaseProperties_EntityToOutputDTO(entity, dto);
    }

    [Fact]
    public void CheckMapping_UpdateDTOToEntity_UpdateAllToValue()
    {
        // Arrange
        Expense oldEntity = Entity;
        Expense newEntity = Entity;
        UpdateDTOExpense dto = new()
        {
            Id = oldEntity.Id,
            SetAmount = 200,
            SetDescription = "Test2",
            SetPaymentMethodId = 2,
            SetCategoryId = 2,
            SetDate = oldEntity.Date - TimeSpan.FromDays(1),
        };

        // Act
        Map_UpdateDTOToEntity(dto, newEntity);

        // Assert
        AssertBaseProperties_UpdateDTOToEntity_UpdateAllToValue(dto, oldEntity, newEntity);
        newEntity.PaymentMethodId.Should().Be(dto.PaymentMethodId.Value);
        newEntity.PaymentMethod.Should().BeNull();
    }

    [Fact]
    public void CheckMapping_UpdateDTOToEntity_UpdateAllPossibleToNull()
    {
        Expense oldEntity = Entity;
        Expense newEntity = Entity;
        UpdateDTOExpense dto = new()
        {
            Id = oldEntity.Id,
            SetDescription = null,
            SetPaymentMethodId = null,
            SetCategoryId = null,
        };

        // Act
        Map_UpdateDTOToEntity(dto, newEntity);

        // Assert
        AssertBaseProperties_UpdateDTOToEntity_UpdateAllPossibleToNull(dto, oldEntity, newEntity);
        newEntity.PaymentMethodId.Should().BeNull();
        newEntity.PaymentMethod.Should().BeNull();
    }

    [Fact]
    public void CheckMapping_UpdateDTOToEntity_UpdateNone()
    {
        Expense oldEntity = Entity;
        Expense newEntity = Entity;
        UpdateDTOExpense dto = new()
        {
            Id = oldEntity.Id,
        };

        // Act
        Map_UpdateDTOToEntity(dto, newEntity);

        // Assert
        AssertBaseProperties_UpdateDTOToEntity_UpdateNone(dto, oldEntity, newEntity);
        newEntity.PaymentMethodId.Should().Be(oldEntity.PaymentMethodId);
        newEntity.PaymentMethod.Should().Be(oldEntity.PaymentMethod);
    }
}