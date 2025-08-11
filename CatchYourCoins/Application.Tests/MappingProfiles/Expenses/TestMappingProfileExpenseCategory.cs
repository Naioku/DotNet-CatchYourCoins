using System;
using System.Collections.Generic;
using Application.Dashboard.DTOs.InputDTOs.Expenses;
using Application.Dashboard.DTOs.OutputDTOs.Expenses;
using Application.Dashboard.DTOs.UpdateDTOs;
using Application.Dashboard.DTOs.UpdateDTOs.Expenses;
using Application.MappingProfiles.Expenses;
using AutoMapper;
using Domain.Dashboard.Entities.Expenses;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.MappingProfiles.Expenses;

[TestSubject(typeof(MappingProfileExpenseCategory))]
public class TestMappingProfileExpenseCategory
    : TestMappingProfileFinancialCategory<
        ExpenseCategory,
        InputDTOExpenseCategory,
        OutputDTOExpenseCategory,
        UpdateDTOExpenseCategory
    >
{
    private readonly InputDTOExpenseCategory _inputDTO = new()
    {
        Name = "Test",
        Limit = 100,
    };
    
    private readonly UpdateDTOExpenseCategory _updateDTO = new()
    {
        Id = 1,
        Limit = new Optional<decimal?>(null),
    };
    
    private readonly ExpenseCategory _oldEntity = new()
    {
        Id = 1,
        Name = "Test",
        Limit = 100,
        UserId = Guid.NewGuid(),
    };

    protected override void AddRequiredProfiles(IList<Profile> profiles)
    {
        base.AddRequiredProfiles(profiles);
        profiles.Add(new MappingProfileExpenseCategory());
    }

    protected override InputDTOExpenseCategory GetInputDTO() => _inputDTO;
    protected override UpdateDTOExpenseCategory GetUpdateDTO() => _updateDTO;
    protected override ExpenseCategory GetOldEntity() => _oldEntity;

    [Fact]
    public void CheckMapping_InputDTOToEntity()
    {
        CheckMapping_InputDTOToEntity_Base((entity) =>
        {
            Assert.Equal(_inputDTO.Name, entity.Name);
            Assert.Equal(_inputDTO.Limit, entity.Limit);
        });
    }
    
    [Fact]
    public void CheckMapping_UpdateDTOToEntity()
    {
        CheckMapping_UpdateDTOToEntity_Base((entity) =>
        {
            Assert.Equal(_oldEntity.Name, entity.Name);
            Assert.Equal(_updateDTO.Limit.Value, entity.Limit);
        });
    }
}