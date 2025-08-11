using System.Collections.Generic;
using Application.Dashboard.DTOs.InputDTOs;
using Application.Dashboard.DTOs.OutputDTOs;
using Application.Dashboard.DTOs.UpdateDTOs;
using Application.MappingProfiles;
using AutoMapper;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Services;

namespace Application.Tests.MappingProfiles;

public abstract class TestMappingProfileFinancialCategory<TEntity, TInputDTO, TOutputDTO, TUpdateDTO>
    : TestMappingProfileBase<TEntity, TInputDTO, TOutputDTO, TUpdateDTO>
    where TEntity : FinancialCategory
    where TInputDTO : InputDTOFinancialCategory
    where TOutputDTO : OutputDTOFinancialCategory
    where TUpdateDTO : UpdateDTOFinancialCategory
{
    protected override void AddRequiredProfiles(IList<Profile> profiles) =>
        profiles.Add(new MappingProfileFinancialCategory(GetMock<IServiceCurrentUser>().Object));
}