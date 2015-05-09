using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ENbt
{
    internal static class Extensions
    {
        [Pure]
        public static IEnumerable<Type> GetTypesByAttribute<T>(this Assembly assembly, bool includeNonPublic, Func<T, bool> attributeFilter = null)
            where T : Attribute
        {
            Contract.Requires<ArgumentNullException>(assembly != null);
            Contract.Ensures(Contract.Result<IEnumerable<Type>>() != null);

            return GetTypesByAttribute(assembly, typeof(T), includeNonPublic, attr => attributeFilter((T)attr));
        }

        [Pure]
        public static IEnumerable<Type> GetTypesByAttribute(this Assembly assembly, Type attributeType, bool includeNonPublic, Func<Attribute, bool> attributeFilter = null)
        {
            Contract.Requires<ArgumentNullException>(assembly != null);
            Contract.Requires<ArgumentNullException>(attributeType != null);
            Contract.Requires<ArgumentException>(typeof(Attribute).IsAssignableFrom(attributeType));
            Contract.Ensures(Contract.Result<IEnumerable<Type>>() != null);

            return from type in (includeNonPublic ? assembly.GetTypes() : assembly.GetExportedTypes())
                   where type.GetCustomAttributes(attributeType).Any(attribute => (attributeFilter == null) || attributeFilter(attribute))
                   select type;
        }

        public static int ReadExactly(this Stream s, byte[] buffer, int offset, int count)
        {
            Contract.Requires<ArgumentNullException>(s != null);
            Contract.Requires<ArgumentException>(s.CanRead);
            Contract.Requires<ArgumentNullException>(buffer != null);
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(count >= 0);
            Contract.Requires<ArgumentOutOfRangeException>((offset + count) <= buffer.Length);
            Contract.Ensures(Contract.Result<int>() >= 0);

            int totalRead = 0;
            while (totalRead < count)
            {
                int read = s.Read(buffer, totalRead + offset, count - totalRead);
                if (read == 0)
                {
                    throw new EndOfStreamException();
                }
                totalRead += read;
            }

            return count;
        }
    }
}
