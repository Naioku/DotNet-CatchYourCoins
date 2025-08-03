using System.Collections.Generic;
using System.Linq;

namespace Application.Tests.Factories.DTOs;

public abstract class TestFactoryDTOBase<TEntity, TDTO>
{
    public abstract TDTO CreateDTO(TEntity entity);
    public List<TDTO> CreateDTOs(IEnumerable<TEntity> entity) => entity.Select(CreateDTO).ToList();
}