using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENbt
{
    [TagHandlerFor(TagType.Object)]
    public class ObjectTag : Tag, IEquatable<ObjectTag>, IDictionary<StringTag, Tag>, IReadOnlyList<KeyValuePair<StringTag, Tag>>
    {
        private readonly Dictionary<StringTag, Tag> children = new Dictionary<StringTag, Tag>();

        public int Count
        {
            get
            {
                return this.children.Count;
            }
        }

        bool ICollection<KeyValuePair<StringTag, Tag>>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public ICollection<StringTag> Keys
        {
            get
            {
                return this.children.Keys;
            }
        }

        public override int PayloadLength
        {
            get 
            {
                return children.Values.Sum(child => child.Length) + EndTag.Default.Length;
            }
        }

        public ICollection<Tag> Values
        {
            get
            {
                return this.children.Values;
            }
        }

        public KeyValuePair<StringTag, Tag> this[int index]
        {
            get
            {
                return this.children.ElementAt(index);
            }
        }

        public Tag this[StringTag key]
        {
            get
            {
                return this.children[key];
            }
            set
            {
                this.children[key] = value;
            }
        }

        public ObjectTag() : base(TagType.Object) { }

        public ObjectTag(ENBtBinaryReader reader)
            : this()
        {
            Contract.Requires<ArgumentNullException>(reader != null);

            this.children = new Dictionary<StringTag, Tag>();

            Tag readTag = null;
            while (!(readTag is EndTag))
            {
                readTag = Tag.ReadFrom(reader);

                StringTag stringTag = readTag as StringTag;
                if (stringTag != null)
                {
                    this.children.Add(stringTag, Tag.ReadFrom(reader));
                    break;
                }
            }
        }

        public ObjectTag(IDictionary<string, Tag> children)
            : this(children.ToDictionary(kvp => new StringTag(kvp.Key), kvp => kvp.Value))
        {
            Contract.Requires<ArgumentNullException>(children != null);
        }

        public ObjectTag(IDictionary<StringTag, Tag> children) 
            : this()
        {
            Contract.Requires<ArgumentNullException>(children != null);

            this.children = children.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value
            );
        }

        public void Add(string name, Tag item)
        {
            Contract.Requires<ArgumentNullException>(name != null);

            this.Add((StringTag)name, item);
        }

        public void Add(StringTag key, Tag value)
        {
            this.children.Add(key, value);
        }

        void ICollection<KeyValuePair<StringTag, Tag>>.Add(KeyValuePair<StringTag, Tag> item)
        {
            this.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            this.children.Clear();
        }

        public bool Contains(KeyValuePair<StringTag, Tag> item)
        {
            return this.children.Contains(item);
        }

        public bool ContainsKey(StringTag key)
        {
            return this.children.ContainsKey(key);
        }

        void ICollection<KeyValuePair<StringTag, Tag>>.CopyTo(KeyValuePair<StringTag, Tag>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<StringTag, Tag>>)this.children).CopyTo(array, arrayIndex);
        }

        public override bool Equals(Tag other)
        {
            return (other != null) && this.Equals(other as ObjectTag);
        }

        public bool Equals(ObjectTag other)
        {
            if (ReferenceEquals(other, this))
                return true;
            if (ReferenceEquals(other, null))
                return false;

            return this.children.SequenceEqual(other.children);
        }

        public IEnumerator<KeyValuePair<StringTag, Tag>> GetEnumerator()
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

        public bool Remove(StringTag key)
        {
            return this.children.Remove(key);
        }

        bool ICollection<KeyValuePair<StringTag, Tag>>.Remove(KeyValuePair<StringTag, Tag> item)
        {
            return ((ICollection<KeyValuePair<StringTag, Tag>>)this.children).Remove(item);
        }

        public bool TryGetValue(StringTag key, out Tag value)
        {
            return this.children.TryGetValue(key, out value);
        }

        protected override void WritePayloadTo(ENbtBinaryWriter writer)
        {
            foreach (KeyValuePair<StringTag, Tag> child in this.children)
            {
                if (child.Key != null && child.Value != null)
                {
                    child.Key.WriteTo(writer);
                    child.Value.WriteTo(writer);
                }
            }
            EndTag.Default.WriteTo(writer);
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.children != null);
        }
    }
}
