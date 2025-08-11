using System;
using System.Collections.Generic;
using Application.Dashboard.DTOs.InputDTOs.Incomes;
using Application.Dashboard.DTOs.OutputDTOs.Incomes;
using Application.Dashboard.DTOs.UpdateDTOs;
using Application.Dashboard.DTOs.UpdateDTOs.Incomes;
using Application.MappingProfiles.Incomes;
using AutoMapper;
using Domain.Dashboard.Entities.Incomes;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.MappingProfiles.Incomes;

[TestSubject(typeof(MappingProfileIncomeCategory))]
public class TestMappingProfileIncomeCategory
    : TestMappingProfileFinancialCategory<
        IncomeCategory,
        InputDTOIncomeCategory,
        OutputDTOIncomeCategory,
        UpdateDTOIncomeCategory
    >
{
    private readonly InputDTOIncomeCategory _inputDTO = new()
    {
        Name = "Test",
        Limit = 100,
    };
    
    private readonly UpdateDTOIncomeCategory _updateDTO = new()
    {
        Id = 1,
        Limit = new Optional<decimal?>(null),
    };
    
    private readonly IncomeCategory _oldEntity = new()
    {
        Id = 1,
        Name = "Test",
        Limit = 100,
        UserId = Guid.NewGuid(),
    };

    protected override void AddRequiredProfiles(IList<Profile> profiles)
    {
        base.AddRequiredProfiles(profiles);
        profiles.Add(new MappingProfileIncomeCategory());
    }

    protected override InputDTOIncomeCategory GetInputDTO() => _inputDTO;
    protected override UpdateDTOIncomeCategory GetUpdateDTO() => _updateDTO;
    protected override IncomeCategory GetOldEntity() => _oldEntity;

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