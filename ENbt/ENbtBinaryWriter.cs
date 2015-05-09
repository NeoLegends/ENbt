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
    /// Represents a <see cref="BinaryWriter"/> that writes in ENbt-compatible endianness.
    /// </summary>
    public class ENbtBinaryWriter : IDisposable
    {
        public const int defaultBufferSize = 512;

        private static readonly UTF8Encoding encoding = new UTF8Encoding(false);

        private readonly byte[] buffer;

        private readonly Stream destination;

        private readonly Encoder encoder = encoding.GetEncoder();

        private readonly int maximumAllocatableCharacters;

        private readonly bool ownsDestinationStream;

        private readonly EndianBinaryWriter writer;

        /// <summary>
        /// Initializes a new <see cref="ENbtBinaryWriter"/>.
        /// </summary>
        /// <param name="destination">The <see cref="Stream"/> to write to.</param>
        public ENbtBinaryWriter(Stream destination)
            : this(destination, false)
        {
            Contract.Requires<ArgumentNullException>(destination != null);
            Contract.Requires<ArgumentException>(destination.CanWrite);
        }

        /// <summary>
        /// Initializes a new <see cref="ENbtBinaryWriter"/>.
        /// </summary>
        /// <param name="destination">The <see cref="Stream"/> to write to.</param>
        /// <param name="leaveOpen">Indicates whether the underlying <see cref="Stream"/> shall be left open on disposal.</param>
        public ENbtBinaryWriter(Stream destination, bool leaveOpen)
            : this(destination, defaultBufferSize, leaveOpen)
        {
            Contract.Requires<ArgumentNullException>(destination != null);
            Contract.Requires<ArgumentException>(destination.CanWrite);
        }

        /// <summary>
        /// Initializes a new <see cref="ENbtBinaryWriter"/>.
        /// </summary>
        /// <param name="destination">The <see cref="Stream"/> to write to.</param>
        /// <param name="bufferSize">
        /// The size of the buffer used to write <see cref="Strings"/> into the <paramref name="destination"/>. If lots of
        /// long strings will be written, a larger value may be chosen. Defaults to 512.
        /// </param>
        /// <param name="leaveOpen">Indicates whether the underlying <see cref="Stream"/> shall be left open on disposal.</param>
        public ENbtBinaryWriter(Stream destination, int bufferSize, bool leaveOpen)
        {
            Contract.Requires<ArgumentNullException>(destination != null);
            Contract.Requires<ArgumentException>(destination.CanWrite);
            Contract.Requires<ArgumentOutOfRangeException>(bufferSize > 0);

            this.buffer = new byte[Math.Max(bufferSize, 32)];
            this.destination = destination;
            this.maximumAllocatableCharacters = this.buffer.Length / 4; // UTF-8 encoded characters take max. 4 bytes.
            this.ownsDestinationStream = !leaveOpen;
            this.writer = new EndianBinaryWriter(EndianBitConverter.Little, destination, encoding);
        }

        /// <summary>
        /// Finalizes the <see cref="ENbtBinaryWriter"/>.
        /// </summary>
        ~ENbtBinaryWriter()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Finalizes the <see cref="ENbtBinaryWriter"/>.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Writes the specified 8-bit signed integer.
        /// </summary>
        /// <param name="value">The value to write.</param>
        [CLSCompliant(false)]
        public void Write(sbyte value)
        {
            this.writer.Write(value);
        }

        /// <summary>
        /// Writes the specified 8-bit unsigned integer.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void Write(byte value)
        {
            this.writer.Write(value);
        }

        /// <summary>
        /// Writes the specified 16-bit unsigned integer.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void Write(short value)
        {
            this.writer.Write(value);
        }

        /// <summary>
        /// Writes the specified 16-bit unsigned integer.
        /// </summary>
        /// <param name="value">The value to write.</param>
        [CLSCompliant(false)]
        public void Write(ushort value)
        {
            this.writer.Write(value);
        }

        /// <summary>
        /// Writes the specified 32-bit unsigned integer.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void Write(int value)
        {
            this.writer.Write(value);
        }

        /// <summary>
        /// Writes the specified 32-bit unsigned integer.
        /// </summary>
        /// <param name="value">The value to write.</param>
        [CLSCompliant(false)]
        public void Write(uint value)
        {
            this.writer.Write(value);
        }

        /// <summary>
        /// Writes the specified 64-bit unsigned integer.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void Write(long value)
        {
            this.writer.Write(value);
        }

        /// <summary>
        /// Writes the specified 64-bit unsigned integer.
        /// </summary>
        /// <param name="value">The value to write.</param>
        [CLSCompliant(false)]
        public void Write(ulong value)
        {
            this.writer.Write(value);
        }

        /// <summary>
        /// Writes the specified IEEE 754 single accuracy floating point number.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void Write(float value)
        {
            this.writer.Write(value);
        }

        /// <summary>
        /// Writes the specified IEEE 754 double accuracy floating point number.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void Write(double value)
        {
            this.writer.Write(value);
        }

        /// <summary>
        /// Writes the specified <see cref="TagType"/>.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void Write(TagType type)
        {
            this.writer.Write((byte)type);
        }

        /// <summary>
        /// Writes the specified <see cref="String"/> as 32-bit length prefixed UTF-8 encoded data.
        /// </summary>
        /// <param name="value">The value to write.</param>
        [ContractVerification(false)]
        public unsafe void Write(string value)
        {
            if (value != null && value.Length > 0)
            {
                int totalByteCount = encoding.GetByteCount(value);
                this.writer.Write(totalByteCount);

                // Try very aggressively not to allocate new objects for performance reasons.
                // First check if we can write the string into the buffer entirely, otherwise
                // write chunk by chunk.
                fixed (char* pValue = value)
                fixed (byte* pBuf = this.buffer)
                {
                    if (totalByteCount <= this.buffer.Length)
                    {
                        encoder.GetBytes(pValue, value.Length, pBuf, this.buffer.Length, true);
                        this.writer.Write(this.buffer, 0, totalByteCount);
                    }
                    else
                    {
                        int charStart = 0;
                        do
                        {
                            int charsLeft = value.Length - charStart;
                            int charsToWrite = (charsLeft <= maximumAllocatableCharacters) ? charsLeft : maximumAllocatableCharacters;
                            int bytesToWrite = encoder.GetBytes(pValue + charStart, charsToWrite, pBuf, this.buffer.Length, charsLeft == 0);

                            if (bytesToWrite > 0)
                            {
                                this.writer.Write(this.buffer, 0, bytesToWrite);
                            }

                            charStart += charsToWrite;
                        } while (charStart < value.Length);
                    }
                }
            }
            else
            {
                this.Write(0);
            }
        }

        /// <summary>
        /// Writes the specified byte-array.
        /// </summary>
        /// <param name="values">The values to write.</param>
        public void Write(byte[] values)
        {
            Contract.Requires<ArgumentNullException>(values != null);

            this.Write(values, 0, values.Length);
        }

        /// <summary>
        /// Writes the specified byte-array.
        /// </summary>
        /// <param name="count">The amount of bytes to write.</param>
        /// <param name="offset">The offset to start writing at.</param>
        /// <param name="values">The values to write.</param>
        public void Write(byte[] values, int offset, int count)
        {
            Contract.Requires<ArgumentNullException>(values != null);
            Contract.Requires<ArgumentOutOfRangeException>(count >= 0);
            Contract.Requires<ArgumentOutOfRangeException>((offset + count) <= values.Length);

            this.writer.Write(values, offset, count);
        }

        /// <summary>
        /// Disposes the <see cref="ENbtBinaryWriter"/>.
        /// </summary>
        /// <param name="disposing">Indicates whether to dispose managed resources as well.</param>
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                this.writer.Flush();
                if (this.ownsDestinationStream)
                {
                    this.destination.Dispose();
                }
            }
            finally
            {
                GC.SuppressFinalize(this);
            }
        }
    }
}
