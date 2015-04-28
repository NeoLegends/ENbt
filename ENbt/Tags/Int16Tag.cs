using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENbt
{
    [TagHandlerFor(TagType.Int16)]
    public class Int16Tag : ValueTag<short>, IComparable<Int16Tag>, IEquatable<Int16Tag>
    {
        public Int16Tag() : base(TagType.Int16) { }

        public Int16Tag(ENBtBinaryReader reader)
            : this(reader.ReadInt16())
        {
            Contract.Requires<ArgumentNullException>(reader != null);
        }

        public Int16Tag(Int16 value) : base(TagType.Int16, value) { }

        public int CompareTo(Int16Tag other)
        {
            if (other == null)
            {
                return -1;
            }
            return this.Value.CompareTo(other.Value);
        }

        public override bool Equals(Tag other)
        {
            Int16Tag tag = other as Int16Tag;
            return (tag != null) && this.Equals(tag);
        }

        public bool Equals(Int16Tag other)
        {
            if (ReferenceEquals(other, this))
                return true;
            if (ReferenceEquals(other, null))
                return false;

            return (this.Value == other.Value);
        }

        protected override void WritePayloadTo(ENbtBinaryWriter writer)
        {
            writer.Write(this.Value);
        }
    }

    [CLSCompliant(false)]
    [TagHandlerFor(TagType.UInt16)]
    public class UInt16Tag : ValueTag<ushort>, IComparable<UInt16Tag>, IEquatable<UInt16Tag>
    {
        public UInt16Tag() : base(TagType.UInt16) { }

        public UInt16Tag(ENBtBinaryReader reader)
            : this(reader.ReadUInt16())
        {
            Contract.Requires<ArgumentNullException>(reader != null);
        }

        public UInt16Tag(UInt16 value) : base(TagType.UInt16, value) { }

        public int CompareTo(UInt16Tag other)
        {
            if (other == null)
            {
                return -1;
            }
            return this.Value.CompareTo(other.Value);
        }

        public override bool Equals(Tag other)
        {
            UInt16Tag tag = other as UInt16Tag;
            return (tag != null) && this.Equals(tag);
        }

        public bool Equals(UInt16Tag other)
        {
            if (ReferenceEquals(other, this))
                return true;
            if (ReferenceEquals(other, null))
                return false;

            return (this.Value == other.Value);
        }

        protected override void WritePayloadTo(ENbtBinaryWriter writer)
        {
            writer.Write(this.Value);
        }
    }
}
