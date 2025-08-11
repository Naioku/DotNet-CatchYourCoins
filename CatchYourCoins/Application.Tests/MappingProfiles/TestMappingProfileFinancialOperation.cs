using System.Collections.Generic;
using Application.Dashboard.DTOs.InputDTOs;
using Application.Dashboard.DTOs.OutputDTOs;
using Application.Dashboard.DTOs.UpdateDTOs;
using Application.MappingProfiles;
using AutoMapper;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Services;

namespace Application.Tests.MappingProfiles;

public abstract class TestMappingProfileFinancialOperation<TEntity, TInputDTO, TOutputDTO, TUpdateDTO, TCategory>
    : TestMappingProfileBase<TEntity, TInputDTO, TOutputDTO, TUpdateDTO>
    where TEntity : FinancialOperation<TCategory>
    where TInputDTO : InputDTOFinancialOperation
    where TOutputDTO : OutputDTOFinancialOperation
    where TUpdateDTO : UpdateDTOFinancialOperation
    where TCategory : FinancialCategory
{
    protected override void AddRequiredProfiles(IList<Profile> profiles) =>
        profiles.Add(new MappingProfileFinancialOperation(GetMock<IServiceCurrentUser>().Object));
}