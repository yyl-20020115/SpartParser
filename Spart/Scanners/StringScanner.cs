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

namespace Spart.Scanners
{
    using System;
    using Spart.Parsers;

    /// <summary>
    /// Scanner acting on a string.
    /// <seealso cref="IScanner"/>
    /// </summary>
    public class StringScanner : IScanner
    {
        protected long offset = 0L;
        /// <summary>
        /// the input string
        /// </summary>
        public virtual string InputString { get; protected set; }
        /// <summary>
        /// Current offset
        /// </summary>
        public virtual long Offset
        {
            get => this.offset;
            set
            {
                if (value < 0 || value > InputString.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                this.offset = value;
            }
        }
        /// <summary>
        /// true if at the end of the string
        /// </summary>
        public virtual bool AtEnd
        {
            get
            {
                return this.Offset == this.InputString.Length;
            }
        }
        /// <summary>
        /// Current filter
        /// </summary>
        public virtual IFilter Filter { get; set; } = null;
        /// <summary>
        /// Failure match
        /// </summary>
        public virtual ParserMatch NoMatch
        {
            get
            {
                return new ParserMatch(this, 0, -1);
            }
        }

        /// <summary>
        /// Empty match
        /// </summary>
        public virtual ParserMatch EmptyMatch
        {
            get
            {
                return new ParserMatch(this, this.Offset, 0);
            }
        }

        /// <summary>
        /// Creates a scanner on the string at a specified offset
        /// </summary>
        /// <param name="inputString">Input string</param>
        /// <param name="offset">Offset</param>
        /// <exception cref="ArgumentNullException">input string is null</exception>
        /// <exception cref="ArgumentException">offset if out of range</exception>
        public StringScanner(string inputString, long offset = 0L)
        {
            this.InputString = inputString ?? throw new ArgumentNullException(nameof(inputString));

            if (offset < 0 || offset >= inputString.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            this.Offset = offset;
        }

        /// <summary>
        /// Advance the cursor once
        /// </summary>
        /// <returns>true if not at end</returns>
        public virtual bool Read()
        {
            if (!this.AtEnd)
            {
                this.Offset++;

                return !this.AtEnd;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Current character
        /// </summary>
        /// <returns>character at cursor position</returns>
        public virtual int Peek()
        {
			if (this.AtEnd)
			{
				return -1;
			}
			else
			{
				int x = this.InputString[(int)this.Offset];
				int z = x;

				if (char.IsHighSurrogate((char)x))
				{
					this.Offset++;

					if (this.AtEnd)
					{
						return -1;
					}
					else
					{
						int y = this.InputString[(int)this.Offset];

						if (char.IsLowSurrogate((char)y))
						{
							z = char.ConvertToUtf32((char)x, (char)y);
						}
					}
				}

				return this.Filter != null ? this.Filter.DoFilter(z) : z;

			}
        }

        /// <summary>
        /// Extracts a substring 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public virtual string Substring(long offset, int length)
        {
            if (offset < 0L || offset >= this.InputString.Length) throw new ArgumentOutOfRangeException(nameof(offset));
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));

            if ((offset + length) > this.InputString.Length) throw new ArgumentOutOfRangeException(nameof(offset) + "+" + nameof(length));

            string substring = this.InputString.Substring((int)offset,
                Math.Min(length, this.InputString.Length - (int)offset));

            if (this.Filter != null)
            {
                substring = this.Filter.DoFilter(substring);
            }

            return substring;
        }

        /// <summary>
        /// Moves the cursor to the offset position
        /// </summary>
        /// <param name="offset"></param>
        public virtual void Seek(long offset)
        {
            if (offset < 0L || offset > this.InputString.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            this.Offset = offset;
        }

        /// <summary>
        /// Creates a successful match
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public virtual ParserMatch CreateMatch(long offset, int length)
        {
            return new ParserMatch(this, offset, length);
        }
    }
}