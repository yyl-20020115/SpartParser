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
/// Author: Jonathan de Halleuxnamespace Spart.Parsers.Composite

namespace Spart.Parsers.Composite
{
    using System;
    using Spart.Scanners;

    public class RepetitionParser : UnaryTerminalParser
    {
        public virtual uint LowerBound { get; protected set; } = 0;
        public virtual uint UpperBound { get; protected set; } = 0;
        public RepetitionParser(Parser parser, uint lowerBound, uint upperBound) : base(parser)
        {
            this.SetBounds(lowerBound, upperBound);
        }
        public virtual void SetBounds(uint lb, uint ub)
        {
            if (ub < lb)
                throw new ArgumentException("lower bound must be smaller than upper bound");
            this.LowerBound = lb;
            this.UpperBound = ub;
        }
        public override ParserMatch ParseMain(IScanner scanner)
        {
            if (scanner == null) throw new ArgumentNullException(nameof(scanner));

            // save scanner state
            long offset = scanner.Offset;

            ParserMatch m = scanner.EmptyMatch, t = null;

            // execution bound                                
            int count = 0;

            // lower bound, minimum number of executions
            while (count < LowerBound && !scanner.AtEnd)
            {
                t = Parser.Parse(scanner);

                // stop if not successful
                if (!t.Success)
                {
                    break;
                }

                // increment count and update full match
                ++count;
            }

            if (count == LowerBound)
            {
                while (count < UpperBound && !scanner.AtEnd)
                {
                    t = Parser.Parse(scanner);

                    // stop if not successful
                    if (!t.Success)
                    {
                        break;
                    }
                    // increment count
                    ++count;
                }
            }
            else
            {
                m = scanner.NoMatch;
            }

            if (m.Success)
            {
                m = scanner.CreateMatch(offset, count);
            }
            // restoring parser failed, rewind scanner
            if (!m.Success)
            {
                scanner.Seek(offset);
            }
            return m;
        }
    }
}
