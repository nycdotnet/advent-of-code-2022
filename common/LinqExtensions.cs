namespace common
{
    public static class LinqExtensions
    {
        /// <summary>
        /// Partitions an <see cref="IEnumerable{T}"/> into zero or more <see cref="List{T}"/>,
        /// splitting the <paramref name="source"/> on elements which when processed with
        /// <paramref name="splitOn"/> return true.  Does not include those elements.
        /// </summary>
        public static IEnumerable<List<T>> SelectPartition<T>(
            this IEnumerable<T> source,
            Func<T, bool> splitOn)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));
            ArgumentNullException.ThrowIfNull(splitOn, nameof(splitOn));

            bool shouldYield = false;
            var buffer = new List<T>();
            foreach (var item in source)
            {
                if (splitOn(item))
                {
                    yield return buffer;
                    buffer = new List<T>();
                }
                else
                {
                    shouldYield = true;
                    buffer.Add(item);
                }
            }
            if (shouldYield)
            {
                yield return buffer;
            }
        }
    }
}
