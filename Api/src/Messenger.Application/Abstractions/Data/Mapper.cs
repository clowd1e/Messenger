namespace Messenger.Application.Abstractions.Data
{
    public abstract class Mapper<TSource, TDestination>
    {
        public abstract TDestination Map(TSource source);

        public virtual IEnumerable<TDestination> Map(IEnumerable<TSource> source)
            => source.Select(Map);
    }
}
