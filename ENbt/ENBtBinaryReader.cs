using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENbt
{
    /// <summary>
    /// Represents a <see cref="BinaryReader"/> that reads in ENbt-compatible endianness.
    /// </summary>
    public class ENbtBinaryReader : IDisposable
    {
        private static readonly UTF8Encoding encoding = new UTF8Encoding(false);

        private readonly bool ownsSource;

        private readonly EndianBinaryReader reader;

        private readonly Stream source;
        
        /// <summary>
        /// Initializes a new <see cref="ENbtBinaryReader"/> from the specified <see cref="Stream"/>. See remarks.
        /// </summary>
        /// <remarks>Automatically disposes the <paramref name="source"/> on disposal.</remarks>
        /// <param name="source">The <see cref="Stream"/> to read from.</param>
        public ENbtBinaryReader(Stream source)
            : this(source, false)
        {
            Contract.Requires<ArgumentNullException>(source != null);
            Contract.Requires<ArgumentException>(source.CanRead);
        }

        /// <summary>
        /// Initializes a new <see cref="ENbtBinaryReader"/> from the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="source">The <see cref="Stream"/> to read from.</param>
        /// <param name="leaveOpen">Indicates whether the <paramref name="source"/> shall be left open on disposal of this class.</param>
        public ENbtBinaryReader(Stream source, bool leaveOpen)
        {
            Contract.Requires<ArgumentNullException>(source != null);
            Contract.Requires<ArgumentException>(source.CanRead);

            this.ownsSource = !leaveOpen;
            this.reader = new EndianBinaryReader(EndianBitConverter.Little, source);
            this.source = source;
        }

        /// <summary>
        /// Finalizes the <see cref="ENbtBinaryReader"/>.
        /// </summary>
        ~ENbtBinaryReader()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Finalizes the <see cref="ENbtBinaryReader"/>.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Reads an 8-bit signed integer.
        /// </summary>
        /// <returns>The read value.</returns>
        [CLSCompliant(false)]
        public sbyte ReadSByte()
        {
            return this.reader.ReadSByte();
        }

        /// <summary>
        /// Reads an 8-bit unsigned integer.
        /// </summary>
        /// <returns>The read value.</returns>
        public byte ReadByte()
        {
            return this.reader.ReadByte();
        }

        /// <summary>
        /// Reads a 16-bit signed integer.
        /// </summary>
        /// <returns>The read value.</returns>
        public short ReadInt16()
        {
            return this.reader.ReadInt16();
        }

        /// <summary>
        /// Reads a 16-bit unsigned integer.
        /// </summary>
        /// <returns>The read value.</returns>
        [CLSCompliant(false)]
        public ushort ReadUInt16()
        {
            return this.reader.ReadUInt16();
        }

        /// <summary>
        /// Reads a 32-bit signed integer.
        /// </summary>
        /// <returns>The read value.</returns>
        public int ReadInt32()
        {
            return this.reader.ReadInt32();
        }

        /// <summary>
        /// Reads a 32-bit unsigned integer.
        /// </summary>
        /// <returns>The read value.</returns>
        [CLSCompliant(false)]
        public uint ReadUInt32()
        {
            return this.reader.ReadUInt32();
        }

        /// <summary>
        /// Reads a 64-bit signed integer.
        /// </summary>
        /// <returns>The read value.</returns>
        public long ReadInt64()
        {
            return this.reader.ReadInt64();
        }

        /// <summary>
        /// Reads a 64-bit unsigned integer.
        /// </summary>
        /// <returns>The read value.</returns>
        [CLSCompliant(false)]
        public ulong ReadUInt64()
        {
            return this.reader.ReadUInt64();
        }

        /// <summary>
        /// Reads an IEEE single accuracy float.
        /// </summary>
        /// <returns>The read value.</returns>
        public float ReadSingle()
        {
            return this.reader.ReadSingle();
        }

        /// <summary>
        /// Reads an IEEE double accuracy float.
        /// </summary>
        /// <returns>The read value.</returns>
        public double ReadDouble()
        {
            return this.reader.ReadDouble();
        }

        /// <summary>
        /// Reads the specified amount of bytes into the <paramref name="buffer"/> and returns how many bytes were actually read.
        /// </summary>
        /// <param name="buffer">The buffer to read the data into.</param>
        /// <param name="offset">The offset to start reading at.</param>
        /// <param name="count">The amount of bytes to read.</param>
        /// <returns>The amount of bytes actually read.</returns>
        public int Read(byte[] buffer, int offset, int count)
        {
            Contract.Requires<ArgumentNullException>(buffer != null);
            Contract.Requires<ArgumentOutOfRangeException>((offset + count) <= buffer.Length);

            return this.reader.Read(buffer, offset, count);
        }

        /// <summary>
        /// Reads exactly <paramref name="count"/> bytes into the <paramref name="buffer"/>. In case there are not enough bytes
        /// in the source to read <paramref name="count"/> of them, an <see cref="EndOfStreamException"/> will be thrown.
        /// </summary>
        /// <param name="buffer">The buffer to read the data into.</param>
        /// <param name="offset">The offset to start reading at.</param>
        /// <param name="count">The amount of bytes to read.</param>
        /// <returns>The amount of bytes actually read. Since this method reads exactly <paramref name="count"/> bytes, it will always be <paramref name="count"/>.</returns>
        public int ReadExactly(byte[] buffer, int offset, int count)
        {
            Contract.Requires<ArgumentNullException>(buffer != null);
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(count >= 0);
            Contract.Requires<ArgumentOutOfRangeException>((offset + count) <= buffer.Length);

            return this.source.ReadExactly(buffer, offset, count);
        }

        /// <summary>
        /// Reads a 32-bit length prefixed, UTF-8 encoded <see cref="String"/>.
        /// </summary>
        /// <returns>The read value.</returns>
        public string ReadString()
        {
            int length = this.ReadInt32();
            if (length < 0)
            {
                throw new InvalidOperationException(string.Format("Negative string length ({0}) given!", length));
            }
            if (length == 0)
            {
                return string.Empty;
            }

            byte[] buffer = new byte[length]; // Will be collected in Gen0
            this.ReadExactly(buffer, 0, buffer.Length);
            return encoding.GetString(buffer);
        }

        /// <summary>
        /// Reads a <see cref="TagType"/>.
        /// </summary>
        /// <returns>The read value.</returns>
        public TagType ReadTagType()
        {
            return (TagType)this.ReadByte();
        }

        /// <summary>
        /// Repositions the internal pointer of the underlying source, if possible.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="origin">The <see cref="SeekOrigin"/>.</param>
        public void Seek(int offset, SeekOrigin origin)
        {
            this.reader.Seek(offset, origin);
        }

        /// <summary>
        /// Disposes the <see cref="ENbtBinaryReader"/>.
        /// </summary>
        /// <param name="disposing">Indicates whether to dispose managed resources as well.</param>
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
