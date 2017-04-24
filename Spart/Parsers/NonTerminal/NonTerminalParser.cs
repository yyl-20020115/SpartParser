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
	/// NonTerminal parser abstract class
	/// </summary>
	public abstract class NonTerminalParser : Parser
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public NonTerminalParser(string name = "") : base(name) { }

		/// <summary>
		/// Pre parse event
		/// </summary>
		public virtual event PreParseEventHandler PreParse;

		/// <summary>
		/// Post parse event 
		/// </summary>
		public virtual event PostParseEventHandler PostParse;

		/// <summary>
		/// Preparse event caller
		/// </summary>
		/// <param name="scanner"></param>
		public virtual void OnPreParse(IScanner scanner)
		{
			if (scanner == null) throw new ArgumentNullException(nameof(scanner));

			this.PreParse?.Invoke(this, new PreParseEventArgs(this, scanner));
		}

		/// <summary>
		/// Post parse event caller
		/// </summary>
		/// <param name="match"></param>
		/// <param name="scanner"></param>
		public virtual void OnPostParse(ParserMatch match, IScanner scanner)
		{
			if (match == null) throw new ArgumentNullException(nameof(match));

			if (scanner == null) throw new ArgumentNullException(nameof(scanner));

			this.PostParse?.Invoke(this, new PostParseEventArgs(match, this, scanner));
		}

		/// <summary>
		/// Adds event handlers
		/// </summary>
		/// <param name="context"></param>
		public virtual void AddContext(IParserContext context)
		{
			if (context == null) throw new ArgumentNullException(nameof(context));

			this.PreParse += new PreParseEventHandler(context.PreParse);
			this.PostParse += new PostParseEventHandler(context.PostParse);
		}

		/// <summary>
		/// Removes event handlers
		/// </summary>
		/// <param name="context"></param>
		public virtual void RemoveContext(IParserContext context)
		{
			if (context == null) throw new ArgumentNullException(nameof(context));

			this.PreParse -= new PreParseEventHandler(context.PreParse);
			this.PostParse -= new PostParseEventHandler(context.PostParse);
		}
	}
}
