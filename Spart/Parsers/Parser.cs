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
    using Spart.Scanners;
    using Spart.Actions;
    using Spart.Parsers.Composite;
    using System;

    /// <summary>
    /// Abstract parser class
    /// </summary>
    public abstract class Parser
    {
		public virtual string Name { get; set; }
		/// <summary>
		/// Default constructor
		/// </summary>
		public Parser(string name = "")
		{
			if(string.IsNullOrEmpty(this.Name = name))
			{
				this.Name = this.GetType().Name + string.Format("_{0:X8}", this.GetHashCode());
			}
		}

		/// <summary>
		/// Inner parse method
		/// </summary>
		/// <param name="scanner">scanner</param>
		/// <returns>the match</returns>
		public abstract ParserMatch ParseMain(IScanner scanner);

        /// <summary>
        /// Outer parse method
        /// </summary>
        /// <param name="scanner"></param>
        /// <returns></returns>
        public virtual ParserMatch Parse(IScanner scanner)
        {
            if (scanner == null) throw new ArgumentNullException(nameof(scanner));

            ParserMatch m = this.ParseMain(scanner);

            if (m.Success)
            {
                this.OnAction(m);
            }
            return m;
        }

        /// <summary>
        /// Action event
        /// </summary>
        public virtual event ActionHandler Action;

		public virtual Parser WithAction(Action<Parser, ActionEventArgs> action)
		{
			if (action != null)
			{
				this.Action += new ActionHandler(action);
			}
			return this;
		}
        /// <summary>
        /// Action caller method
        /// </summary>
        /// <param name="m"></param>
        public virtual ParserMatch OnAction(ParserMatch m)
        {
            if (m == null) throw new ArgumentNullException(nameof(m));

            this.Action?.Invoke(this, new ActionEventArgs(m));

            return m;
        }
		public override string ToString()
		{
			return this.Name;
		}
		#region Operators
		public static Parser operator +(Parser first, Parser second)
        {
            return Ops.Seq(first, second);
        }
        public static Parser operator ~(Parser p)
        {
            return Ops.Klenee(p);
        }

		public static implicit operator Parser(char c)
		{
			return Prims.CharOf(c);
		}
		public static implicit operator Parser(int c)
		{
			return Prims.CharOf(c);
		}
		public static implicit operator Parser(string str)
        {
            return Prims.StringOf(str);
        }

        /// <summary>
        /// Positive operator
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static RepetitionParser operator +(Parser p)
        {
            return Ops.OnePlus(p);
        }

        /// <summary>
        /// Optional operator
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static RepetitionParser operator !(Parser p)
        {
            return Ops.Optional(p);
        }

        /// <summary>
        /// Alternative operator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static AlternativeParser operator |(Parser left, Parser right)
        {
            return Ops.Alternative(left, right);
        }

        /// <summary>
        /// Intersection operator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IntersectionParser operator &(Parser left, Parser right)
        {
            return Ops.Intersection(left, right);
        }

        /// <summary>
        /// Difference operator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static DifferenceParser operator -(Parser left, Parser right)
        {
            return Ops.Difference(left, right);
        }

        /// <summary>
        /// List operator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static ListParser operator %(Parser left, Parser right)
        {
            return Ops.List(left, right);
        }

        #endregion
    }
}
