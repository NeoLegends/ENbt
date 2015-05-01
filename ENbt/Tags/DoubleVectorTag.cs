using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENbt
{
    [TagHandlerFor(TagType.DoubleVector2)]
    public class DoubleVector2Tag : Tag, IEquatable<DoubleVector2Tag>
    {
        public override int PayloadLength
        {
            get
            {
                return sizeof(double) * 2;
            }
        }

        public double X { get; set; }

        public double Y { get; set; }

        public DoubleVector2Tag() : base(TagType.DoubleVector2) { }

        public DoubleVector2Tag(ENbtBinaryReader reader)
            : this(reader.ReadDouble(), reader.ReadDouble())
        {
            Contract.Requires<ArgumentNullException>(reader != null);
        }

        public DoubleVector2Tag(double x, double y)
            : this()
        {
            this.X = x;
            this.Y = y;
        }

        public override bool Equals(Tag other)
        {
            DoubleVector2Tag tag = other as DoubleVector2Tag;
            return (tag != null) && this.Equals(tag);
        }

        public bool Equals(DoubleVector2Tag other)
        {
            if (ReferenceEquals(other, this))
                return true;
            if (ReferenceEquals(other, null))
                return false;

            return (this.X == other.X) && (this.Y == other.Y);
        }

        protected override void WritePayloadTo(ENbtBinaryWriter writer)
        {
            writer.Write(this.X);
            writer.Write(this.Y);
        }
    }

    [TagHandlerFor(TagType.DoubleVector3)]
    public class DoubleVector3Tag : Tag, IEquatable<DoubleVector3Tag>
    {
        public override int PayloadLength
        {
            get
            {
                return sizeof(double) * 3;
            }
        }

        public double X { get; set; }

        public double Y { get; set; }

        public double Z { get; set; }

        public DoubleVector3Tag() : base(TagType.DoubleVector3) { }

        public DoubleVector3Tag(ENbtBinaryReader reader)
            : this(reader.ReadDouble(), reader.ReadDouble(), reader.ReadDouble())
        {
            Contract.Requires<ArgumentNullException>(reader != null);
        }

        public DoubleVector3Tag(double x, double y, double z)
            : this()
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public override bool Equals(Tag other)
        {
            DoubleVector3Tag tag = other as DoubleVector3Tag;
            return (tag != null) && this.Equals(tag);
        }

        public bool Equals(DoubleVector3Tag other)
        {
            if (ReferenceEquals(other, this))
                return true;
            if (ReferenceEquals(other, null))
                return false;

            return (this.X == other.X) && (this.Y == other.Y) && (this.Z == other.Z);
        }

        protected override void WritePayloadTo(ENbtBinaryWriter writer)
        {
            writer.Write(this.X);
            writer.Write(this.Y);
            writer.Write(this.Z);
        }
    }

    [TagHandlerFor(TagType.DoubleVector4)]
    public class DoubleVector4Tag : Tag, IEquatable<DoubleVector4Tag>
    {
        public override int PayloadLength
        {
            get
            {
                return sizeof(double) * 4;
            }
        }

        public double X { get; set; }

        public double Y { get; set; }

        public double Z { get; set; }

        public double W { get; set; }

        public DoubleVector4Tag() : base(TagType.DoubleVector4) { }

        public DoubleVector4Tag(ENbtBinaryReader reader)
            : this(reader.ReadDouble(), reader.ReadDouble(), reader.ReadDouble(), reader.ReadDouble())
        {
            Contract.Requires<ArgumentNullException>(reader != null);
        }

        public DoubleVector4Tag(double x, double y, double z, double w)
            : this()
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        public override bool Equals(Tag other)
        {
            DoubleVector4Tag tag = other as DoubleVector4Tag;
            return (tag != null) && this.Equals(tag);
        }

        public bool Equals(DoubleVector4Tag other)
        {
            if (ReferenceEquals(other, this))
                return true;
            if (ReferenceEquals(other, null))
                return false;

            return (this.X == other.X) && (this.Y == other.Y) && (this.Z == other.Z) && (this.W == other.W);
        }

        protected override void WritePayloadTo(ENbtBinaryWriter writer)
        {
            writer.Write(this.X);
            writer.Write(this.Y);
            writer.Write(this.Z);
            writer.Write(this.W);
        }
    }
}
