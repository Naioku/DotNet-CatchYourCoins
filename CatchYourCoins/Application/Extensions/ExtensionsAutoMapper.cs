using AutoMapper;

namespace Application.Extensions;

public static class ExtensionsAutoMapper
{
    public static void MapEnumerable<TSource, TDestination>(
        this IMapper mapper,
        IEnumerable<TSource> source,
        IEnumerable<TDestination> destination) 
        where TSource : class 
        where TDestination : class
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
    }
}