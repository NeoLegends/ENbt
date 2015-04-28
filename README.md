# ENbt
A structured binary format loosely based on Minecraft's NBT.

## Differences to Minecraft NBT
1. In contrast to Minecraft NBT, ENbt (to which it is not binary-compatible) does not store the name
   within a tag. Instead, names of the items will be stored by the parent which contains the Tag.
   This simplifies the structure of the document tree, as there is no more decision to make in whether
   a Tag's name shall be written out or not. Objects simply write the name of the child to the output,
   lists don't. It also allows for greater flexibility, since you can associate the same value to 
   different keys without mutating the value itself.
2. ENbt does not require the tree root to be an object. You can use lists, objects, integers, floating 
   point numbers, dates, time ranges, vectors, or whatever object you desire as root. No restrictions!

## ENbt Specification

### General
ENbt Trees are somewhat like JSON. They consist of Tags, which represent either a value itself 
(like a byte, a date, or a vector), or contain other Tags like a list or a map. Lists store their elements 
based on an index, maps (or Objects, in ENbt notation) use names instead.

### Structure of a Tag
A tag always has the same, short structure. It only consists of the type of the tag and the payload. That 
is  

     <Tag Type>  <Payload>  
    |  1 byte  || n bytes |  
    
for a single tag.

The type of the tag has a very important role. Since the reader itself does not know what to do with the 
payload of a tag, it will read the tag and do a lookup for an instance that is capable of handling data of
that specified type. Since the lookup for the built-in types uses a switch-statement, it is very fast.
If a handler for the tag type cannot be found, the library will fall back to (more capable, albeit slow)
reflection. If that lookup failed as well, an exception will be thrown indicating that there was no parser
for data of the specified type.

----

The following tags are pre-defined in ENbt:

- `End = 0`, End marker of object
- `Object = 1`, A tag containing other tags by name
- `List = 2`, A tag containing other tags by index. Items may have different types.
- `SByte = 3`, A signed 8-bit integer
- `Byte = 4`, An unsigned 8-bit integer
- `Int16 = 5`, A signed 16-bit integer
- `UInt16 = 6`, An unsigned 16-bit integer
- `Int32 = 7`, A signed 32-bit integer
- `UInt32 = 8`, An unsigned 32-bit integer
- `Int64 = 9`, A signed 64-bit integer
- `UInt64 = 10`, An unsigned 64-bit integer
- `Single = 11`, IEEE 754 32-bit floating point (single accuracy)
- `Double = 12`, IEEE 754 64-bit floating point (double accuracy)
- `String = 13`, 32-bit length prefixed, UTF-8 encoded string
- `Date = 14`, Date and time store, internally stored as UNIX time in milliseconds (Int64)
- `TimeSpan = 15`, Duration store, internally stored as ticks, which represent 100 nanoseconds each (Int64)
- `ByteArray = 16`, A dedicated data type for an array of unsigned 8-bit integers, in order to reduce the overhead of List
- `ByteVector2 = 17`, A two-component, 8-bit integer vector
- `ByteVector3 = 18`, A three-component, 8-bit integer vector
- `ByteVector4 = 19`, A four-component, 8-bit integer vector
- `SingleVector2 = 20`, A two-component, single accuracy floating point vector
- `SingleVector3 = 21`, A three-component, single accuracy floating point vector
- `SingleVector4 = 22`, A four-component, single accuracy floating point vector
- `DoubleVector2 = 23`, A two-component, double accuracy floating point vector
- `DoubleVector3 = 24`, A three-component, double accuracy floating point vector
- `DoubleVector4 = 25`, A four-component, double accuracy floating point vector

In most cases, the payload is just what you expect of a binary format. So in case of a four-component
single accuracy vector, the payload consists of 128 bit data or four `Single`s (without delimeter) in order XYZW.

### Special Tags
Special tags are `Object`, `End` and `List`, since they are essential for representing a tree-like data 
structure like ENbt.

#### Object
An `Object` is an ENbt tag that works like a map. It assigns tags to names. It possesses the regular tag 
header which is followed by name / tag-pairs. A name / tag-pair consists of a `String`-tag directly 
followed by the named tag itself. The structure of a tag of type `Object` looks like this:

     Tag Type Object [ Tag Type String  String Length in Bytes  UTF-8 Encoded String Data  <Tag Type>  <Payload> ] Tag Type End
    |    1 byte     |[|    1 byte     ||       4 bytes        ||         n bytes         ||  1 byte  || n bytes |]|   1 byte   |
                     [                              as many times as there are children                          ]

The parser will continue to read name / tag-pairs until it has reached an `End`-tag. That marker marks the
 end of an object and is _always_ required.

#### End
An `End` tag is very simple. It consists just of the tag type (which is 0) without any payload. It's only
purpose is to mark the end of an `Object`.

#### List
A list contains a specified amount of elements. Specifically, `List`s in ENbt are length-prefixed, meaning
that the amount of items inside the list is written as a 32-bit integer before the items themselves are 
written. The structure of a `List` looks like this:

     Tag Type List  <List Length> [ <Tag Type>  <Payload> ]
    |   1 byte    ||   4 bytes   |[|  1 byte  || n bytes |]
                                  [    <Length> times     ]
