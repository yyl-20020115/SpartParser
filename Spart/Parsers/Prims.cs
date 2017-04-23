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

namespace Spart.Parsers
{
	using System;
	using Spart.Parsers.Primitives;
	using Spart.Parsers.Primitives.Testers;

	/// <summary>
	/// Static helper class to create primitive parsers
	/// </summary>
	public static class Prims
	{
        public static CharParser ToParser(this char c)
        {
            return Ch(c);
        }
        public static StringParser ToParser(this string str)
        {
            return Str(str);
        }
        /// <summary>
        /// Creates a parser that matches a single character
        /// </summary>
        /// <param name="c">character to match</param>
        /// <returns></returns>
        public static CharParser Ch(char c)
		{
			return new CharParser(new LitteralCharTester(c));
		}

		/// <summary>
		/// Creates a parser that matches a string
		/// </summary>
		/// <param name="s">string to match</param>
		/// <returns></returns>
		public static StringParser Str(string s)
		{
            if (s == null) throw new ArgumentNullException(nameof(s));

            return new StringParser(s);
		}

		/// <summary>
		/// Creates a parser that matches a range of character
		/// </summary>
		/// <param name="first"></param>
		/// <param name="last"></param>
		/// <returns></returns>
		public static CharParser Range(char first, char last)
		{
			return new CharParser(new RangeCharTester(first, last));
		}

        /// <summary>
        /// Creates a parser that matches any character
        /// </summary>
        public static CharParser AnyChar => new CharParser(new AnyCharTester());

        /// <summary>
        /// Creates a parser that matches control characters
        /// </summary>
        public static CharParser Control => new CharParser(new ControlCharTester());

        /// <summary>
        /// Creates a parser that matches digit characters
        /// </summary>
        public static CharParser Digit => new CharParser(new DigitCharTester());

        /// <summary>
        /// Creates a parser that matches letter characters
        /// </summary>
        public static CharParser Letter => new CharParser(new LetterCharTester());

        /// <summary>
        /// Creates a parser that matches letter or digit characters
        /// </summary>
        public static CharParser LetterOrDigit => new CharParser(new LetterOrDigitCharTester());

        /// <summary>
        /// Creates a parser that matches lower case characters
        /// </summary>
        public static CharParser Lower => new CharParser(new LowerCharTester());

        /// <summary>
        /// Creates a parser that matches punctuation characters
        /// </summary>
        public static CharParser Punctuation => new CharParser(new PunctuationCharTester());

        /// <summary>
        /// Creates a parser that matches separator characters
        /// </summary>
        public static CharParser Separator => new CharParser(new SeparatorCharTester());

        /// <summary>
        /// Creates a parser that matches symbol characters
        /// </summary>
        public static CharParser Symbol => new CharParser(new SymbolCharTester());

        /// <summary>
        /// Creates a parser that matches upper case characters
        /// </summary>
        public static CharParser Upper => new CharParser(new UpperCharTester());

        /// <summary>
        /// Creates a parser that matches whitespace characters
        /// </summary>
        public static CharParser WhiteSpace => new CharParser(new WhiteSpaceCharTester());

        /// <summary>
        /// Creates a parser that matches and end of line
        /// </summary>
        public static EolParser Eol => new EolParser();

        /// <summary>
        /// Creates a parser that matches the end of the input
        /// </summary>
        public static EndParser End => new EndParser();
    }
}
