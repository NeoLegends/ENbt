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

    [ContractClass(typeof(TagResolverContracts))]
    public abstract class TagResolver
    {
        private static readonly TagResolver _Default = new DefaultTagResolver();

        public static TagResolver Default
        {
            get
            {
                return _Default;
            }
        }

        public TagInitializationDelegate Resolve(TagType type)
        {
            Contract.Ensures(Contract.Result<TagInitializationDelegate>() != null);

            TagInitializationDelegate result;
            if (!TryResolve(type, out result))
            {
                throw new TagUnknownException(type, "Tag unknown!");
            }
            return result;
        }

        public abstract bool TryResolve(TagType type, out TagInitializationDelegate initializer);
    }

    [ContractClassFor(typeof(TagResolver))]
    abstract class TagResolverContracts : TagResolver
    {
        public override bool TryResolve(TagType type, out TagInitializationDelegate initializer)
        {
            Contract.Ensures(!Contract.Result<bool>() || Contract.ValueAtReturn(out initializer) != null);

            initializer = null;
            return false;
        }
    }
}
