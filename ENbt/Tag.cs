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

            using (ENBtBinaryReader rdr = new ENBtBinaryReader(source, false))
            {
                return ReadFrom(rdr);
            }
        }

        public static Tag ReadFrom(ENBtBinaryReader reader)
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

        public static T ReadFrom<T>(ENBtBinaryReader reader)
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
