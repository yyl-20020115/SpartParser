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

using System;

namespace Spart.Tests.Parsers.Composite
{
    using NUnit.Framework;
    using Spart.Parsers;
    using Spart.Parsers.Composite;
    using Spart.Scanners;

    [TestFixture]
    public class RepetionTest
    {
        public Parser Parser
        {
            get
            {
                return Prims.CharOf('a');
            }
        }

        [Test]
        public void Constructor()
        {
            Parser p = Parser;
            RepetitionParser rp = new RepetitionParser(p, 10, 20);
            Assert.AreEqual(rp.LowerBound, 10);
            Assert.AreEqual(rp.UpperBound, 20);
            Assert.AreEqual(rp.Parser, p);
        }

        [Test]
        public void Constructor2()
        {
            RepetitionParser rp = new RepetitionParser(null, 0, 1);
        }

        [Test]
        public void Constructor3()
        {
            RepetitionParser rp = new RepetitionParser(Parser, 1, 0);
        }

        [Test]
        public void PositiveSuccess1AtEnd()
        {
            RepetitionParser rp = +Parser;
            string s = "a";
            StringScanner scan = new StringScanner(s);
            ParserMatch m = rp.Parse(scan);
            Assert.IsTrue(m.Success);
            Assert.AreEqual(m.Length, 1);
            Assert.IsTrue(scan.AtEnd);
        }

        [Test]
        public void PositiveSuccess2AtEnd()
        {
            RepetitionParser rp = +Parser;
            string s = "aa";
            StringScanner scan = new StringScanner(s);
            ParserMatch m = rp.Parse(scan);
            Assert.IsTrue(m.Success);
            Assert.AreEqual(m.Length, 2);
            Assert.IsTrue(scan.AtEnd);
        }

        [Test]
        public void PositiveSuccessNotAtEnd()
        {
            RepetitionParser rp = +Parser;
            string s = "aaa ";
            StringScanner scan = new StringScanner(s);
            ParserMatch m = rp.Parse(scan);
            Assert.IsTrue(m.Success);
            Assert.AreEqual(m.Length, 3);
            Assert.IsTrue(!scan.AtEnd);
        }

        [Test]
        public void PositiveFailure()
        {
            RepetitionParser rp = +Parser;
            string s = "b";
            StringScanner scan = new StringScanner(s);
            ParserMatch m = rp.Parse(scan);
            Assert.IsTrue(!m.Success);
        }

        [Test]
        public void KleneeSuccess1AtEnd()
        {
            RepetitionParser rp = Ops.Klenee(Parser);
            string s = "a";
            StringScanner scan = new StringScanner(s);
            ParserMatch m = rp.Parse(scan);
            Assert.IsTrue(m.Success);
            Assert.AreEqual(m.Length, 1);
            Assert.IsTrue(scan.AtEnd);
        }

        [Test]
        public void KleneeSuccess2AtEnd()
        {
            RepetitionParser rp = Ops.Klenee(Parser);
            string s = "aa";
            StringScanner scan = new StringScanner(s);
            ParserMatch m = rp.Parse(scan);
            Assert.IsTrue(m.Success);
            Assert.AreEqual(m.Length, 2);
            Assert.IsTrue(scan.AtEnd);
        }

        [Test]
        public void KleneeSuccessNotAtEnd()
        {
            RepetitionParser rp = Ops.Klenee(Parser);
            string s = "aaa ";
            StringScanner scan = new StringScanner(s);
            ParserMatch m = rp.Parse(scan);
            Assert.IsTrue(m.Success);
            Assert.AreEqual(m.Length, 3);
            Assert.IsTrue(!scan.AtEnd);
        }

        [Test]
        public void KleneeSuccess0()
        {
            RepetitionParser rp = Ops.Klenee(Parser);
            string s = "b";
            StringScanner scan = new StringScanner(s);
            ParserMatch m = rp.Parse(scan);
            Assert.IsTrue(m.Success);
            Assert.IsTrue(m.Empty);
            Assert.IsTrue(!scan.AtEnd);
        }

        [Test]
        public void OptionalSuccess()
        {
            RepetitionParser rp = !Parser;
            string s = "aa";
            StringScanner scan = new StringScanner(s);
            ParserMatch m = rp.Parse(scan);
            Assert.IsTrue(m.Success);
            Assert.AreEqual(m.Length, 1);
            Assert.AreEqual(scan.Offset, 1);
        }

        [Test]
        public void OptionalSuccess0()
        {
            RepetitionParser rp = !Parser;
            string s = "";
            StringScanner scan = new StringScanner(s);
            ParserMatch m = rp.Parse(scan);
            Assert.IsTrue(m.Success);
            Assert.IsTrue(m.Empty);
        }
    }
}
