﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENbt
{
    [TagHandlerFor(TagType.SingleVector2)]
    public class SingleVector2Tag : Tag, IEquatable<SingleVector2Tag>
    {
        public override int PayloadLength
        {
            get
            {
                return sizeof(float) * 2;
            }
        }

        public override TagType Type
        {
            get
            {
                return TagType.SingleVector2;
            }
        }

        public float X { get; set; }

        public float Y { get; set; }

        public SingleVector2Tag() { }

        public SingleVector2Tag(ENbtBinaryReader reader)
            : this(reader.ReadSingle(), reader.ReadSingle())
        {
            Contract.Requires<ArgumentNullException>(reader != null);
        }

        public SingleVector2Tag(float x, float y)
            : this()
        {
            this.X = x;
            this.Y = y;
        }

        public override bool Equals(Tag other)
        {
            SingleVector2Tag tag = other as SingleVector2Tag;
            return (tag != null) && this.Equals(tag);
        }

        public bool Equals(SingleVector2Tag other)
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

    [TagHandlerFor(TagType.SingleVector3)]
    public class SingleVector3Tag : Tag, IEquatable<SingleVector3Tag>
    {
        public override int PayloadLength
        {
            get
            {
                return sizeof(float) * 3;
            }
        }

        public override TagType Type
        {
            get
            {
                return TagType.SingleVector3;
            }
        }

        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }

        public SingleVector3Tag() { }

        public SingleVector3Tag(ENbtBinaryReader reader)
            : this(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle())
        {
            Contract.Requires<ArgumentNullException>(reader != null);
        }

        public SingleVector3Tag(float x, float y, float z)
            : this()
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public override bool Equals(Tag other)
        {
            SingleVector3Tag tag = other as SingleVector3Tag;
            return (tag != null) && this.Equals(tag);
        }

        public bool Equals(SingleVector3Tag other)
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

    [TagHandlerFor(TagType.SingleVector4)]
    public class SingleVector4Tag : Tag, IEquatable<SingleVector4Tag>
    {
        public override int PayloadLength
        {
            get
            {
                return sizeof(float) * 4;
            }
        }

        public override TagType Type
        {
            get
            {
                return TagType.SingleVector4;
            }
        }

        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }

        public float W { get; set; }

        public SingleVector4Tag() { }

        public SingleVector4Tag(ENbtBinaryReader reader)
            : this(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle())
        {
            Contract.Requires<ArgumentNullException>(reader != null);
        }

        public SingleVector4Tag(float x, float y, float z, float w)
            : this()
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        public override bool Equals(Tag other)
        {
            SingleVector4Tag tag = other as SingleVector4Tag;
            return (tag != null) && this.Equals(tag);
        }

        public bool Equals(SingleVector4Tag other)
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
