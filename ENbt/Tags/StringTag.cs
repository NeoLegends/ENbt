using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENbt
{
    [TagHandlerFor(TagType.String)]
    public class StringTag : ValueTag<string>, IComparable<StringTag>, IEquatable<StringTag>
    {
        private static readonly StringTag _Empty = new StringTag(string.Empty);

        public static StringTag Empty
        {
            get
            {
                return _Empty;
            }
        }

        public override int PayloadLength
        {
            get
            {
                return sizeof(int) + Encoding.UTF8.GetByteCount(this.Value);
            }
        }

        public override TagType Type
        {
            get
            {
                return TagType.String;
            }
        }

        public StringTag() { }

        public StringTag(ENbtBinaryReader reader)
            : this(reader.ReadString())
        {
            Contract.Requires<ArgumentNullException>(reader != null);
        }

        public StringTag(string value) : base(value) { }

        public int CompareTo(StringTag other)
        {
            if (other == null)
            {
                return -1;
            }
            return this.Value.CompareTo(other.Value);
        }

        public override bool Equals(Tag other)
        {
            StringTag tag = other as StringTag;
            return (tag != null) && this.Equals(tag);
        }

        public bool Equals(StringTag other)
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

        public static implicit operator StringTag(string value)
        {
            if (value == string.Empty) // Special case to avoid allocation
            {
                return StringTag.Empty;
            }

            return (value != null) ? new StringTag(value) : null;
        }
    }
}
