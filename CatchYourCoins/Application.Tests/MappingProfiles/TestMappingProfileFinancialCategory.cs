using System.Collections.Generic;
using Application.Dashboard.DTOs.InputDTOs;
using Application.Dashboard.DTOs.OutputDTOs;
using Application.MappingProfiles;
using AutoMapper;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Services;

namespace Application.Tests.MappingProfiles;

public abstract class TestMappingProfileFinancialCategory<TEntity, TInputDTO, TOutputDTO>
    : TestMappingProfileBase<TEntity, TInputDTO, TOutputDTO>
    where TEntity : FinancialCategory
    where TInputDTO : InputDTOFinancialCategory
    where TOutputDTO : OutputDTOFinancialCategory
{
    protected override void AddRequiredProfiles(IList<Profile> profiles) =>
        profiles.Add(new MappingProfileFinancialCategory(GetMock<IServiceCurrentUser>().Object));
}