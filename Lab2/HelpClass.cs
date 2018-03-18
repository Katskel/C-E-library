using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library;

namespace Lab2
{
    public static class HelpClass
    {
        public static ItemCollection<T> ToItemCollection<T>(this IEnumerable<T> source) where T : IEquatable<T>
        {
            ItemCollection<T> collection = new ItemCollection<T>();
            T[] array = source.ToArray();
            for (int i = 0; i < array.Count(); i++)
                collection.Add(array[i]);
            return collection;
        }
    }
}
