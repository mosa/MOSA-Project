/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// </summary>
	public struct Bitmap64Bit : IEnumerable<int>
	{
		#region Data members

		private ulong map;

		#endregion

		public bool HasValue { get { return map != 0; } }

		public void Set(Register register)
		{
			if (register == null)
				return;

			map |= ((ulong)1 << register.Index);
		}

		public void Clear(Register register)
		{
			if (register == null)
				return;

			map &= ~((ulong)1 << register.Index);
		}

		public void ClearAll()
		{
			map = 0;
		}

		public void SetAll()
		{
			map = ~((ulong)0);
		}

		public void And(Bitmap64Bit bitmap64Bit)
		{
			map &= bitmap64Bit.map;
		}

		public void Or(Bitmap64Bit bitmap64Bit)
		{
			map |= bitmap64Bit.map;
		}

		public void Not()
		{
			map = ~map;
		}

		IEnumerator<int> IEnumerable<int>.GetEnumerator()
		{
			ulong value = map;

			for (int i = 0; i < 64; i++)
			{
				ulong result = value & 1;
				if (result != 0)
					yield return i;

				value = value >> 1;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<int>)this).GetEnumerator();
		}


	}
}
