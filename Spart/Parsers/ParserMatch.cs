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
/// Author: Jonathan de Halleuxnamespace Spart.Parsers

namespace Spart.Parsers
{
    using System;
    using Spart.Scanners;

    /// <summary>
    /// A parser match
    /// </summary>
    public class ParserMatch
    {
        /// <summary>
        /// Scanner
        /// </summary>
        public virtual IScanner Scanner { get; protected set; }

        /// <summary>
        /// Offset
        /// </summary>
        public virtual long Offset { get; protected set; }

        /// <summary>
        /// Length
        /// </summary>
        public virtual int Length { get; protected set; }

        /// <summary>
        /// Extracts the match value
        /// </summary>
        public virtual string Value
        {
            get
            {
                if (Length < 0)
                    throw new Exception("no match");
                return this.Scanner.Substring(Offset, Length);
            }
        }

        /// <summary>
        /// True if match successfull
        /// </summary>
        public virtual bool Success
        {
            get
            {
                return this.Length >= 0;
            }
        }

        /// <summary>
        /// True if match empty
        /// </summary>
        public virtual bool Empty
        {
            get
            {
                if (Length < 0)
                    throw new Exception("no match");
                return Length == 0;
            }
        }

        /// <summary>
        /// Builds a new match
        /// </summary>
        /// <param name="scanner"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        public ParserMatch(IScanner scanner, long offset, int length)
        {
            this.Scanner = scanner ?? throw new ArgumentNullException(nameof(scanner));
            this.Offset = offset;
            this.Length = length;
        }

        /// <summary>
        /// Concatenates match with m
        /// </summary>
        /// <param name="m"></param>
        public virtual ParserMatch Concat(ParserMatch m)
        {
            if (m == null) throw new ArgumentNullException(nameof(m));

            if (!m.Success) throw new ArgumentException("Trying to concatenated non successful match");

            // if other is empty, return this
            if (!m.Empty)
            {
                if (m.Offset < Offset) throw new ArgumentException("match resersed ?");

                this.Length = (int)(m.Offset - Offset) + m.Length;
            }

            return this;
        }
    }
}