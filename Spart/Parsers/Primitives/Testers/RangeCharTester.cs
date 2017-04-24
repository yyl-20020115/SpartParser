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

namespace Spart.Parsers.Primitives.Testers
{
	using System;

	public class RangeCharTester : ICharTester
	{
		public virtual int First { get; protected set; }

		public virtual int Last { get; protected set; }

		public RangeCharTester(int first, int last)
		{
			this.SetRange(first, last);
		}

		public virtual void SetRange(int first, int last)
		{
			if (first < 0) throw new ArgumentOutOfRangeException(nameof(first));
			if (last < 0) throw new ArgumentOutOfRangeException(nameof(last));

			if (last < first) throw new ArgumentException("last character < first character");

			this.First = first;
			this.Last = last;
		}

		public virtual bool Test(char c)
		{
			return c >= First && c <= Last;
		}

		public virtual bool Test(int c)
		{
			return c >= First && c <= Last;
		}

		public virtual bool Test(string s, int i)
		{
			if (s == null) throw new ArgumentNullException(nameof(s));
			if (i < 0 || i >= s.Length) throw new ArgumentOutOfRangeException(nameof(i));

			return this.Test(char.ConvertToUtf32(s, i));
		}
	}
}
