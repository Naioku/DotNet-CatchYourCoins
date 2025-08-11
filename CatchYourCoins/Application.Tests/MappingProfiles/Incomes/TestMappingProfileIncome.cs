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

[TestSubject(typeof(MappingProfileIncome))]
public class TestMappingProfileIncome
    : TestMappingProfileFinancialOperation<
        Income,
        InputDTOIncome,
        OutputDTOIncome,
        UpdateDTOIncome,
        IncomeCategory
    >
{
    private readonly InputDTOIncome _inputDTO = new()
    {
        Amount = 100,
        Date = DateTime.Now,
        Description = "Test",
        CategoryId = 1,
    };
    
    private readonly UpdateDTOIncome _updateDTO = new()
    {
        Id = 1,
        Amount = new Optional<decimal?>(200),
        Description = new Optional<string?>("Test2"),
    };

    private readonly Income _oldEntity = new()
    {
        Id = 1,
        Amount = 100,
        Date = DateTime.Now,
        Description = "Test",
        CategoryId = 1,
        UserId = Guid.NewGuid()
    };

    protected override void AddRequiredProfiles(IList<Profile> profiles)
    {
        base.AddRequiredProfiles(profiles);
        profiles.Add(new MappingProfileIncome());
    }

    protected override InputDTOIncome GetInputDTO() => _inputDTO;
    protected override UpdateDTOIncome GetUpdateDTO() => _updateDTO;
    protected override Income GetOldEntity() => _oldEntity;

    [Fact]
    public void CheckMapping_InputDTOToEntity()
    {
        CheckMapping_InputDTOToEntity_Base((entity) =>
        {
            Assert.Equal(_inputDTO.Amount, entity.Amount);
            Assert.Equal(_inputDTO.Date, entity.Date);
            Assert.Equal(_inputDTO.Description, entity.Description);
            Assert.Equal(_inputDTO.CategoryId, entity.CategoryId);
        });
    }
    
    [Fact]
    public void CheckMapping_UpdateDTOToEntity()
    {
        CheckMapping_UpdateDTOToEntity_Base((entity) =>
        {
            Assert.Equal(_updateDTO.Id, entity.Id);
            Assert.Equal(_updateDTO.Amount.Value, entity.Amount);
            Assert.Equal(_oldEntity.Date, entity.Date);
            Assert.Equal(_updateDTO.Description.Value, entity.Description);
            Assert.Equal(_oldEntity.CategoryId, entity.CategoryId);
        });
    }
}