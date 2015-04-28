using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENbt
{
    public class ENBtBinaryReader : IDisposable
    {
        private static readonly UTF8Encoding encoding = new UTF8Encoding(false);

        private readonly bool ownsSource;

        private readonly EndianBinaryReader reader;

        private readonly Stream source;

        public ENBtBinaryReader(Stream source)
            : this(source, true)
        {
            Contract.Requires<ArgumentNullException>(source != null);
            Contract.Requires<ArgumentException>(source.CanRead);
        }

        public ENBtBinaryReader(Stream source, bool ownsSource)
        {
            Contract.Requires<ArgumentNullException>(source != null);
            Contract.Requires<ArgumentException>(source.CanRead);

            this.ownsSource = ownsSource;
            this.reader = new EndianBinaryReader(EndianBitConverter.Little, source);
            this.source = source;
        }

        ~ENBtBinaryReader()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        [CLSCompliant(false)]
        public sbyte ReadSByte()
        {
            return this.reader.ReadSByte();
        }

        public byte ReadByte()
        {
            return this.reader.ReadByte();
        }

        public short ReadInt16()
        {
            return this.reader.ReadInt16();
        }

        [CLSCompliant(false)]
        public ushort ReadUInt16()
        {
            return this.reader.ReadUInt16();
        }

        public int ReadInt32()
        {
            return this.reader.ReadInt32();
        }

        [CLSCompliant(false)]
        public uint ReadUInt32()
        {
            return this.reader.ReadUInt32();
        }

        public long ReadInt64()
        {
            return this.reader.ReadInt64();
        }

        [CLSCompliant(false)]
        public ulong ReadUInt64()
        {
            return this.reader.ReadUInt64();
        }

        public float ReadSingle()
        {
            return this.reader.ReadSingle();
        }

        public double ReadDouble()
        {
            return this.reader.ReadDouble();
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            Contract.Requires<ArgumentNullException>(buffer != null);
            Contract.Requires<ArgumentOutOfRangeException>((offset + count) <= buffer.Length);

            return this.reader.Read(buffer, offset, count);
        }

        public string ReadString()
        {
            int length = this.ReadInt32();
            if (length < 0)
            {
                throw new InvalidOperationException(string.Format("Negative string length ({0}) given!", length));
            }

            byte[] buffer = new byte[length];
            int bytesRead = this.Read(buffer, 0, buffer.Length);
            if (bytesRead < buffer.Length)
            {
                throw new EndOfStreamException("The end of the stream was reached before the string could be read entirely.");
            }

            return encoding.GetString(buffer);
        }

        public TagType ReadTagType()
        {
            return (TagType)this.ReadByte();
        }

        public void Seek(int offset, SeekOrigin origin)
        {
            this.reader.Seek(offset, origin);
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (this.ownsSource)
                {
                    this.source.Dispose();
                }
            }
            finally
            {
                GC.SuppressFinalize(this);
            }
        }
    }
}
