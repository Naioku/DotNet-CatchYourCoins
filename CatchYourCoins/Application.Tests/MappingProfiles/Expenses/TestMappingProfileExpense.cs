using System;
using System.Collections.Generic;
using Application.DTOs.InputDTOs.Expenses;
using Application.DTOs.OutputDTOs.Expenses;
using Application.MappingProfiles.Expenses;
using AutoMapper;
using Domain.Dashboard.Entities.Expenses;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.MappingProfiles.Expenses;

[TestSubject(typeof(MappingProfileExpense))]
public class TestMappingProfileExpense
    : TestMappingProfileFinancialOperation<
        Expense,
        InputDTOExpense,
        OutputDTOExpense,
        ExpenseCategory
    >
{
    private readonly InputDTOExpense _dto = new()
    {
        Amount = 100,
        Date = DateTime.Now,
        Description = "Test",
        CategoryId = 1,
    };

    protected override void AddRequiredProfiles(IList<Profile> profiles)
    {
        base.AddRequiredProfiles(profiles);
        profiles.Add(new MappingProfileExpense());
    }

    protected override InputDTOExpense GetDTO() => _dto;

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