namespace Common.Extensions;

public static class LinqExtension
{
    public static ulong Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, ulong> summer)
    {
        return source.Aggregate<TSource?, ulong>(0, (current, item) => current + summer(item));
    }
}