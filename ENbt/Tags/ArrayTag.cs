using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENbt
{
    [TagHandlerFor(TagType.Array)]
    public class ArrayTag : Tag, IEquatable<ArrayTag>, IList<Tag>
    {
        private readonly List<Tag> children = new List<Tag>();

        public TagType ChildrenType { get; private set; }

        public int Count
        {
            get
            {
                return this.children.Count;
            }
        }

        bool ICollection<Tag>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public override int PayloadLength
        {
            // Code contracts does not seem to like .Sum. It warns even though there are contracts that prevent the case it is warning about
            [ContractVerification(false)]
            get
            {
                return sizeof(int) + sizeof(TagType) + children.Sum(child => child.PayloadLength);
            }
        }

        public override TagType Type
        {
            get
            {
                return TagType.Array;
            }
        }

        public Tag this[int index]
        {
            get
            {
                return this.children[index];
            }
            set
            {
                if (value != null && value.Type != this.ChildrenType)
                {
                    throw new ArgumentException("Item cannot be set since the children type mismatches.");
                }
                this.children[index] = value;
            }
        }

        public ArrayTag(TagType childrenType) 
        {
            this.ChildrenType = childrenType;
        }

        public ArrayTag(ENbtBinaryReader reader)
        {
            Contract.Requires<ArgumentNullException>(reader != null);

            this.ChildrenType = reader.ReadTagType();
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                children.Add(Tag.ReadFrom(reader, this.ChildrenType));
            }
        }

        public ArrayTag(TagType childrenType, params Tag[] children)
            : this(children, childrenType)
        {
            Contract.Requires<ArgumentNullException>(children != null);
            Contract.Requires<ArgumentException>(children.All(child => child.Type == childrenType));
        }

        public ArrayTag(IEnumerable<Tag> children, TagType childrenType)
        {
            Contract.Requires<ArgumentNullException>(children != null);
            Contract.Requires<ArgumentException>(children.All(child => child.Type == childrenType));

            this.ChildrenType = childrenType;
            this.children.AddRange(children);
        }

        public void Add(Tag item)
        {
            if (item.Type != this.ChildrenType)
            {
                throw new ArgumentException("Item cannot be added, since the child type mismatches.");
            }
            this.children.Add(item);
        }

        public void AddRange(IEnumerable<Tag> items)
        {
            Contract.Requires<ArgumentNullException>(items != null);
            Contract.Requires<ArgumentException>(items.All(child => child.Type == this.ChildrenType));

            this.children.AddRange(items);
        }

        public void Clear()
        {
            this.children.Clear();
        }

        public bool Contains(Tag item)
        {
            return this.children.Contains(item);
        }

        public void CopyTo(Tag[] array, int arrayIndex)
        {
            this.children.CopyTo(array, arrayIndex);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as ArrayTag);
        }

        public override bool Equals(Tag other)
        {
            return this.Equals(other as ArrayTag);
        }

        public bool Equals(ArrayTag other)
        {
            if (ReferenceEquals(other, this))
                return true;
            if (ReferenceEquals(other, null))
                return false;

            return (this.ChildrenType == other.ChildrenType) && this.SequenceEqual(other);
        }

        public IEnumerator<Tag> GetEnumerator()
        {
            return this.children.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public override int GetHashCode()
        {
            return Hashing.GetHashCode(this.ChildrenType, Hashing.GetCollectionHash(this.children));
        }

        public int IndexOf(Tag item)
        {
            return this.children.IndexOf(item);
        }

        public void Insert(int index, Tag item)
        {
            if (item.Type != this.ChildrenType)
            {
                throw new ArgumentException("Item cannot be added, since the child type mismatches.");
            }

            this.children.Insert(index, item);
        }

        public void InsertRange(int index, IEnumerable<Tag> items)
        {
            Contract.Requires<ArgumentOutOfRangeException>(index >= 0 && index <= this.Count);
            Contract.Requires<ArgumentNullException>(items != null);
            Contract.Requires<ArgumentException>(items.All(child => child.Type == this.ChildrenType));

            this.children.InsertRange(index, items);
        }

        public bool Remove(Tag item)
        {
            return this.children.Remove(item);
        }

        public void RemoveAt(int index)
        {
            this.children.RemoveAt(index);
        }

        [ContractVerification(false)] // Code contracts doesn't see the .Where(child => child != null)-clause and thus warns.
        public override void WritePayloadTo(ENbtBinaryWriter writer)
        {
            List<Tag> childsToWrite = this.children.Where(child => child != null).ToList();

            writer.Write(this.ChildrenType);
            writer.Write(childsToWrite.Count);
            for (int i = 0; i < childsToWrite.Count; i++)
            {
                childsToWrite[i].WritePayloadTo(writer);
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.children != null);
        }

        public static bool operator ==(ArrayTag left, ArrayTag right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(ArrayTag left, ArrayTag right)
        {
            return !(left == right);
        }
    }
}
