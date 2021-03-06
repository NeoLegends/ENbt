﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENbt
{
    [TagHandlerFor(TagType.Int64)]
    public class Int64Tag : ValueTag<Int64>, IComparable<Int64Tag>, IEquatable<Int64Tag>
    {
        public override TagType Type
        {
            get
            {
                return TagType.Int64;
            }
        }

        public Int64Tag() { }

        public Int64Tag(ENbtBinaryReader reader)
            : this(reader.ReadInt64())
        {
            Contract.Requires<ArgumentNullException>(reader != null);
        }

        public Int64Tag(Int64 value) : base(value) { }

        public int CompareTo(Int64Tag other)
        {
            if (other == null)
            {
                return -1;
            }
            return this.Value.CompareTo(other.Value);
        }

        public override bool Equals(Tag other)
        {
            Int64Tag tag = other as Int64Tag;
            return (tag != null) && this.Equals(tag);
        }

        public bool Equals(Int64Tag other)
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

    [CLSCompliant(false)]
    [TagHandlerFor(TagType.UInt64)]
    public class UInt64Tag : ValueTag<UInt64>, IComparable<UInt64Tag>, IEquatable<UInt64Tag>
    {
        public override TagType Type
        {
            get
            {
                return TagType.UInt64;
            }
        }

        public UInt64Tag() { }

        public UInt64Tag(ENbtBinaryReader reader)
            : this(reader.ReadUInt64())
        {
            Contract.Requires<ArgumentNullException>(reader != null);
        }

        public UInt64Tag(UInt64 value) : base(value) { }

        public int CompareTo(UInt64Tag other)
        {
            if (other == null)
            {
                return -1;
            }
            return this.Value.CompareTo(other.Value);
        }

        public override bool Equals(Tag other)
        {
            UInt64Tag tag = other as UInt64Tag;
            return (tag != null) && this.Equals(tag);
        }

        public bool Equals(UInt64Tag other)
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
