using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ENbt
{
    public class TagUnknownException : InvalidOperationException
    {
        public TagType Type { get; private set; }

        public TagUnknownException(TagType type) 
        {
            this.Type = type;
        }

        public TagUnknownException(TagType type, string message)
            : base(message)
        {
            this.Type = type;
        }

        public TagUnknownException(TagType type, string message, Exception inner)
            : base(message, inner)
        {
            this.Type = type;
        }

        protected TagUnknownException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.Type = (TagType)info.GetByte("TagType");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            try
            {
                info.AddValue("TagType", (byte)this.Type);
            }
            finally
            {
                base.GetObjectData(info, context);
            }
        }
    }
}
