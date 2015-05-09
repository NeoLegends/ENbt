using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENbt
{
    [TagHandlerFor(TagType.End)]
    public class EndTag : Tag, IEquatable<EndTag>
    {
        private static readonly EndTag _Default = new EndTag();

        public static EndTag Default
        {
            get
            {
                return _Default;
            }
        }

        public override int PayloadLength
        {
            get 
            {
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

        public EndTag() { }

        public EndTag(ENbtBinaryReader reader) : this() { }

        public override bool Equals(Tag other)
        {
            return (other is EndTag);
        }

        public bool Equals(EndTag other)
        {
            return this.Equals((Tag)other);
        }

        protected override void WritePayloadTo(ENbtBinaryWriter writer) { }
    }
}
