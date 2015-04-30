using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENbt
{
    internal static class Extensions
    {
        public static int ReadExactly(this Stream s, byte[] buffer, int offset, int count)
        {
            Contract.Requires<ArgumentNullException>(s != null);
            Contract.Requires<ArgumentNullException>(buffer != null);
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(count >= 0);
            Contract.Requires<ArgumentOutOfRangeException>((offset + count) <= buffer.Length);

            int totalRead = 0;
            while (totalRead < count)
            {
                int read = s.Read(buffer, totalRead + offset, count - totalRead);
                if (read == 0)
                {
                    throw new EndOfStreamException("The end of the stream was reached.");
                }
                totalRead += read;
            }

            return count;
        }
    }
}
