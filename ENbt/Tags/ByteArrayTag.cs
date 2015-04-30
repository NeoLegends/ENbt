using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ENbt
{
    [TagHandlerFor(TagType.ByteArray)]
    public class ByteArrayTag : ValueTag<byte[]>, IEquatable<ByteArrayTag>, IReadOnlyList<byte>
    {
        int IReadOnlyCollection<byte>.Count
        {
            get
            {
                return this.Length;
            }
        }

        public override int Length
        {
            [ContractVerification(false)]
            get
            {
                byte[] value = this.Value;
                return (value != null) ? value.Length : 0;
            }
        }

        public override int PayloadLength
        {
            get
            {
                return sizeof(int) + this.Length;
            }
        }

        public override byte[] Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                if ((value != null) && (value.LongLength > int.MaxValue))
                {
                    throw new OverflowException("Length of the array may only be int.MaxValue.");
                }

                base.Value = value;
            }
        }

        public byte this[int index]
        {
            [ContractVerification(false)]
            get
            {
                byte[] values = this.Value;
                if (values == null)
                {
                    throw new ArgumentOutOfRangeException("The array has not been set, cannot access the desired element.");
                }

                return values[index];
            }
            [ContractVerification(false)]
            set
            {
                byte[] values = this.Value;
                if (values == null)
                {
                    throw new ArgumentOutOfRangeException("The array has not been set, cannot access the desired element.");
                }

                values[index] = value;
            }
        }

        public ByteArrayTag() : base(TagType.ByteArray) { }

        public ByteArrayTag(byte[] value)
            : base(TagType.ByteArray, value) 
        {
            Contract.Requires<OverflowException>(value == null || value.LongLength <= int.MaxValue);
        }

        public ByteArrayTag(ENBtBinaryReader reader)
            : this()
        {
            Contract.Requires<ArgumentNullException>(reader != null);

            int length = reader.ReadInt32();
            if (length < 0)
            {
                throw new InvalidOperationException(string.Format("Negative array length ({0}) given!", length));
            }
            this.Value = new byte[length];
            reader.ReadExactly(this.Value, 0, length);
        }

        public override bool Equals(Tag other)
        {
            return this.Equals(other as ByteArrayTag);
        }

        public bool Equals(ByteArrayTag other)
        {
            if (ReferenceEquals(other, this))
                return true;
            if (ReferenceEquals(other, null))
                return false;

            return ReferenceEquals(this.Value, other.Value) || this.Value.SequenceEqual(other.Value);
        }

        [ContractVerification(false)]
        public IEnumerator<byte> GetEnumerator()
        {
            return (this.Value ?? Enumerable.Empty<byte>()).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public override int GetHashCode()
        {
            return Hashing.GetCollectionHash(this.Value);
        }

        protected override void WritePayloadTo(ENbtBinaryWriter writer)
        {
            writer.Write(this.Value.Length);
            writer.Write(this.Value);
        }
    }
}
