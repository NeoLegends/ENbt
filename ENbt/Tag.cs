using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENbt
{
    /// <summary>
    /// Represents a value in ENbt.
    /// </summary>
    [ContractClass(typeof(TagContracts))]
    public abstract class Tag : IEquatable<Tag>
    {
        #region Conversion Properties

        public byte AsByte
        {
            get
            {
                return this.As<byte>();
            }
        }

        [CLSCompliant(false)]
        public sbyte AsSByte
        {
            get
            {
                return this.As<sbyte>();
            }
        }

        public short AsInt16
        {
            get
            {
                return this.As<short>();
            }
        }

        [CLSCompliant(false)]
        public ushort AsUInt16
        {
            get
            {
                return this.As<ushort>();
            }
        }

        public int AsInt32
        {
            get
            {
                return this.As<int>();
            }
        }

        [CLSCompliant(false)]
        public uint AsUInt32
        {
            get
            {
                return this.As<uint>();
            }
        }

        public long AsInt64
        {
            get
            {
                return this.As<long>();
            }
        }

        [CLSCompliant(false)]
        public ulong AsUInt64
        {
            get
            {
                return this.As<ulong>();
            }
        }

        public float AsSingle
        {
            get
            {
                return this.As<float>();
            }
        }

        public double AsDouble
        {
            get
            {
                return this.As<double>();
            }
        }

        public string AsString
        {
            get
            {
                return this.As<string>();
            }
        }

        public ObjectTag AsObject
        {
            get
            {
                return this.As<ObjectTag>();
            }
        }

        public ArrayTag AsArray
        {
            get
            {
                return this.As<ArrayTag>();
            }
        }

        public ListTag AsList
        {
            get
            {
                return this.As<ListTag>();
            }
        }

        public DateTime AsDate
        {
            get
            {
                return this.As<DateTime>();
            }
        }

        public TimeSpan AsTimeSpan
        {
            get
            {
                return this.As<TimeSpan>();
            }
        }

        #endregion

        /// <summary>
        /// The total length (in bytes) of the <see cref="Tag"/>-
        /// </summary>
        public virtual int Length
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= sizeof(TagType));

                return sizeof(TagType) + this.PayloadLength;
            }
        }

        /// <summary>
        /// The length of the <see cref="Tag"/> minus the length of the header.
        /// </summary>
        public abstract int PayloadLength { get; }

        /// <summary>
        /// The type of the <see cref="Tag"/>.
        /// </summary>
        /// <seealso cref="TagType"/>
        public abstract TagType Type { get; }

        /// <summary>
        /// Initializes a new <see cref="Tag"/>.
        /// </summary>
        protected Tag() { }

        /// <summary>
        /// Casts the <see cref="Tag"/> to another object of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of object to cast to.</typeparam>
        /// <returns>The <see cref="Tag"/> casted to an object of the specified type.</returns>
        public T As<T>()
        {
            return (T)(object)this;
        }

        /// <summary>
        /// Checks whether the current object equals the other specified object.
        /// </summary>
        /// <param name="obj">The object to test for equality.</param>
        /// <returns><c>true</c> if the objects are equal, otherwise <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, this))
                return true;
            if (ReferenceEquals(obj, null))
                return false;

            return this.Equals(obj as Tag);
        }

        /// <summary>
        /// Checks whether the current object equals the other specified object.
        /// </summary>
        /// <param name="other">The object to test for equality.</param>
        /// <returns><c>true</c> if the objects are equal, otherwise <c>false</c>.</returns>
        public abstract bool Equals(Tag other);

        /// <summary>
        /// Writes the <see cref="Tag"/> to the specified <paramref name="destination"/>.
        /// </summary>
        /// <param name="destination">The <see cref="Stream"/> to write the <see cref="Tag"/> to.</param>
        public void WriteTo(Stream destination)
        {
            Contract.Requires<ArgumentNullException>(destination != null);

            using (ENbtBinaryWriter bw = new ENbtBinaryWriter(destination, true))
            {
                this.WriteTo(bw);
            }
        }

        /// <summary>
        /// Writes the <see cref="Tag"/> to the specified <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The <see cref="ENbtBinaryWriter"/> to write the tag to.</param>
        public void WriteTo(ENbtBinaryWriter writer)
        {
            Contract.Requires<ArgumentNullException>(writer != null);

            writer.Write(this.Type);
            this.WritePayloadTo(writer);
        }

        /// <summary>
        /// Writes the payload (all data without the tag type) of the <see cref="Tag"/> to the specified <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The <see cref="ENbtBinaryWriter"/> to write the tag to.</param>
        public abstract void WritePayloadTo(ENbtBinaryWriter writer);

        public static Tag ReadFrom(Stream source)
        {
            Contract.Requires<ArgumentNullException>(source != null);
            Contract.Requires<InvalidOperationException>(source.CanRead);

            return ReadFrom(source, TagResolver.Default);
        }

        public static Tag ReadFrom(Stream source, TagResolver resolver)
        {
            Contract.Requires<ArgumentNullException>(source != null);
            Contract.Requires<InvalidOperationException>(source.CanRead);
            Contract.Requires<ArgumentNullException>(resolver != null);

            using (ENbtBinaryReader rdr = new ENbtBinaryReader(source, true))
            {
                return ReadFrom(rdr, resolver);
            }
        }

        public static Tag ReadFrom(ENbtBinaryReader reader)
        {
            Contract.Requires<ArgumentNullException>(reader != null);

            return ReadFrom(reader, TagResolver.Default);
        }

        public static Tag ReadFrom(ENbtBinaryReader reader, TagResolver resolver)
        {
            Contract.Requires<ArgumentNullException>(reader != null);
            Contract.Requires<ArgumentNullException>(resolver != null);

            return resolver.Resolve(reader.ReadTagType())
                           .Invoke(reader);
        }

        public static Tag ReadFrom(Stream source, TagType tagType)
        {
            Contract.Requires<ArgumentNullException>(source != null);
            Contract.Requires<InvalidOperationException>(source.CanRead);

            return ReadFrom(source, tagType, TagResolver.Default);
        }

        public static Tag ReadFrom(Stream source, TagType tagType, TagResolver resolver)
        {
            Contract.Requires<ArgumentNullException>(source != null);
            Contract.Requires<InvalidOperationException>(source.CanRead);
            Contract.Requires<ArgumentNullException>(resolver != null);

            using (ENbtBinaryReader rdr = new ENbtBinaryReader(source, true))
            {
                return ReadFrom(rdr, tagType, resolver);
            }
        }

        public static Tag ReadFrom(ENbtBinaryReader reader, TagType tagType)
        {
            Contract.Requires<ArgumentNullException>(reader != null);

            return ReadFrom(reader, tagType, TagResolver.Default);
        }

        public static Tag ReadFrom(ENbtBinaryReader reader, TagType tagType, TagResolver resolver)
        {
            Contract.Requires<ArgumentNullException>(reader != null);
            Contract.Requires<ArgumentNullException>(resolver != null);

            return resolver.Resolve(tagType)
                           .Invoke(reader);
        }

        public static T ReadFrom<T>(Stream source)
            where T : Tag
        {
            Contract.Requires<ArgumentNullException>(source != null);
            Contract.Requires<InvalidOperationException>(source.CanRead);

            return (T)ReadFrom(source);
        }

        public static T ReadFrom<T>(Stream source, TagResolver resolver)
            where T : Tag
        {
            Contract.Requires<ArgumentNullException>(source != null);
            Contract.Requires<InvalidOperationException>(source.CanRead);
            Contract.Requires<ArgumentNullException>(resolver != null);

            return (T)ReadFrom(source, resolver);
        }

        public static T ReadFrom<T>(ENbtBinaryReader reader)
            where T : Tag
        {
            Contract.Requires<ArgumentNullException>(reader != null);

            return (T)ReadFrom(reader);
        }

        public static T ReadFrom<T>(ENbtBinaryReader reader, TagResolver resolver)
            where T : Tag
        {
            Contract.Requires<ArgumentNullException>(reader != null);
            Contract.Requires<ArgumentNullException>(resolver != null);

            return (T)ReadFrom(reader, resolver);
        }

        public static T ReadFrom<T>(Stream source, TagType tagType)
            where T : Tag
        {
            Contract.Requires<ArgumentNullException>(source != null);
            Contract.Requires<InvalidOperationException>(source.CanRead);

            return (T)ReadFrom(source, tagType);
        }

        public static T ReadFrom<T>(Stream source, TagType tagType, TagResolver resolver)
            where T : Tag
        {
            Contract.Requires<ArgumentNullException>(source != null);
            Contract.Requires<InvalidOperationException>(source.CanRead);
            Contract.Requires<ArgumentNullException>(resolver != null);

            return (T)ReadFrom(source, tagType, resolver);
        }

        public static T ReadFrom<T>(ENbtBinaryReader reader, TagType tagType)
            where T : Tag
        {
            Contract.Requires<ArgumentNullException>(reader != null);

            return (T)ReadFrom(reader, tagType);
        }

        public static T ReadFrom<T>(ENbtBinaryReader reader, TagType tagType, TagResolver resolver)
            where T : Tag
        {
            Contract.Requires<ArgumentNullException>(reader != null);
            Contract.Requires<ArgumentNullException>(resolver != null);

            return (T)ReadFrom(reader, tagType, resolver);
        }

        public static bool operator ==(Tag left, Tag right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(Tag left, Tag right)
        {
            return !(left == right);
        }
    }

    [ContractClassFor(typeof(Tag))]
    abstract class TagContracts : Tag
    {
        public override int PayloadLength
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);

                return 0;
            }
        }

        public override TagType Type
        {
            get 
            {
                return TagType.End;
            }
        }

        protected TagContracts() { }

        public override bool Equals(Tag other)
        {
            return false;
        }

        public override void WritePayloadTo(ENbtBinaryWriter writer)
        {
            Contract.Requires<ArgumentNullException>(writer != null);
        }
    }
}
