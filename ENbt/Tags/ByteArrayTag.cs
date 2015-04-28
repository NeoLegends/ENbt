﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ENbt
{
    [TagHandlerFor(TagType.ByteArray)]
    public class ByteArrayTag : ValueTag<byte[]>, IEquatable<ByteArrayTag>
    {
        public ByteArrayTag() : base(TagType.ByteArray) { }

        public ByteArrayTag(byte[] value) : base(TagType.ByteArray, value) { }

        public ByteArrayTag(ENBtBinaryReader reader)
            : this()
        {
            Contract.Requires<ArgumentNullException>(reader != null);

            this.Value = new byte[reader.ReadInt32()];
            if (reader.Read(this.Value, 0, this.Value.Length) < this.Value.Length)
            {
                throw new EndOfStreamException(string.Format("The specified amount of bytes ({0}) could not be read.", this.Value.Length));
            }
        }

        public override bool Equals(Tag other)
        {
            ByteArrayTag tag = other as ByteArrayTag;
            return (tag != null) && this.Equals(tag);
        }

        public bool Equals(ByteArrayTag other)
        {
            if (ReferenceEquals(other, this))
                return true;
            if (ReferenceEquals(other, null))
                return false;

            return ReferenceEquals(this.Value, other.Value) || ((this.Value != null && other.Value != null) && this.Value.SequenceEqual(other.Value));
        }

        public override int GetHashCode()
        {
            return Hashing.GetCollectionHash(this.Value);
        }

        protected override void WritePayloadTo(ENbtBinaryWriter writer)
        {
            writer.Write(this.Value.Length);
            writer.Write(this.Value);
        }
    }
}