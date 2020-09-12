using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.Extensions
{
    /// <summary>
    /// This static class provides an extension method for CollectionExtensions class in orrder to append a list of elements rather than a single element.
    /// </summary>
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> source, IEnumerable<T> newValues)
        {
            var arr = newValues.ToArray();

            foreach (var value in arr)
            {
                source.Add(value);
            }
        }
    }
}
