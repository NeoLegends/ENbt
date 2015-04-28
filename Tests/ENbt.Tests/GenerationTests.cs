using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ENbt;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ENbt.Tests
{
    [TestClass]
    public class GenerationTests
    {
        [TestMethod]
        public async Task TestListSpeed()
        {
            Random r = new Random();
            ListTag list = new ListTag(
                Enumerable.Range(0, 1500).Select(i => new Int32Tag(r.Next()))
            );

            using (MemoryStream ms = new MemoryStream())
            using (FileStream fs = File.Create(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "List.nbt")))
            {
                list.WriteTo(ms);
                ms.Position = 0;

                Stopwatch st = Stopwatch.StartNew();
                list.WriteTo(ms);
                TimeSpan elapsed = st.Elapsed;
                ms.Position = 0;

                await ms.CopyToAsync(fs);

                Console.WriteLine("Finished serialization in {0}ms.", elapsed.TotalMilliseconds);

                ms.Position = 0;
                ListTag deserializedList = Tag.ReadFrom<ListTag>(ms); // Read two times to prevent initialization of static methods obscuring the results
                ms.Position = 0;

                st.Restart();
                deserializedList = Tag.ReadFrom<ListTag>(ms);
                Console.WriteLine(string.Format("Finished deserialization in {0}ms.", st.Elapsed.TotalMilliseconds));
                Console.WriteLine("Deserialized list! It is " + ((deserializedList == list) ? "equal " : "not equal ") + "to the deserialized one.");
            }
        }

        [TestMethod]
        public void TestSpeed()
        {
            Tag tag = GenerateTag();
            int passes = 1000;

            using (MemoryStream ms = new MemoryStream())
            {
                Stopwatch st = Stopwatch.StartNew();
                TimeSpan elapsed = TimeSpan.Zero;

                for (int i = 0; i < passes; i++)
                {
                    st.Restart();
                    tag.WriteTo(ms);
                    elapsed += st.Elapsed;
                    ms.Position = 0;
                }

                Console.WriteLine("Took {0}ms! {1}ms on average.", elapsed.TotalMilliseconds, elapsed.TotalMilliseconds / passes);
            }
        }

        private static Tag GenerateTag()
        {
            return new ListTag(
                new ByteTag(125),
                new DateTag(DateTime.Now),
                new StringTag("Hello, World!"),
                new ObjectTag(
                    new Dictionary<string, Tag>() {  
                        { "TestString", new StringTag("Test string!") },
                        { "TestInt", new Int32Tag(150) }
                    }
                )
            );
        }
    }
}
