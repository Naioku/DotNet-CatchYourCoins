using AutoMapper;

namespace Application.Mapping;

public class MapperExtended(IMapper mapper) : IMapperExtended
{
    public List<TDestination> UpdateCollection<TSource, TDestination>(IEnumerable<TSource> source, IEnumerable<TDestination> destination) where TSource : class where TDestination : class
    {
        List<TSource> sourceList = source.ToList();
        List<TDestination> destList = destination.ToList();
            
        if (sourceList.Count != destList.Count)
        {
            throw new ArgumentException("Source and destination collections must have the same count");
        }
            
        for (int i = 0; i < sourceList.Count; i++)
        {
            mapper.Map(sourceList[i], destList[i]);
        }

        return destList; // I think it is not the correct approach to update collection and return it,
                         // but currently I can't figure out how to make it work well with unit test and Moq.
    }
}