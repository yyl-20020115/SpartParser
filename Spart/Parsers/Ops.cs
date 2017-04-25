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
    using Spart.Parsers.Composite;
	using Spart.Actions;

	/// <summary>
	/// Static helper class to create new parser operators
	/// </summary>
	public static class Ops
    {
		public static Parser With(this char c, Action<Parser, ActionEventArgs> action)
		{
			return ((Parser)c).With(action);
		}

		public static Parser With(this (char h, char l) c, Action<Parser, ActionEventArgs> action)
		{
			return With(char.ConvertToUtf32(c.h,c.l),action);
		}

		public static Parser With(this int c, Action<Parser, ActionEventArgs> action)
		{
			return ((Parser)c).With(action);
		}

		public static Parser With(this string s, Action<Parser, ActionEventArgs> action)
		{
			return ((Parser)s).With(action);
		}

		/// <summary>
		/// &gt;&gt; operator
		/// </summary>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		public static SequenceParser Seq(Parser first, Parser second)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));

            return new SequenceParser(first, second);
        }

        /// <summary>
        /// * operators
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        public static RepetitionParser Klenee(Parser parser)
        {
			return Repeat(parser, 0);
		}

		/// <summary>
		/// ! operator
		/// </summary>
		/// <param name="parser"></param>
		/// <returns></returns>
		public static RepetitionParser Optional(Parser parser)
        {
			return Repeat(parser, 0, 1);
        }

		/// <summary>
		/// + operator
		/// </summary>
		/// <param name="parser"></param>
		/// <param name="mintimes"></param>
		/// <param name="maxtimes"></param>
		/// <returns></returns>
		public static RepetitionParser Repeat(Parser parser, uint mintimes = 1, uint maxtimes = uint.MaxValue)
		{
			if (parser == null) throw new ArgumentNullException(nameof(parser));

			return new RepetitionParser(parser, mintimes, maxtimes);
		}

		/// <summary>
		/// | operator
		/// </summary>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		public static AlternativeParser Alternative(Parser first, Parser second)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));

            return new AlternativeParser(first, second);
        }

        /// <summary>
        /// &amp; operator
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static IntersectionParser Intersection(Parser first, Parser second)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));

            return new IntersectionParser(first, second);
        }

        /// <summary>
        /// - operator
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static DifferenceParser Difference(Parser first, Parser second)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));

            return new DifferenceParser(first, second);
        }

        /// <summary>
        /// % operator
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static ListParser List(Parser first, Parser second)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));

            return new ListParser(first, second);
        }
    }
}
