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
/// Author: Jonathan de Halleuxnamespace Spart.Parsers.Primitives

namespace Spart.Parsers.Primitives
{
	using System;
	using Spart.Scanners;
	using Spart.Parsers.Primitives.Testers;

	public class CharParser : NegatableParser
	{
		public static implicit operator CharParser((int s, int e) range)
		{
			return Prims.Range(range.s, range.e);
		}
		public static implicit operator CharParser((char s, char e) range)
		{
			return Prims.Range(range.s, range.e);
		}

		public virtual ICharTester Tester { get; protected set; }

		public CharParser(ICharTester tester, string name = "") : base(name)
		{
			this.Tester = tester ?? throw new ArgumentNullException(nameof(tester));
		}

		public override ParserMatch ParseMain(IScanner scanner)
		{
			if (scanner == null) throw new ArgumentNullException(nameof(scanner));

			long offset = scanner.Offset;

			int c = scanner.Peek();

			if (c < 0)
			{
				return scanner.NoMatch;
			}

			bool test = this.Tester.Test(c);

			if (test && Negate || !test && !Negate)
			{
				return scanner.NoMatch;
			}

			// match character
			// if we arrive at this point, we have a match
			ParserMatch m = scanner.CreateMatch(offset, 1);

			// updating offset
			scanner.Read();

			// return match
			return m;
		}
	}
}
