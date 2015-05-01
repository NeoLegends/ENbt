using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ENbt.Tests
{
    [TestClass]
    public class SpeedTests
    {
        private const int passes = 100;

        [TestMethod]
        public void TestListSpeed()
        {
            ListTag list = Helpers.GenerateLongList();

            using (MemoryStream ms = new MemoryStream())
            {
                list.WriteTo(ms);
                ms.Position = 0;
                Tag.ReadFrom(ms);

                this.TestTagSpeed(ms, list);
            }
        }

        [TestMethod]
        public void TestObjectSpeed()
        {
            ObjectTag objectTag = Helpers.GenerateObjectTag();

            using (MemoryStream ms = new MemoryStream())
            {
                objectTag.WriteTo(ms);
                ms.Position = 0;
                Tag.ReadFrom(ms);

                this.TestTagSpeed(ms, objectTag);
            }
        }

        private void TestTagSpeed(Stream s, Tag tag)
        {
            Contract.Requires<ArgumentNullException>(s != null);
            Contract.Requires<ArgumentException>(s.CanRead && s.CanSeek && s.CanWrite);

            TimeSpan totalElapsed = TimeSpan.Zero;
            Stopwatch st = Stopwatch.StartNew();
            for (int i = 0; i < passes; i++)
            {
                s.Position = 0;
                st.Restart();
                tag.WriteTo(s);
                totalElapsed += st.Elapsed;
            }

            Console.WriteLine("Finished serialization in {0}ms. Thats {1}ms per pass.", totalElapsed.TotalMilliseconds, totalElapsed.TotalMilliseconds / passes);
            totalElapsed = TimeSpan.Zero;

            for (int i = 0; i < passes; i++)
            {
                s.Position = 0;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                st.Restart();
                Tag t = Tag.ReadFrom(s);
                totalElapsed += st.Elapsed;
            }

            Console.WriteLine("Finished deserialization in {0}ms. Thats {1}ms per pass.", totalElapsed.TotalMilliseconds, totalElapsed.TotalMilliseconds / passes);
        }
    }
}
