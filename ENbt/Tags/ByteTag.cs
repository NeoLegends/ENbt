using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ENbt
{
    [CLSCompliant(false)]
    [TagHandlerFor(TagType.SByte)]
    public class SByteTag : ValueTag<sbyte>, IComparable<SByteTag>, IEquatable<SByteTag>
    {
        public override TagType Type
        {
            get
            {
                return TagType.SByte;
            }
        }

        public SByteTag() { }

        public SByteTag(ENbtBinaryReader reader)
            : this(reader.ReadSByte())
        {
            Contract.Requires<ArgumentNullException>(reader != null);
        }

        public SByteTag(sbyte value) : base(value) { }

        public int CompareTo(SByteTag other)
        {
            if (other == null)
            {
                return -1;
            }
            return this.Value.CompareTo(other.Value);
        }

        public override bool Equals(Tag other)
        {
            SByteTag tag = other as SByteTag;
            return (tag != null) && this.Equals(tag);
        }

        public bool Equals(SByteTag other)
        {
            if (ReferenceEquals(other, this))
                return true;
            if (ReferenceEquals(other, null))
                return false;

            return (this.Value == other.Value);
        }

        public override void WritePayloadTo(ENbtBinaryWriter writer)
        {
            writer.Write(this.Value);
        }
    }

    [TagHandlerFor(TagType.Byte)]
    public class ByteTag : ValueTag<byte>, IComparable<ByteTag>, IEquatable<ByteTag>
    {
        public override TagType Type
        {
            get
            {
                return TagType.Byte;
            }
        }

        public ByteTag() { }

        public ByteTag(ENbtBinaryReader reader)
            : this(reader.ReadByte())
        {
            Contract.Requires<ArgumentNullException>(reader != null);
        }

        public ByteTag(byte value) : base(value) { }

        public int CompareTo(ByteTag other)
        {
            if (other == null)
            {
                return -1;
            }
            return this.Value.CompareTo(other.Value);
        }

        public override bool Equals(Tag other)
        {
            ByteTag tag = other as ByteTag;
            return (tag != null) && this.Equals(tag);
        }

        public bool Equals(ByteTag other)
        {
            if (ReferenceEquals(other, this))
                return true;
            if (ReferenceEquals(other, null))
                return false;

            return (this.Value == other.Value);
        }

        public override void WritePayloadTo(ENbtBinaryWriter writer)
        {
            writer.Write(this.Value);
        }
    }
}
