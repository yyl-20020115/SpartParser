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
/// Author: Jonathan de Halleuxnamespace Spart.Actions

namespace Spart.Actions
{
    using System;
    using Spart.Parsers;

    /// <summary>
    /// Action event argument class
    /// </summary>
    public class ActionEventArgs : EventArgs
    {
        /// <summary>
        /// The parser match
        /// </summary>
        public virtual ParserMatch Match { get; protected set; }
        /// <summary>
        /// The parser match value
        /// </summary>
        public virtual string Value => this.Match?.Value;
        /// <summary>
        /// The typed parse result
        /// </summary>
        public virtual object Result { get; set; }
        /// <summary>
        /// Construct a new event argument instance
        /// </summary>
        /// <param name="match"></param>
        public ActionEventArgs(ParserMatch match)
            : this(match, null)
        {
        }
        /// <summary>
        /// Construct a new event argument instance
        /// </summary>
        /// <param name="match"></param>
        /// <param name="result"></param>
        public ActionEventArgs(ParserMatch match, object result)
        {
            this.Match = match ?? throw new ArgumentNullException(nameof(match));
            if (!match.Success)
            {
                this.Match = null;
                throw new ArgumentException(nameof(match) + " is not successfull");
            }
            this.Result = result;
        }
    }
}
