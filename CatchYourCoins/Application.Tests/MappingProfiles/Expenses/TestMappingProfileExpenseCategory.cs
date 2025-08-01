using System.Collections.Generic;
using Application.DTOs.InputDTOs.Expenses;
using Application.DTOs.OutputDTOs.Expenses;
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
        OutputDTOExpenseCategory
    >
{
    private readonly InputDTOExpenseCategory _dto = new()
    {
        Name = "Test",
        Limit = 100,
    };

    protected override void AddRequiredProfiles(IList<Profile> profiles)
    {
        base.AddRequiredProfiles(profiles);
        profiles.Add(new MappingProfileExpenseCategory());
    }


    protected override InputDTOExpenseCategory GetDTO() => _dto;

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