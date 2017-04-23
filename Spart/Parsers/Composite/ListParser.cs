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
/// Author: Jonathan de Halleuxusing System;

namespace Spart.Parsers.Composite
{
    using Scanners;
    using System;

    public class ListParser : BinaryTerminalParser
    {
        public ListParser(Parser first, Parser second) : base(first, second) { }
        public override ParserMatch ParseMain(IScanner scanner)
        {
            if (scanner == null) throw new ArgumentNullException(nameof(scanner));

            long offset = scanner.Offset;

            ParserMatch a = null, b = null;

            ParserMatch m = this.FirstParser.Parse(scanner);

            if (!m.Success)
            {
                scanner.Seek(offset);

                return scanner.NoMatch;
            }

            while (!scanner.AtEnd)
            {
                offset = scanner.Offset;

                b = this.SecondParser.Parse(scanner);

                if (!b.Success)
                {
                    scanner.Seek(offset);

                    return m;
                }

                a = this.FirstParser.Parse(scanner);

                if (!a.Success)
                {
                    scanner.Seek(offset);

                    return m;
                }

                m.Concat(b);

                m.Concat(a);
            }

            return m;
        }
    }
}
