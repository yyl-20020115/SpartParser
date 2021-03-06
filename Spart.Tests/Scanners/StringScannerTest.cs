/// Spart License (zlib/png)
/// 
/// 
/// Copyright (c) 2003 Jonathan de Halleux
/// 
/// This software is provided 'as-is', without any express or implied warranty. 
/// In no event will the authors be held liable for any damages arising from 
/// the use of this software.
/// 
/// Permission is granted to anyone to use this software for any purpose, 
/// including commercial applications, and to alter it and redistribute it 
/// freely, subject to the following restrictions:
/// 
/// 1. The origin of this software must not be misrepresented; you must not 
/// claim that you wrote the original software. If you use this software in a 
/// product, an acknowledgment in the product documentation would be 
/// appreciated but is not required.
/// 
/// 2. Altered source versions must be plainly marked as such, and must not be 
/// misrepresented as being the original software.
/// 
/// 3. This notice may not be removed or altered from any source distribution.
/// 
/// Author: Jonathan de Halleux
/// 

namespace Spart.Tests.Scanners
{
    using Spart.Scanners;
    using Spart.Parsers;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class StringScannerTest
    {
        public string Text
        {
            get
            {
                return Provider.Text;
            }
        }

        public long Offset
        {
            get
            {
                return 5;
            }
        }

        [Test]
        public void Constructor()
        {
            StringScanner scanner = new StringScanner(Text);
            Assert.AreEqual(Text, scanner.InputString);
        }


        [Test]
        public void Constructor2()
        {
            StringScanner scanner = new StringScanner(Text, Offset);
            Assert.AreEqual(Text, scanner.InputString);
            Assert.AreEqual(Offset, scanner.Offset);
        }

        [Test]
        public void Substring()
        {
            StringScanner scanner = new StringScanner(Text, Offset);
            Assert.AreEqual(Text.Substring(3, 6), scanner.Substring(3, 6));
        }

        [Test]
        public void ReadAndPeek()
        {
            StringScanner scanner = new StringScanner(Text);
            int i = 0;

            while (!scanner.AtEnd)
            {
                Assert.IsTrue(i < Text.Length);
                Assert.AreEqual(scanner.Peek(), Text[i]);
                scanner.Read();
                ++i;
            }

            Assert.IsTrue(i == Text.Length);
        }

        [Test]
        public void ReadAndPeekOffset()
        {
            StringScanner scanner = new StringScanner(Text, Offset);
            int i = (int)Offset;

            while (!scanner.AtEnd)
            {
                Assert.IsTrue(i < Text.Length);
                Assert.AreEqual(scanner.Peek(), Text[i]);
                scanner.Read();
                ++i;
            }

            Assert.IsTrue(i == Text.Length);
        }

        [Test]
        public void Seek()
        {
            StringScanner scanner = new StringScanner(Text);
            int i = (int)Offset;
            scanner.Seek(Offset);

            while (!scanner.AtEnd)
            {
                Assert.IsTrue(i < Text.Length);
                Assert.AreEqual(scanner.Peek(), Text[i]);
                scanner.Read();
                ++i;
            }

            Assert.IsTrue(i == Text.Length);
        }

        [Test]
        public void NoMatch()
        {
            StringScanner scanner = new StringScanner(Text);
            ParserMatch m = scanner.NoMatch;
            Assert.IsTrue(!m.Success);
        }

        [Test]
        public void EmptyMatch()
        {
            StringScanner scanner = new StringScanner(Text);
            ParserMatch m = scanner.EmptyMatch;
            Assert.IsTrue(m.Success);
            Assert.IsTrue(m.Empty);
        }


        [Test]
        public void Match()
        {
            StringScanner scanner = new StringScanner(Text);
            ParserMatch m = scanner.CreateMatch(Offset, 2);
            Assert.IsTrue(m.Success);
            Assert.IsTrue(!m.Empty);
            Assert.AreEqual(m.Length, 2);

            Assert.AreEqual(m.Value, Text.Substring((int)Offset, 2));
        }

    }
}
