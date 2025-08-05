using System.Collections.Generic;
using Application.Dashboard.DTOs.InputDTOs;
using Application.Dashboard.DTOs.OutputDTOs;
using Application.MappingProfiles;
using AutoMapper;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Services;

namespace Application.Tests.MappingProfiles;

public abstract class TestMappingProfileFinancialOperation<TEntity, TInputDTO, TOutputDTO, TCategory>
    : TestMappingProfileBase<TEntity, TInputDTO, TOutputDTO>
    where TEntity : FinancialOperation<TCategory>
    where TInputDTO : InputDTOFinancialOperation
    where TOutputDTO : OutputDTOFinancialOperation
    where TCategory : FinancialCategory
{
    protected override void AddRequiredProfiles(IList<Profile> profiles) =>
        profiles.Add(new MappingProfileFinancialOperation(GetMock<IServiceCurrentUser>().Object));
}