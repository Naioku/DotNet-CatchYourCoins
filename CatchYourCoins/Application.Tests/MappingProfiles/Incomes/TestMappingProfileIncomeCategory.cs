using System.Collections.Generic;
using Application.DTOs.InputDTOs.Incomes;
using Application.DTOs.OutputDTOs.Incomes;
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
        OutputDTOIncomeCategory
    >
{
    private readonly InputDTOIncomeCategory _dto = new()
    {
        Name = "Test",
        Limit = 100,
    };

    protected override void AddRequiredProfiles(IList<Profile> profiles)
    {
        base.AddRequiredProfiles(profiles);
        profiles.Add(new MappingProfileIncomeCategory());
    }


    protected override InputDTOIncomeCategory GetDTO() => _dto;

    [Fact]
    public void CheckMapping_InputDTOToEntity()
    {
        CheckMapping_InputDTOToEntity_Base((entity) =>
        {
            Assert.Equal(_dto.Name, entity.Name);
            Assert.Equal(_dto.Limit, entity.Limit);
        });
    }
}