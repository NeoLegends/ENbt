﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENbt
{
    [TagHandlerFor(TagType.Single)]
    public class SingleTag : ValueTag<Single>, IEquatable<SingleTag>
    {
        public SingleTag() : base(TagType.Single) { }

        public SingleTag(ENBtBinaryReader reader)
            : this(reader.ReadSingle())
        {
            Contract.Requires<ArgumentNullException>(reader != null);
        }

        public SingleTag(Single value) : base(TagType.Single, value) { }

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

        protected override void WritePayloadTo(ENbtBinaryWriter writer)
        {
            writer.Write(this.Value);
        }
    }

    [TagHandlerFor(TagType.Double)]
    public class DoubleTag : ValueTag<Double>, IEquatable<DoubleTag>
    {
        public DoubleTag() : base(TagType.Double) { }

        public DoubleTag(ENBtBinaryReader reader)
            : this(reader.ReadDouble())
        {
            Contract.Requires<ArgumentNullException>(reader != null);
        }

        public DoubleTag(Double value) : base(TagType.Double, value) { }

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

        protected override void WritePayloadTo(ENbtBinaryWriter writer)
        {
            writer.Write(this.Value);
        }
    }
}