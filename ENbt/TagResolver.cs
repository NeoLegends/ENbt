using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENbt
{
    public delegate Tag TagInitializationDelegate(ENBtBinaryReader reader);

    internal class TagResolver
    {
        private static readonly ConcurrentDictionary<TagType, TagInitializationDelegate> initializers = new ConcurrentDictionary<TagType, TagInitializationDelegate>();

        public TagResolver() { }

        public TagInitializationDelegate Resolve(TagType type)
        {
            Contract.Ensures(Contract.Result<TagInitializationDelegate>() != null);

            TagInitializationDelegate result;
            if (!this.TryResolve(type, out result))
            {
                throw new TagUnknownException(type, "Tag unknown!");
            }
            return result;
        }

        public bool TryResolve(TagType type, out TagInitializationDelegate initializer)
        {
            if (TryGetDefaultType(type, out initializer) || initializers.TryGetValue(type, out initializer))
            {
                return true;
            }
            else
            {
                return false; // Resolve using reflection in the future
            }
        }

        private static bool TryGetDefaultType(TagType type, out TagInitializationDelegate initializer)
        {
            Contract.Ensures(!Contract.Result<bool>() || Contract.ValueAtReturn(out initializer) != null);

            switch (type)
            {
                case TagType.End:
                    initializer = rdr => new EndTag(rdr);
                    break;
                case TagType.Object:
                    initializer = rdr => new ObjectTag(rdr);
                    break;
                case TagType.List:
                    initializer = rdr => new ListTag(rdr);
                    break;
                case TagType.SByte:
                    initializer = rdr => new SByteTag(rdr);
                    break;
                case TagType.Byte:
                    initializer = rdr => new ByteTag(rdr);
                    break;
                case TagType.Int16:
                    initializer = rdr => new Int16Tag(rdr);
                    break;
                case TagType.UInt16:
                    initializer = rdr => new UInt16Tag(rdr);
                    break;
                case TagType.Int32:
                    initializer = rdr => new Int32Tag(rdr);
                    break;
                case TagType.UInt32:
                    initializer = rdr => new UInt32Tag(rdr);
                    break;
                case TagType.Int64:
                    initializer = rdr => new Int64Tag(rdr);
                    break;
                case TagType.UInt64:
                    initializer = rdr => new UInt64Tag(rdr);
                    break;
                case TagType.Single:
                    initializer = rdr => new SingleTag(rdr);
                    break;
                case TagType.Double:
                    initializer = rdr => new DoubleTag(rdr);
                    break;
                case TagType.String:
                    initializer = rdr => new StringTag(rdr);
                    break;
                case TagType.ByteArray:
                    initializer = rdr => new ByteArrayTag(rdr);
                    break;
                case TagType.ByteVector2:
                    initializer = rdr => new ByteVector2Tag(rdr);
                    break;
                case TagType.ByteVector3:
                    initializer = rdr => new ByteVector2Tag(rdr);
                    break;
                case TagType.ByteVector4:
                    initializer = rdr => new ByteVector2Tag(rdr);
                    break;
                case TagType.SingleVector2:
                    initializer = rdr => new SingleVector2Tag(rdr);
                    break;
                case TagType.SingleVector3:
                    initializer = rdr => new SingleVector3Tag(rdr);
                    break;
                case TagType.SingleVector4:
                    initializer = rdr => new SingleVector4Tag(rdr);
                    break;
                case TagType.DoubleVector2:
                    initializer = rdr => new DoubleVector2Tag(rdr);
                    break;
                case TagType.DoubleVector3:
                    initializer = rdr => new DoubleVector3Tag(rdr);
                    break;
                case TagType.DoubleVector4:
                    initializer = rdr => new DoubleVector4Tag(rdr);
                    break;
                default:
                    initializer = null;
                    break;
            }

            return (initializer != null);
        }
    }
}
