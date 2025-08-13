namespace Application.Mapping;

public interface IMapperExtended
{
    public List<TDestination> UpdateCollection<TSource, TDestination>(
        IEnumerable<TSource> source,
        IEnumerable<TDestination> destination)
        where TSource : class
        where TDestination : class;
}