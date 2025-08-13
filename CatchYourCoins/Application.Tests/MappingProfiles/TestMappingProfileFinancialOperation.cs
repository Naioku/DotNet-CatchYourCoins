using System.Collections.Generic;
using Application.Dashboard.DTOs.CreateDTOs;
using Application.Dashboard.DTOs.OutputDTOs;
using Application.Dashboard.DTOs.UpdateDTOs;
using Application.MappingProfiles;
using AutoMapper;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Services;
using FluentAssertions;

namespace Application.Tests.MappingProfiles;

public abstract class TestMappingProfileFinancialOperation<TEntity, TCreateDTO, TOutputDTO, TUpdateDTO, TCategory>
    : TestMappingProfileBase<TEntity, TCreateDTO, TOutputDTO, TUpdateDTO>
    where TEntity : FinancialOperation<TCategory>
    where TCreateDTO : CreateDTOFinancialOperation
    where TOutputDTO : OutputDTOFinancialOperation
    where TUpdateDTO : UpdateDTOFinancialOperation
    where TCategory : FinancialCategory
{
    protected override void AddRequiredProfiles(IList<Profile> profiles) =>
        profiles.Add(new MappingProfileFinancialOperation(GetMock<IServiceCurrentUser>().Object));

    protected void AssertBaseProperties_CreateDTOToEntity(TCreateDTO dto, TEntity newEntity)
    {
        newEntity.Amount.Should().Be(dto.Amount);
        newEntity.Date.Should().Be(dto.Date);
        newEntity.Description.Should().Be(dto.Description);
        newEntity.CategoryId.Should().Be(dto.CategoryId);
    }
    
    protected void AssertBaseProperties_UpdateDTOToEntity_UpdateAllToValue(TUpdateDTO dto, TEntity oldEntity, TEntity newEntity)
    {
        newEntity.Id.Should().Be(dto.Id);
        newEntity.Amount.Should().Be(dto.Amount.Value);
        newEntity.Date.Should().Be(dto.Date.Value);
        newEntity.Description.Should().Be(dto.Description.Value);
        newEntity.CategoryId.Should().Be(dto.CategoryId.Value);
        newEntity.Category.Should().BeNull();
        newEntity.UserId.Should().Be(oldEntity.UserId);
    }
    
    protected void AssertBaseProperties_UpdateDTOToEntity_UpdateAllPossibleToNull(TUpdateDTO dto, TEntity oldEntity, TEntity newEntity)
    {
        newEntity.Id.Should().Be(dto.Id);
        newEntity.Amount.Should().Be(oldEntity.Amount);
        newEntity.Date.Should().Be(oldEntity.Date);
        newEntity.Description.Should().BeNull();
        newEntity.CategoryId.Should().BeNull();
        newEntity.Category.Should().BeNull();
        newEntity.UserId.Should().Be(oldEntity.UserId);
    }
    
    protected void AssertBaseProperties_UpdateDTOToEntity_UpdateNone(TUpdateDTO dto, TEntity oldEntity, TEntity newEntity)
    {
        newEntity.Id.Should().Be(dto.Id);
        newEntity.Amount.Should().Be(oldEntity.Amount);
        newEntity.Date.Should().Be(oldEntity.Date);
        newEntity.Description.Should().Be(oldEntity.Description);
        newEntity.CategoryId.Should().Be(oldEntity.CategoryId);
        newEntity.Category.Should().Be(oldEntity.Category);
        newEntity.UserId.Should().Be(oldEntity.UserId);
    }
}