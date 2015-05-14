using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENbt
{
    public enum TagType : byte
    {
        End,

        Object,

        Array,

        List,

        SByte,

        Byte,

        Int16,

        UInt16,

        Int32,

        UInt32,

        Int64,

        UInt64,

        Single,

        Double,

        String,

        Date,

        TimeSpan,

        ByteVector2,

        ByteVector3,

        ByteVector4,

        Int32Vector2,

        Int32Vector3,

        Int32Vector4,

        SingleVector2,

        SingleVector3,

        SingleVector4,

        DoubleVector2,

        DoubleVector3,

        DoubleVector4
    }
}
