// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Common
{
	public sealed class SparseBitArray
	{
		private ulong[] Bits;

		private int Length { get { return Bits == null ? 0 : Bits.Length * 8; } }

		public SparseBitArray()
		{
		}

		public SparseBitArray(int size)
		{
			Debug.Assert(size >= 0);

			Bits = new ulong[(size + 63) / 64];
		}

		public bool Get(int index)
		{
			Debug.Assert(index >= 0);

			if (index >= Length)
				return false;

			return (Bits[index / 64] & (1ul << (index % 64))) != 0;
		}

		public void Reserve(int size)
		{
			Debug.Assert(size >= 0);
			Debug.Assert(size > Length || size == 0);

			if (size < 32)
			{
				size = 64;
			}

			if (Bits == null)
			{
				Bits = new ulong[(size + 63) / 64];
			}
			else
			{
				var newBits = new ulong[(size + 63) / 64];

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
				Bits[index / 64] |= (1u << (index % 64));
			}
			else

			{
				Bits[index / 64] &= ~(1u << (index % 64));
			}
		}

		public void SetAll(bool value)
		{
			for (int i = 0; i < Bits.Length; i++)
			{
				Bits[i] = value ? uint.MaxValue : 0u;
			}
		}

		public IEnumerator<int> GetEnumerator()
		{
			for (int i = 0; i < Length; i++)
			{
				if (Get(i))
				{
					yield return i;
				}
			}
		}
	}
}
