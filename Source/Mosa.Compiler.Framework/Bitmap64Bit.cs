/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// </summary>
	public struct Bitmap64Bit
	{
		#region Data members

		private ulong map;

		#endregion
		
		public void Set(Register register)
		{
			map |= ((ulong)1 << register.Index);
		}

		public void Clear(Register register)
		{
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
	}
}
