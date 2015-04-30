using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ENbt
{
    public abstract class ValueTag<T> : Tag
    {
        public override int PayloadLength
        {
            get
            {
                return Marshal.SizeOf(typeof(T));
            }
        }

        public virtual T Value { get; set; }

        protected ValueTag(TagType type) : base(type) { }

        protected ValueTag(TagType type, T value) 
            : base(type) 
        {
            this.Value = value;
        }

        public override int GetHashCode()
        {
            return Hashing.GetHashCode(this.Value);
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }

        public static implicit operator T(ValueTag<T> tag)
        {
            return (tag != null) ? tag.Value : default(T);
        }
    }
}
