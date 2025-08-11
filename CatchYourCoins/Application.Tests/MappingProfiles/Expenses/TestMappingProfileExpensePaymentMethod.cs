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

[TestSubject(typeof(MappingProfileExpensePaymentMethod))]
public class TestMappingProfileExpensePaymentMethod
    : TestMappingProfileFinancialCategory<
        ExpensePaymentMethod,
        InputDTOExpensePaymentMethod,
        OutputDTOExpensePaymentMethod,
        UpdateDTOExpensePaymentMethod
    >
{
    private readonly InputDTOExpensePaymentMethod _inputDTO = new()
    {
        Name = "Test",
        Limit = 100,
    };
    
    private readonly UpdateDTOExpensePaymentMethod _updateDTO = new()
    {
        Id = 1,
        Limit = new Optional<decimal?>(null),
    };
    
    private readonly ExpensePaymentMethod _oldEntity = new()
    {
        Id = 1,
        Name = "Test",
        Limit = 100,
        UserId = Guid.NewGuid(),
    };
    
    protected override void AddRequiredProfiles(IList<Profile> profiles)
    {
        base.AddRequiredProfiles(profiles);
        profiles.Add(new MappingProfileExpensePaymentMethod());
    }

    protected override InputDTOExpensePaymentMethod GetInputDTO() => _inputDTO;
    protected override UpdateDTOExpensePaymentMethod GetUpdateDTO() => _updateDTO;
    protected override ExpensePaymentMethod GetOldEntity() => _oldEntity;

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
            Assert.Equal(_updateDTO.Id, entity.Id);
            Assert.Equal(_oldEntity.Limit, entity.Limit);
            Assert.Equal(_updateDTO.Limit.Value, entity.Limit);
        });
    }
}