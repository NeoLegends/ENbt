using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENbt
{
    [TagHandlerFor(TagType.List)]
    public class ListTag : Tag, IEquatable<ListTag>, IList<Tag>
    {
        private readonly List<Tag> children = new List<Tag>();

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
                return sizeof(int) + children.Sum(child => child.Length);
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
                this.children[index] = value;
            }
        }

        public ListTag() : base(TagType.List) { }

        public ListTag(ENBtBinaryReader reader)
            : this()
        {
            Contract.Requires<ArgumentNullException>(reader != null);

            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                children.Add(Tag.ReadFrom(reader));
            }
        }

        public ListTag(params Tag[] children)
            : this((IEnumerable<Tag>)children)
        {
            Contract.Requires<ArgumentNullException>(children != null);
        }

        public ListTag(IEnumerable<Tag> children)
            : this()
        {
            Contract.Requires<ArgumentNullException>(children != null);

            this.children.AddRange(children);
        }

        public void Add(Tag item)
        {
            this.children.Add(item);
        }

        public void AddRange(IEnumerable<Tag> items)
        {
            Contract.Requires<ArgumentNullException>(items != null);

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
            return this.Equals(obj as ListTag);
        }

        public override bool Equals(Tag other)
        {
            return this.Equals(other as ListTag);
        }

        public bool Equals(ListTag other)
        {
            if (ReferenceEquals(other, this))
                return true;
            if (ReferenceEquals(other, null))
                return false;

            return this.SequenceEqual(other);
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
            return Hashing.GetCollectionHash(this.children);
        }

        public int IndexOf(Tag item)
        {
            return this.children.IndexOf(item);
        }

        public void Insert(int index, Tag item)
        {
            this.children.Insert(index, item);
        }

        public void InsertRange(int index, IEnumerable<Tag> items)
        {
            Contract.Requires<ArgumentOutOfRangeException>(index >= 0 && index <= this.Count);
            Contract.Requires<ArgumentNullException>(items != null);

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
        protected override void WritePayloadTo(ENbtBinaryWriter writer)
        {
            List<Tag> childsToWrite = this.children.Where(child => child != null).ToList();

            writer.Write(childsToWrite.Count);
            for (int i = 0; i < childsToWrite.Count; i++)
            {
                childsToWrite[i].WriteTo(writer);
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.children != null);
        }

        public static bool operator ==(ListTag left, ListTag right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(ListTag left, ListTag right)
        {
            return !(left == right);
        }
    }
}
