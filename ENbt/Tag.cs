using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENbt
{
    [ContractClass(typeof(TagContracts))]
    public abstract class Tag : IEquatable<Tag>
    {
        private static readonly TagResolver resolver = new TagResolver();

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

        public byte[] AsByteArray
        {
            get
            {
                return this.As<byte[]>();
            }
        }

        #endregion

        public virtual int Length
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= sizeof(TagType));

                return sizeof(TagType) + this.PayloadLength;
            }
        }

        public abstract int PayloadLength { get; }

        public TagType Type { get; private set; }

        protected Tag(TagType type)
        {
            this.Type = type;
        }

        public T As<T>()
        {
            return (T)(object)this;
        }

        public abstract bool Equals(Tag other);

        public void WriteTo(Stream destination)
        {
            Contract.Requires<ArgumentNullException>(destination != null);

            using (ENbtBinaryWriter bw = new ENbtBinaryWriter(destination, false))
            {
                this.WriteTo(bw);
            }
        }

        public void WriteTo(ENbtBinaryWriter writer)
        {
            Contract.Requires<ArgumentNullException>(writer != null);

            writer.Write(this.Type);
            this.WritePayloadTo(writer);
        }

        protected abstract void WritePayloadTo(ENbtBinaryWriter writer);

        public static Tag ReadFrom(Stream source)
        {
            Contract.Requires<ArgumentNullException>(source != null);
            Contract.Requires<InvalidOperationException>(source.CanRead);

            using (ENbtBinaryReader rdr = new ENbtBinaryReader(source, false))
            {
                return ReadFrom(rdr);
            }
        }

        public static Tag ReadFrom(ENbtBinaryReader reader)
        {
            Contract.Requires<ArgumentNullException>(reader != null);

            return resolver.Resolve(reader.ReadTagType())
                           .Invoke(reader);
        }

        public static T ReadFrom<T>(Stream source)
            where T : Tag
        {
            Contract.Requires<ArgumentNullException>(source != null);
            Contract.Requires<InvalidOperationException>(source.CanRead);

            return (T)ReadFrom(source);
        }

        public static T ReadFrom<T>(ENbtBinaryReader reader)
            where T : Tag
        {
            Contract.Requires<ArgumentNullException>(reader != null);

            return (T)ReadFrom(reader);
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

        protected TagContracts() : base(TagType.End) { }

        public override bool Equals(Tag other)
        {
            return false;
        }

        protected override void WritePayloadTo(ENbtBinaryWriter writer)
        {
            Contract.Requires<ArgumentNullException>(writer != null);
        }
    }
}
