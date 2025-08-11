using System;
using System.Collections.Generic;
using Application.Dashboard.DTOs.InputDTOs.Expenses;
using Application.Dashboard.DTOs.OutputDTOs.Expenses;
using Application.Dashboard.DTOs.UpdateDTOs;
using Application.Dashboard.DTOs.UpdateDTOs.Expenses;
using Application.MappingProfiles.Expenses;
using AutoMapper;
using Domain.Dashboard.Entities.Expenses;
using FluentAssertions;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.MappingProfiles.Expenses;

[TestSubject(typeof(MappingProfileExpense))]
public class TestMappingProfileExpense
    : TestMappingProfileFinancialOperation<
        Expense,
        InputDTOExpense,
        OutputDTOExpense,
        UpdateDTOExpense,
        ExpenseCategory
    >
{
    private readonly InputDTOExpense _inputDTO = new()
    {
        Amount = 100,
        Date = DateTime.Today,
        Description = "Test",
        CategoryId = 1,
        PaymentMethodId = 1,
    };

    private readonly UpdateDTOExpense _updateDTO = new()
    {
        Id = 1,
        Amount = new Optional<decimal?>(200),
        Description = new Optional<string?>("Test2"),
        PaymentMethodId = new Optional<int?>(null),
    };

    protected override void AddRequiredProfiles(IList<Profile> profiles)
    {
        base.AddRequiredProfiles(profiles);
        profiles.Add(new MappingProfileExpense());
    }

    protected override InputDTOExpense GetInputDTO() => _inputDTO;
    protected override UpdateDTOExpense GetUpdateDTO() => _updateDTO;
    protected override Expense GetOldEntity() => new()
    {
        Id = 1,
        Amount = 100,
        Date = DateTime.Today,
        Description = "Test",
        CategoryId = 1,
        PaymentMethodId = 1,
        UserId = Guid.NewGuid()
    };

    [Fact]
    public void CheckMapping_InputDTOToEntity()
    {
        CheckMapping_InputDTOToEntity_Base((entity) =>
        {
            entity.Amount.Should().Be(_inputDTO.Amount);
            entity.Date.Should().Be(_inputDTO.Date);
            entity.Description.Should().Be(_inputDTO.Description);
            entity.CategoryId.Should().Be(_inputDTO.CategoryId);
        });
    }
    
    [Fact]
    public void CheckMapping_UpdateDTOToEntity()
    {
        Expense oldEntity = GetOldEntity();
        CheckMapping_UpdateDTOToEntity_Base((entity) =>
        {
            entity.Id.Should().Be(_updateDTO.Id);
            entity.Amount.Should().Be(_updateDTO.Amount.Value);
            entity.Date.Should().Be(oldEntity.Date);
            entity.Description.Should().Be(_updateDTO.Description.Value);
            entity.CategoryId.Should().Be(oldEntity.CategoryId);
            entity.Category.Should().Be(oldEntity.Category);
            entity.PaymentMethodId.Should().Be(_updateDTO.PaymentMethodId.Value);
            entity.PaymentMethod.Should().BeNull();
        });
    }
}