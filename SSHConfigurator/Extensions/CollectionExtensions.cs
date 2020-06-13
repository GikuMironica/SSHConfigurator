using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.Extensions
{
    public class CollectionExtensions
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
