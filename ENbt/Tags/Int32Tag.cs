using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENbt
{
    [TagHandlerFor(TagType.Int32)]
    public class Int32Tag : ValueTag<Int32>, IEquatable<Int32Tag>
    {
        public Int32Tag() : base(TagType.Int32) { }

        public Int32Tag(ENBtBinaryReader reader)
            : this(reader.ReadInt32())
        {
            Contract.Requires<ArgumentNullException>(reader != null);
        }

        public Int32Tag(Int32 value) : base(TagType.Int32, value) { }

        public override bool Equals(Tag other)
        {
            Int32Tag tag = other as Int32Tag;
            return (tag != null) && this.Equals(tag);
        }

        public bool Equals(Int32Tag other)
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
    [TagHandlerFor(TagType.UInt32)]
    public class UInt32Tag : ValueTag<uint>, IEquatable<UInt32Tag>
    {
        public UInt32Tag() : base(TagType.UInt32) { }

        public UInt32Tag(ENBtBinaryReader reader)
            : this(reader.ReadUInt32())
        {
            Contract.Requires<ArgumentNullException>(reader != null);
        }

        public UInt32Tag(UInt32 value) : base(TagType.UInt32, value) { }

        public override bool Equals(Tag other)
        {
            UInt32Tag tag = other as UInt32Tag;
            return (tag != null) && this.Equals(tag);
        }

        public bool Equals(UInt32Tag other)
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
