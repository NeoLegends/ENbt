using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENbt.ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().TestResolveOfUnknownType();

            Console.WriteLine("Finished tests.");
            Console.ReadLine();
        }

        public void TestResolveOfUnknownType()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                new UnknownTypeTag().WriteTo(ms);
                ms.Position = 0;

                UnknownTypeTag tag = Tag.ReadFrom<UnknownTypeTag>(ms);
            }
        }
    }

    [TagHandlerFor((TagType)26)]
    public class UnknownTypeTag : Tag
    {
        public override int PayloadLength
        {
            get
            {
                return 0;
            }
        }

        public override TagType Type
        {
            get
            {
                return (TagType)100;
            }
        }

        public UnknownTypeTag() { }

        public UnknownTypeTag(ENbtBinaryReader reader) 
        {
            Console.WriteLine(typeof(UnknownTypeTag).Name + " was initialized from the constructor with the reader!");
        }

        public override bool Equals(Tag other)
        {
            return (other is UnknownTypeTag);
        }

        public override void WritePayloadTo(ENbtBinaryWriter writer)
        {

        }
    }
}
