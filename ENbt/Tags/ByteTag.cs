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
    public class SByteTag : ValueTag<sbyte>, IEquatable<SByteTag>
    {
        public SByteTag() : base(TagType.SByte) { }

        public SByteTag(ENBtBinaryReader reader)
            : this(reader.ReadSByte())
        {
            Contract.Requires<ArgumentNullException>(reader != null);
        }

        public SByteTag(sbyte value) : base(TagType.SByte, value) { }

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

        protected override void WritePayloadTo(ENbtBinaryWriter writer)
        {
            writer.Write(this.Value);
        }
    }

    [TagHandlerFor(TagType.Byte)]
    public class ByteTag : ValueTag<byte>, IEquatable<ByteTag>
    {
        public ByteTag() : base(TagType.Byte) { }

        public ByteTag(ENBtBinaryReader reader)
            : this(reader.ReadByte())
        {
            Contract.Requires<ArgumentNullException>(reader != null);
        }

        public ByteTag(byte value) : base(TagType.Byte,  value) { }

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

        protected override void WritePayloadTo(ENbtBinaryWriter writer)
        {
            writer.Write(this.Value);
        }
    }
}
