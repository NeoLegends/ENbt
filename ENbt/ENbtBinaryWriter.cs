using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENbt
{
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

        public ENbtBinaryWriter(Stream destination)
            : this(destination, true)
        {
            Contract.Requires<ArgumentNullException>(destination != null);
            Contract.Requires<ArgumentException>(destination.CanWrite);
        }

        public ENbtBinaryWriter(Stream destination, bool ownsDestination)
            : this(destination, defaultBufferSize, ownsDestination)
        {
            Contract.Requires<ArgumentNullException>(destination != null);
            Contract.Requires<ArgumentException>(destination.CanWrite);
        }

        public ENbtBinaryWriter(Stream destination, int bufferSize, bool ownsDestination)
        {
            Contract.Requires<ArgumentNullException>(destination != null);
            Contract.Requires<ArgumentException>(destination.CanWrite);
            Contract.Requires<ArgumentOutOfRangeException>(bufferSize > 0);

            this.buffer = new byte[Math.Min(bufferSize, 32)];
            this.destination = destination;
            this.maximumAllocatableCharacters = this.buffer.Length / 4;
            this.ownsDestinationStream = ownsDestination;
            this.writer = new EndianBinaryWriter(EndianBitConverter.Little, destination, encoding);
        }

        ~ENbtBinaryWriter()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        [CLSCompliant(false)]
        public void Write(sbyte value)
        {
            this.writer.Write(value);
        }

        public void Write(byte value)
        {
            this.writer.Write(value);
        }

        public void Write(byte[] values)
        {
            Contract.Requires<ArgumentNullException>(values != null);

            this.Write(values, 0, values.Length);
        }

        public void Write(byte[] values, int offset, int count)
        {
            Contract.Requires<ArgumentNullException>(values != null);
            Contract.Requires<ArgumentOutOfRangeException>(count >= 0);
            Contract.Requires<ArgumentOutOfRangeException>((offset + count) <= values.Length);

            this.writer.Write(values, offset, count);
        }

        public void Write(short value)
        {
            this.writer.Write(value);
        }

        [CLSCompliant(false)]
        public void Write(ushort value)
        {
            this.writer.Write(value);
        }

        public void Write(int value)
        {
            this.writer.Write(value);
        }

        [CLSCompliant(false)]
        public void Write(uint value)
        {
            this.writer.Write(value);
        }

        public void Write(long value)
        {
            this.writer.Write(value);
        }

        [CLSCompliant(false)]
        public void Write(ulong value)
        {
            this.writer.Write(value);
        }

        public void Write(float value)
        {
            this.writer.Write(value);
        }

        public void Write(double value)
        {
            this.writer.Write(value);
        }

        public void Write(TagType type)
        {
            this.writer.Write((byte)type);
        }

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
                fixed (char* pChars = value)
                fixed (byte* pBuf = this.buffer)
                {
                    if (totalByteCount <= this.buffer.Length)
                    {
                        encoder.GetBytes(pChars, value.Length, pBuf, this.buffer.Length, true);
                        this.writer.Write(this.buffer, 0, totalByteCount);
                    }
                    else
                    {
                        int charStart = 0;
                        do
                        {
                            int charsToWrite = ((value.Length - charStart) <= maximumAllocatableCharacters) ? value.Length : maximumAllocatableCharacters;
                            int bytesToWrite = encoder.GetBytes(pChars + charStart, charsToWrite, pBuf, this.buffer.Length, false);

                            if (bytesToWrite > 0)
                            {
                                this.writer.Write(this.buffer, 0, bytesToWrite);
                            }

                            charStart += charsToWrite;
                        } while (charStart < value.Length);
                        this.encoder.Reset();
                    }
                }
            }
            else
            {
                this.Write(0);
            }
        }

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
