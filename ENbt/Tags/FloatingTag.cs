using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENbt
{
    [TagHandlerFor(TagType.Single)]
    public class SingleTag : ValueTag<Single>, IComparable<SingleTag>, IEquatable<SingleTag>
    {
        public override TagType Type
        {
            get
            {
                return TagType.Single;
            }
        }

        public SingleTag() { }

        public SingleTag(ENbtBinaryReader reader)
            : this(reader.ReadSingle())
        {
            Contract.Requires<ArgumentNullException>(reader != null);
        }

        public SingleTag(Single value) : base(value) { }

        public int CompareTo(SingleTag other)
        {
            if (other == null)
            {
                return -1;
            }
            return this.Value.CompareTo(other.Value);
        }

        public override bool Equals(Tag other)
        {
            SingleTag tag = other as SingleTag;
            return (tag != null) && this.Equals(tag);
        }

        public bool Equals(SingleTag other)
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

    [TagHandlerFor(TagType.Double)]
    public class DoubleTag : ValueTag<Double>, IComparable<DoubleTag>, IEquatable<DoubleTag>
    {
        public override TagType Type
        {
            get
            {
                return TagType.Double;
            }
        }

        public DoubleTag() { }

        public DoubleTag(ENbtBinaryReader reader)
            : this(reader.ReadDouble())
        {
            Contract.Requires<ArgumentNullException>(reader != null);
        }

        public DoubleTag(Double value) : base(value) { }

        public int CompareTo(DoubleTag other)
        {
            if (other == null)
            {
                return -1;
            }
            return this.Value.CompareTo(other.Value);
        }

        public override bool Equals(Tag other)
        {
            DoubleTag tag = other as DoubleTag;
            return (tag != null) && this.Equals(tag);
        }

        public bool Equals(DoubleTag other)
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
