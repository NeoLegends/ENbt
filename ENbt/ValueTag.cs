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

        protected ValueTag() { }

        protected ValueTag(T value) 
        {
            this.Value = value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, this))
                return true;
            if (ReferenceEquals(obj, null))
                return false;

            return this.Equals(obj as ValueTag<T>);
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

        public static bool operator ==(ValueTag<T> left, ValueTag<T> right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(ValueTag<T> left, ValueTag<T> right)
        {
            return !(left == right);
        }
    }
}
