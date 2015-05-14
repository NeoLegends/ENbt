using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ENbt
{
    public delegate Tag TagInitializationDelegate(ENbtBinaryReader reader);

    public static class TagResolver
    {
        private static readonly Type[] constructorFinderArray = new[] { typeof(ENbtBinaryReader) };

        private static readonly Assembly entryAssembly = Assembly.GetEntryAssembly();

        private static readonly ConcurrentDictionary<TagType, TagInitializationDelegate> initializers = new ConcurrentDictionary<TagType, TagInitializationDelegate>();

        private static readonly IEnumerable<Assembly> referencedAssemblies;

        private static readonly ParameterExpression readerParamExpression = Expression.Parameter(typeof(ENbtBinaryReader));

        static TagResolver()
        {
            if (entryAssembly != null)
            {
                referencedAssemblies = entryAssembly.GetReferencedAssemblies()
                                                    .Select(asmName => Assembly.Load(asmName))
                                                    .ToList();
            }
            else
            {
                referencedAssemblies = Enumerable.Empty<Assembly>();
            }
        }

        public static bool Register(TagType type, TagInitializationDelegate initializer)
        {
            Contract.Requires<ArgumentNullException>(initializer != null);

            return initializers.TryAdd(type, initializer);
        }

        public static TagInitializationDelegate Resolve(TagType type)
        {
            Contract.Ensures(Contract.Result<TagInitializationDelegate>() != null);

            TagInitializationDelegate result;
            if (!TryResolve(type, out result))
            {
                throw new TagUnknownException(type, "Tag unknown!");
            }
            return result;
        }

        public static bool TryResolve(TagType type, out TagInitializationDelegate initializer)
        {
            Contract.Ensures(!Contract.Result<bool>() || Contract.ValueAtReturn(out initializer) != null);

            if (TryGetDefaultType(type, out initializer) || initializers.TryGetValue(type, out initializer))
            {
                return true;
            }
            else
            {
                if (entryAssembly != null)
                {
                    // If there is no default resolver, try to get a resolver type from the entry assembly
                    IEnumerable<Type> tagHandlerTypes = entryAssembly.GetTypesByAttribute<TagHandlerForAttribute>(true, attr => attr.Type == type)
                                                                     .Where(t => !t.IsInterface && !t.IsAbstract);
                    if (TryFindMatchingType(tagHandlerTypes, out initializer))
                    {
                        initializers.TryAdd(type, initializer);
                        return true;
                    }

                    if (referencedAssemblies != null)
                    {
                        // If we still don't have a resolver type, do the search in all referenced assemblies for a resolver type
                        IEnumerable<Type> refdTagHandlerTypes = referencedAssemblies.SelectMany(asm => asm.GetTypesByAttribute<TagHandlerForAttribute>(true, attr => attr.Type == type))
                                                                                    .Where(t => !t.IsInterface && !t.IsAbstract);
                        if (TryFindMatchingType(tagHandlerTypes, out initializer))
                        {
                            initializers.TryAdd(type, initializer);
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        public static bool Unregister(TagType type)
        {
            TagInitializationDelegate del;
            return initializers.TryRemove(type, out del);
        }

        private static bool TryFindMatchingType(IEnumerable<Type> tagHandlerTypes, out TagInitializationDelegate initializer)
        {
            Contract.Requires<ArgumentNullException>(tagHandlerTypes != null);
            Contract.Ensures(!Contract.Result<bool>() || Contract.ValueAtReturn(out initializer) != null);

            foreach (Type handlerType in tagHandlerTypes)
            {
                ConstructorInfo ci = handlerType.GetConstructor(constructorFinderArray);
                if (ci != null)
                {
                    initializer = Expression.Lambda<TagInitializationDelegate>(
                        Expression.New(ci, readerParamExpression),
                        readerParamExpression
                    ).Compile();

                    return true;
                }
            }

            initializer = null;
            return false;
        }

        private static bool TryGetDefaultType(TagType type, out TagInitializationDelegate initializer)
        {
            Contract.Ensures(!Contract.Result<bool>() || Contract.ValueAtReturn(out initializer) != null);

            switch (type)
            {
                case TagType.End:
                    initializer = rdr => EndTag.Default;
                    break;
                case TagType.Object:
                    initializer = rdr => new ObjectTag(rdr);
                    break;
                case TagType.Array:
                    initializer = rdr => new ArrayTag(rdr);
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
                case TagType.Date:
                    initializer = rdr => new DateTag(rdr);
                    break;
                case TagType.TimeSpan:
                    initializer = rdr => new TimeSpanTag(rdr);
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
