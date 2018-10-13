// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Common
{
	public sealed class SparseBitArray
	{
		private int[] Bits;

		private int Length { get { return Bits == null ? 0 : Bits.Length * 4; } }

		public SparseBitArray()
		{
		}

		public SparseBitArray(int size)
		{
			Debug.Assert(size >= 0);

			Bits = new int[(size + 31) / 32];
		}

		public bool Get(int index)
		{
			Debug.Assert(index >= 0);

			if (index >= Length)
				return false;

			return (Bits[index / 32] & (1 << (index % 32))) != 0;
		}

		public void Reserve(int size)
		{
			Debug.Assert(size >= 0);
			Debug.Assert(size > Length || size == 0);

			if (size < 32)
			{
				size = 32;
			}

			if (Bits == null)
			{
				Bits = new int[(size + 31) / 32];
			}
			else
			{
				var newBits = new int[(size + 31) / 32];

				for (int i = 0; i < Bits.Length; i++)
				{
					newBits[i] = Bits[i];
				}

				Bits = newBits;
			}
		}

		public void Set(int index, bool value)
		{
			Debug.Assert(index >= 0);

			if (index >= Length)
			{
				if (!value)
					return; // unallocated bytes are "false" (0)

				Reserve(index);
			}

			if (value)
			{
				Bits[index / 32] |= (1 << (index % 32));
			}
			else

			{
				Bits[index / 32] &= ~(1 << (index % 32));
			}
		}

		public IEnumerator<int> GetEnumerator()
		{
			for (int i = 0; i < Length; i++)
			{
				if (Get(i))
					yield return i;
			}
		}
	}
}
