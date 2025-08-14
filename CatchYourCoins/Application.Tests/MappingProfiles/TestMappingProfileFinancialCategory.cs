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

public abstract class TestMappingProfileFinancialCategory<TEntity, TCreateDTO, TOutputDTO, TUpdateDTO>
    : TestMappingProfileBase<TEntity, TCreateDTO, TOutputDTO, TUpdateDTO>
    where TEntity : FinancialCategory
    where TCreateDTO : CreateDTOFinancialCategory
    where TOutputDTO : OutputDTOFinancialCategory
    where TUpdateDTO : UpdateDTOFinancialCategory
{
    protected override void AddRequiredProfiles(IList<Profile> profiles) =>
        profiles.Add(new MappingProfileFinancialCategory(GetMock<IServiceCurrentUser>().Object));
    
    protected void AssertBaseProperties_CreateDTOToEntity(TCreateDTO dto, TEntity newEntity)
    {
        newEntity.UserId.Should().Be(GetMock<IServiceCurrentUser>().Object.User.Id);
        newEntity.Name.Should().Be(dto.Name);
        newEntity.Limit.Should().Be(dto.Limit);
    }
    
    protected void AssertBaseProperties_EntityToOutputDTO(TEntity entity, TOutputDTO dto)
    {
        dto.Id.Should().Be(entity.Id);
        dto.Name.Should().Be(entity.Name);
        dto.Limit.Should().Be(entity.Limit);
    }
    
    protected void AssertBaseProperties_UpdateDTOToEntity_UpdateAllToValue(TUpdateDTO dto, TEntity newEntity)
    {
        newEntity.Id.Should().Be(dto.Id);
        newEntity.UserId.Should().Be(GetMock<IServiceCurrentUser>().Object.User.Id);
        newEntity.Name.Should().Be(dto.Name.Value);
        newEntity.Limit.Should().Be(dto.Limit.Value);
    }
    
    protected void AssertBaseProperties_UpdateDTOToEntity_UpdateAllPossibleToNull(TUpdateDTO dto, TEntity oldEntity, TEntity newEntity)
    {
        newEntity.Id.Should().Be(dto.Id);
        newEntity.UserId.Should().Be(GetMock<IServiceCurrentUser>().Object.User.Id);
        newEntity.Name.Should().Be(oldEntity.Name);
        newEntity.Limit.Should().Be(dto.Limit.Value);
    }
    
    protected void AssertBaseProperties_UpdateDTOToEntity_UpdateNone(TUpdateDTO dto, TEntity oldEntity, TEntity newEntity)
    {
        newEntity.Id.Should().Be(dto.Id);
        newEntity.UserId.Should().Be(GetMock<IServiceCurrentUser>().Object.User.Id);
        newEntity.Name.Should().Be(oldEntity.Name);
        newEntity.Limit.Should().Be(oldEntity.Limit);
    }
}