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
	public struct RegisterBitmap : IEnumerable<int>
	{
		#region Data members

		private ulong map;

		#endregion

		public RegisterBitmap(Register register)
		{
			map = ((ulong)1 << register.Index);
		}

		public RegisterBitmap(Register register1, Register register2)
		{
			map = ((ulong)1 << register1.Index) | ((ulong)1 << register2.Index);
		}

		public RegisterBitmap(Register register1, Register register2, Register register3)
		{
			map = ((ulong)1 << register1.Index) | ((ulong)1 << register2.Index) | ((ulong)1 << register3.Index);
		}

		public RegisterBitmap(Register register1, Register register2, Register register3, Register register4, Register register5, Register register6, Register register7)
		{
			map = ((ulong)1 << register1.Index) | ((ulong)1 << register2.Index) | ((ulong)1 << register3.Index) |
				((ulong)1 << register4.Index) | ((ulong)1 << register5.Index) | ((ulong)1 << register6.Index) |
				((ulong)1 << register7.Index);
		}

		public bool HasValue { get { return map != 0; } }

		public void Set(Register register)
		{
			if (register == null)
				return;

			map |= ((ulong)1 << register.Index);
		}

		public void Set(Register[] registers)
		{
			foreach (Register register in registers)
				Set(register);
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

		public void And(RegisterBitmap bitmap64Bit)
		{
			map &= bitmap64Bit.map;
		}

		public void Or(RegisterBitmap bitmap64Bit)
		{
			map |= bitmap64Bit.map;
		}

		public void Xor(RegisterBitmap bitmap64Bit)
		{
			map ^= bitmap64Bit.map;
		}

		public void Not()
		{
			map = ~map;
		}

		public bool IsSet(Register register)
		{
			ulong result = map & ((ulong)1 << register.Index);
			return (result != 0);
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

		public override string ToString()
		{
			return Convert.ToString((long)map, 2).PadLeft(64, '0');
		}
	}
}
