using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENbt.Tests
{
    internal static class Helpers
    {
        public static ListTag GenerateLongList()
        {
            Random r = new Random();
            return new ListTag(
                Enumerable.Range(0, 1500).Select(i => new Int32Tag(r.Next()))
            );
        }

        public static StringTag GenerateLongStringTag()
        {
            return "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.";
        }

        public static ObjectTag GenerateObjectTag()
        {
            byte[] arr = new byte[1024 * 4];
            new Random().NextBytes(arr);

            return new ObjectTag() 
            {
                { "TestByte", new ByteTag(125) },
                { "TestDate", new DateTag(DateTime.Now) },
                { "TestString1", new StringTag("Hello, World!") },
                { 
                    "TestObject", 
                    new ObjectTag() 
                    {  
                        { "TestString2", new StringTag("Test string!") },
                        { "TestInt", new Int32Tag(150) },
                        { "TestByteArray", new ByteArrayTag(arr) }
                    }
                }
            };
        }
    }
}
