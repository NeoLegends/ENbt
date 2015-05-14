using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENbt
{
    [TagHandlerFor(TagType.TimeSpan)]
    public class TimeSpanTag : ValueTag<TimeSpan>, IComparable<TimeSpanTag>, IEquatable<TimeSpanTag>
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
                return TagType.TimeSpan;
            }
        }

        public TimeSpanTag() { }

        public TimeSpanTag(ENbtBinaryReader reader)
            : this(reader.ReadInt64())
        {
            Contract.Requires<ArgumentNullException>(reader != null);
        }

        public TimeSpanTag(long ticks) : this(TimeSpan.FromTicks(ticks)) { }

        public TimeSpanTag(TimeSpan value) : base(value) { }

        public int CompareTo(TimeSpanTag other)
        {
            if (other == null)
            {
                return -1;
            }

            return this.Value.CompareTo(other.Value);
        }

        public override bool Equals(Tag other)
        {
            TimeSpanTag tag = other as TimeSpanTag;
            return (tag != null) && this.Equals(tag);
        }

        public bool Equals(TimeSpanTag other)
        {
            if (ReferenceEquals(other, this))
                return true;
            if (ReferenceEquals(other, null))
                return false;

            return (this.Value == other.Value);
        }

        public override void WritePayloadTo(ENbtBinaryWriter writer)
        {
            writer.Write(this.Value.Ticks);
        }
    }
}
