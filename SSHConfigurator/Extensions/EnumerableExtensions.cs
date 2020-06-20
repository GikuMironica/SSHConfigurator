﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SSHConfigurator.Extensions
{
    public static class EnumerableExtensions
    {
        public static Collection<T> ToCollection<T>(this IEnumerable<T> source)
        {
            var arr = source.ToArray();

            return new Collection<T>(arr);
        }
    }
}
