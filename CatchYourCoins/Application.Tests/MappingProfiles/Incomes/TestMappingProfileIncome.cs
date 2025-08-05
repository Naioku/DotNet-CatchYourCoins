using System;
using System.Collections.Generic;
using Application.Dashboard.DTOs.InputDTOs.Incomes;
using Application.Dashboard.DTOs.OutputDTOs.Incomes;
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
        IncomeCategory
    >
{
    private readonly InputDTOIncome _dto = new()
    {
        Amount = 100,
        Date = DateTime.Now,
        Description = "Test",
        CategoryId = 1,
    };

    protected override void AddRequiredProfiles(IList<Profile> profiles)
    {
        base.AddRequiredProfiles(profiles);
        profiles.Add(new MappingProfileIncome());
    }

    protected override InputDTOIncome GetDTO() => _dto;

    [Fact]
    public void CheckMapping_InputDTOToEntity()
    {
        CheckMapping_InputDTOToEntity_Base((entity) =>
        {
            Assert.Equal(_dto.Amount, entity.Amount);
            Assert.Equal(_dto.Date, entity.Date);
            Assert.Equal(_dto.Description, entity.Description);
            Assert.Equal(_dto.CategoryId, entity.CategoryId);
        });
    }
}