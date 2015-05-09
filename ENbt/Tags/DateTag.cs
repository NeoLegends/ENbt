using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ENbt
{
    [TagHandlerFor(TagType.Date)]
    public class DateTag : ValueTag<DateTime>, IComparable<DateTag>, IEquatable<DateTag>
    {
        public override int PayloadLength
        {
            get
            {
                return sizeof(long);
            }
        }

        public override TagType Type
        {
            get
            {
                return TagType.Date;
            }
        }

        public DateTag() { }

        public DateTag(ENbtBinaryReader reader)
            : this(reader.ReadInt64())
        {
            Contract.Requires<ArgumentNullException>(reader != null);
        }

        public DateTag(long unixTimeMs) : this(unixTimeMs.FromUnixTimeMilliseconds()) { }

        public DateTag(DateTime value) : base(value) { }

        public int CompareTo(DateTag other)
        {
            if (other == null)
            {
                return -1;
            }
            return this.Value.CompareTo(other.Value);
        }

        public override bool Equals(Tag other)
        {
            DateTag tag = other as DateTag;
            return (tag != null) && this.Equals(tag);
        }

        public bool Equals(DateTag other)
        {
            if (ReferenceEquals(other, this))
                return true;
            if (ReferenceEquals(other, null))
                return false;

            return (this.Value == other.Value);
        }

        protected override void WritePayloadTo(ENbtBinaryWriter writer)
        {
            writer.Write(this.Value.ToUnixTimeMilliseconds());
        }
    }
}
