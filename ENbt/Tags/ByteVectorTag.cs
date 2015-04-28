using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ENbt
{
    [TagHandlerFor(TagType.ByteVector2)]
    public class ByteVector2Tag : Tag, IEquatable<ByteVector2Tag>
    {
        public override int PayloadLength
        {
            get
            {
                return 2;
            }
        }

        public byte X { get; set; }

        public byte Y { get; set; }

        public ByteVector2Tag() : base(TagType.ByteVector2) { }

        public ByteVector2Tag(ENBtBinaryReader reader)
            : this(reader.ReadByte(), reader.ReadByte())
        {
            Contract.Requires<ArgumentNullException>(reader != null);
        }

        public ByteVector2Tag(byte x, byte y)
            : this()
        {
            this.X = x;
            this.Y = y;
        }

        public override bool Equals(Tag other)
        {
            ByteVector2Tag tag = other as ByteVector2Tag;
            return (tag != null) && this.Equals(tag);
        }

        public bool Equals(ByteVector2Tag other)
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

    [TagHandlerFor(TagType.ByteVector3)]
    public class ByteVector3Tag : Tag, IEquatable<ByteVector3Tag>
    {
        public override int PayloadLength
        {
            get
            {
                return 3;
            }
        }

        public byte X { get; set; }

        public byte Y { get; set; }

        public byte Z { get; set; }

        public ByteVector3Tag() : base(TagType.ByteVector3) { }

        public ByteVector3Tag(ENBtBinaryReader reader)
            : this(reader.ReadByte(), reader.ReadByte(), reader.ReadByte())
        {
            Contract.Requires<ArgumentNullException>(reader != null);
        }

        public ByteVector3Tag(byte x, byte y, byte z)
            : this()
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public override bool Equals(Tag other)
        {
            ByteVector3Tag tag = other as ByteVector3Tag;
            return (tag != null) && this.Equals(tag);
        }

        public bool Equals(ByteVector3Tag other)
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

    [TagHandlerFor(TagType.ByteVector4)]
    public class ByteVector4Tag : Tag, IEquatable<ByteVector4Tag>
    {
        public override int PayloadLength
        {
            get
            {
                return 4;
            }
        }

        public byte X { get; set; }

        public byte Y { get; set; }

        public byte Z { get; set; }

        public byte W { get; set; }

        public ByteVector4Tag() : base(TagType.ByteVector4) { }

        public ByteVector4Tag(ENBtBinaryReader reader)
            : this(reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), reader.ReadByte())
        {
            Contract.Requires<ArgumentNullException>(reader != null);
        }

        public ByteVector4Tag(byte x, byte y, byte z, byte w)
            : this()
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        public override bool Equals(Tag other)
        {
            ByteVector4Tag tag = other as ByteVector4Tag;
            return (tag != null) && this.Equals(tag);
        }

        public bool Equals(ByteVector4Tag other)
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
