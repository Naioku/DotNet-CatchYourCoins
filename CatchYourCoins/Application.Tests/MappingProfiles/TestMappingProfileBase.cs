using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;

namespace Application.Tests.MappingProfiles;

public abstract class TestMappingProfileBase<TEntity, TInputDTO, TOutputDTO> : TestBase
{
    protected abstract void AddRequiredProfiles(IList<Profile> profiles);

    protected abstract TInputDTO GetDTO();

    protected void CheckMapping_InputDTOToEntity_Base(Action<TEntity> assertions)
    {
        // Arrange
        MapperConfiguration config = new(config =>
            {
                List<Profile> profiles = [];
                AddRequiredProfiles(profiles);
                
                foreach (Profile profile in profiles)
                {
                    config.AddProfile(profile);
                }
            },
            new NullLoggerFactory()
        );
        IMapper mapper = config.CreateMapper();
        
        // Act
        TEntity result = mapper.Map<TEntity>(GetDTO());
        
        // Assert
        assertions(result);
    }
}