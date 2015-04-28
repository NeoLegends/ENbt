using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENbt
{
    [TagHandlerFor(TagType.TimeSpan)]
    public class TimeSpanTag : ValueTag<TimeSpan>, IEquatable<TimeSpanTag>
    {
        public TimeSpanTag() : base(TagType.TimeSpan) { }

        public TimeSpanTag(ENBtBinaryReader reader)
            : this(reader.ReadInt64())
        {
            Contract.Requires<ArgumentNullException>(reader != null);
        }

        public TimeSpanTag(long ticks) : this(TimeSpan.FromTicks(ticks)) { }

        public TimeSpanTag(TimeSpan value) : base(TagType.TimeSpan, value) { }

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

        protected override void WritePayloadTo(ENbtBinaryWriter writer)
        {
            writer.Write(this.Value.Ticks);
        }
    }
}
