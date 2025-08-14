using System.Collections.Generic;
using Application.MappingProfiles;
using AutoMapper;
using Domain.Interfaces.Services;
using Microsoft.Extensions.Logging.Abstractions;

namespace Application.Tests.MappingProfiles;

public abstract class TestMappingProfileBase<TEntity, TCreateDTO, TOutputDTO, TUpdateDTO> : TestBase
{
    protected abstract void AddRequiredProfiles(IList<Profile> profiles);

    private IMapper CreateMapper()
    {
        MapperConfiguration config = new(config =>
            {
                List<Profile> profiles =
                [
                    new MappingProfileDashboardEntity(GetMock<IServiceCurrentUser>().Object)
                ];
                AddRequiredProfiles(profiles);
                
                foreach (Profile profile in profiles)
                {
                    config.AddProfile(profile);
                }
            },
            new NullLoggerFactory()
        );
        
        return config.CreateMapper();
    }
    
    protected TEntity Map_CreateDTOToEntity(TCreateDTO dto) => CreateMapper().Map<TEntity>(dto);
    protected TOutputDTO Map_EntityToOutputDTO(TEntity entity) => CreateMapper().Map<TOutputDTO>(entity);
    protected void Map_UpdateDTOToEntity(TUpdateDTO dto, TEntity entity) => CreateMapper().Map(dto, entity);
}