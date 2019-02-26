// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	// FUTURE: For future use, see SlotIndex.cs
	public struct Slot
	{
		private readonly int value;

		public int Index { get { return value >> 2; } }

		public Slot(int index)
		{
			value = (index << 2) | 0b01;
		}

		private Slot(Slot slot, bool after)
		{
			Debug.Assert(slot.IsOnSlot);

			value = (slot.value & (~0b11)) | (after ? 0b11 : 0b00);
		}

		public Slot GetSlotAfter()
		{
			return new Slot(this, true);
		}

		public Slot GetSlotBefore()
		{
			return new Slot(this, false);
		}

		public bool IsBeforeSlot { get { return (value & 0b11) == 0b00; } }

		public bool IsOnSlot { get { return (value & 0b11) == 0b01; } }

		public bool IsAfterSlot { get { return (value & 0b11) == 0b11; } }

		public static bool operator ==(Slot s1, Slot s2)
		{
			return s1.value == s2.value;
		}

		public static bool operator !=(Slot s1, Slot s2)
		{
			return s1.value == s2.value;
		}

		public static bool operator >=(Slot s1, Slot s2)
		{
			return s1.value >= s2.value;
		}

		public static bool operator <=(Slot s1, Slot s2)
		{
			return s1.value <= s2.value;
		}

		public static bool operator >(Slot s1, Slot s2)
		{
			return s1.value > s2.value;
		}

		public static bool operator <(Slot s1, Slot s2)
		{
			return s1.value < s2.value;
		}

		public static int operator -(Slot s1, Slot s2)
		{
			return s1.Index - s2.Index;
		}

		public static Slot operator ++(Slot s)
		{
			return new Slot(s, true);
		}

		public static Slot operator --(Slot s)
		{
			return new Slot(s, false);
		}

		public override int GetHashCode()
		{
			return value;
		}
	}
}
