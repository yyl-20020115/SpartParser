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
/// Author: Jonathan de Halleuxnamespace Spart.Parsers.NonTerminal

namespace Spart.Parsers.NonTerminal
{
	using System;
	using Spart.Scanners;

	/// <summary>
	/// A rule is a parser holder.
	/// </summary>
	public class Rule : NonTerminalParser
	{
		/// <summary>
		/// Rule parser
		/// </summary>
		public virtual Parser Parser { get; set; }

		/// <summary>
		/// Empty rule creator
		/// </summary>
		public Rule(string name = "") : this(null, name) { }

		/// <summary>
		/// Creates a rule and assign parser
		/// </summary>
		/// <param name="p"></param>
		/// <param name="name"></param>
		public Rule(Parser p, string name = "")
			: base(name)
		{
			this.Parser = p;
		}

		/// <summary>
		/// Assign a parser to a rule, if r is null, a new rule is created
		/// </summary>
		/// <param name="rule"></param>
		/// <param name="parser"></param>
		/// <returns></returns>
		public static Rule AssignParser(Rule rule, Parser parser)
		{
			if (rule == null)
			{
				return new Rule(parser);
			}
			else
			{
				rule.Parser = parser;

				return rule;
			}
		}

		/// <summary>
		/// Parse the input
		/// </summary>
		/// <param name="scanner"></param>
		/// <returns></returns>
		public override ParserMatch Parse(IScanner scanner)
		{
			if (scanner == null) throw new ArgumentNullException(nameof(scanner));

			ParserMatch match = null;

			this.OnPreParse(scanner);
			{
				match = this.ParseMain(scanner);
			}
			this.OnPostParse(match, scanner);

			if (!match.Success)
			{
				return match;
			}

			return this.OnAction(match);
		}

		/// <summary>
		/// Inner parse method
		/// </summary>
		/// <param name="scanner"></param>
		/// <returns></returns>
		public override ParserMatch ParseMain(IScanner scanner)
		{
			if (scanner == null) throw new ArgumentNullException(nameof(scanner));

			return this.Parser != null ? this.Parser.Parse(scanner) : scanner.NoMatch;
		}
	}
}
