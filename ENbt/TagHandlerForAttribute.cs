using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENbt
{
    public sealed class TagHandlerForAttribute : Attribute
    {
        public TagType Type { get; private set; }

        public TagHandlerForAttribute(TagType type)
        {
            this.Type = type;
        }
    }
}
