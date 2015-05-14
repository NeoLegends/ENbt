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

        public override TagType Type
        {
            get
            {
                return TagType.ByteVector2;
            }
        }

        public byte X { get; set; }

        public byte Y { get; set; }

        public ByteVector2Tag() { }

        public ByteVector2Tag(ENbtBinaryReader reader)
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

        public override void WritePayloadTo(ENbtBinaryWriter writer)
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

        public override TagType Type
        {
            get
            {
                return TagType.ByteVector3;
            }
        }

        public byte X { get; set; }

        public byte Y { get; set; }

        public byte Z { get; set; }

        public ByteVector3Tag() { }

        public ByteVector3Tag(ENbtBinaryReader reader)
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

        public override void WritePayloadTo(ENbtBinaryWriter writer)
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

        public override TagType Type
        {
            get
            {
                return TagType.ByteVector4;
            }
        }

        public byte X { get; set; }

        public byte Y { get; set; }

        public byte Z { get; set; }

        public byte W { get; set; }

        public ByteVector4Tag() { }

        public ByteVector4Tag(ENbtBinaryReader reader)
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

        public override void WritePayloadTo(ENbtBinaryWriter writer)
        {
            writer.Write(this.X);
            writer.Write(this.Y);
            writer.Write(this.Z);
            writer.Write(this.W);
        }
    }
}
