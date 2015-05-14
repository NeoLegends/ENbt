using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENbt
{
    [TagHandlerFor(TagType.Int32Vector2)]
    public class Int32Vector2Tag : Tag, IEquatable<Int32Vector2Tag>
    {
        public override int PayloadLength
        {
            get
            {
                return sizeof(int) * 2;
            }
        }

        public override TagType Type
        {
            get
            {
                return TagType.Int32Vector2;
            }
        }

        public int X { get; set; }

        public int Y { get; set; }

        public Int32Vector2Tag() { }

        public Int32Vector2Tag(ENbtBinaryReader reader)
            : this(reader.ReadInt32(), reader.ReadInt32())
        {
            Contract.Requires<ArgumentNullException>(reader != null);
        }

        public Int32Vector2Tag(int x, int y)
            : this()
        {
            this.X = x;
            this.Y = y;
        }

        public override bool Equals(Tag other)
        {
            Int32Vector2Tag tag = other as Int32Vector2Tag;
            return (tag != null) && this.Equals(tag);
        }

        public bool Equals(Int32Vector2Tag other)
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

    [TagHandlerFor(TagType.Int32Vector3)]
    public class Int32Vector3Tag : Tag, IEquatable<Int32Vector3Tag>
    {
        public override int PayloadLength
        {
            get
            {
                return sizeof(int) * 3;
            }
        }

        public override TagType Type
        {
            get
            {
                return TagType.Int32Vector3;
            }
        }

        public int X { get; set; }

        public int Y { get; set; }

        public int Z { get; set; }

        public Int32Vector3Tag() { }

        public Int32Vector3Tag(ENbtBinaryReader reader)
            : this(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32())
        {
            Contract.Requires<ArgumentNullException>(reader != null);
        }

        public Int32Vector3Tag(int x, int y, int z)
            : this()
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public override bool Equals(Tag other)
        {
            Int32Vector3Tag tag = other as Int32Vector3Tag;
            return (tag != null) && this.Equals(tag);
        }

        public bool Equals(Int32Vector3Tag other)
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

    [TagHandlerFor(TagType.Int32Vector4)]
    public class Int32Vector4Tag : Tag, IEquatable<Int32Vector4Tag>
    {
        public override int PayloadLength
        {
            get
            {
                return sizeof(int) * 4;
            }
        }

        public override TagType Type
        {
            get
            {
                return TagType.Int32Vector4;
            }
        }

        public int X { get; set; }

        public int Y { get; set; }

        public int Z { get; set; }

        public int W { get; set; }

        public Int32Vector4Tag() { }

        public Int32Vector4Tag(ENbtBinaryReader reader)
            : this(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32())
        {
            Contract.Requires<ArgumentNullException>(reader != null);
        }

        public Int32Vector4Tag(int x, int y, int z, int w)
            : this()
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        public override bool Equals(Tag other)
        {
            Int32Vector4Tag tag = other as Int32Vector4Tag;
            return (tag != null) && this.Equals(tag);
        }

        public bool Equals(Int32Vector4Tag other)
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
