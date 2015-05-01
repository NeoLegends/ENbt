using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ENbt.Tests
{
    [TestClass]
    public class GeneralTests
    {
        [TestMethod]
        public async Task TestListSerialization()
        {
            ListTag list = Helpers.GenerateLongList();

            using (MemoryStream ms = new MemoryStream())
            using (FileStream fs = File.Create(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "List.nbt")))
            {
                list.WriteTo(ms);

                ms.Position = 0;
                await ms.CopyToAsync(fs);

                Console.WriteLine("Finished list serialization.");

                ms.Position = 0;
                Assert.AreEqual(list, Tag.ReadFrom<ListTag>(ms));
            }
        }

        [TestMethod]
        public async Task TestObjectSerialization()
        {
            ObjectTag objectTag = Helpers.GenerateObjectTag();

            using (MemoryStream ms = new MemoryStream())
            using (FileStream fs = File.Create(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Object.nbt")))
            {
                objectTag.WriteTo(ms);

                ms.Position = 0;
                await ms.CopyToAsync(fs);

                Console.WriteLine("Finished object serialization.");

                ms.Position = 0;
                Assert.IsTrue(objectTag.Equals(Tag.ReadFrom<ObjectTag>(ms)));
            }
        }

        [TestMethod]
        public async Task TestStringSerialization()
        {
            StringTag st = Helpers.GenerateLongStringTag();

            using (MemoryStream ms = new MemoryStream())
            using (FileStream fs = File.Create(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "String.nbt")))
            {
                st.WriteTo(ms);

                ms.Position = 0;
                await ms.CopyToAsync(fs);

                Console.WriteLine("Finished string serialization.");

                ms.Position = 0;
                Assert.AreEqual((string)st, (string)Tag.ReadFrom<StringTag>(ms));
            }
        }
    }
}
