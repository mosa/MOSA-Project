// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	public struct SlotIndex : IComparable<SlotIndex>
	{
		public static SlotIndex NullSlot = new SlotIndex(0);

		public readonly int Value;

		public int Index { get { return Value >> 2; } }

		public SlotIndex Before { get { return new SlotIndex(this, false); } }

		public SlotIndex After { get { return new SlotIndex(this, true); } }

		private SlotIndex(int index)
		{
			Value = (index << 2) | 0b01;
		}

		public SlotIndex(InstructionNode node)
			: this(node.Offset)
		{
		}

		private SlotIndex(SlotIndex slot, bool after)
		{
			Debug.Assert(slot.IsOnSlot);

			Value = (slot.Value & (~0b11)) | (after ? 0b11 : 0b00);
		}

		public bool IsBeforeSlot { get { return (Value & 0b11) == 0b00; } }

		public bool IsOnSlot { get { return (Value & 0b11) == 0b01; } }

		public bool IsAfterSlot { get { return (Value & 0b11) == 0b11; } }

		public bool IsNull { get { return Value == 0b01; } }

		public bool IsNotNull { get { return !IsNull; } }

		public static bool operator ==(SlotIndex s1, SlotIndex s2)
		{
			return s1.Value == s2.Value;
		}

		public static bool operator !=(SlotIndex s1, SlotIndex s2)
		{
			return s1.Value != s2.Value;
		}

		public static bool operator >=(SlotIndex s1, SlotIndex s2)
		{
			return s1.Value >= s2.Value;
		}

		public static bool operator <=(SlotIndex s1, SlotIndex s2)
		{
			return s1.Value <= s2.Value;
		}

		public static bool operator >(SlotIndex s1, SlotIndex s2)
		{
			return s1.Value > s2.Value;
		}

		public static bool operator <(SlotIndex s1, SlotIndex s2)
		{
			return s1.Value < s2.Value;
		}

		public static int operator -(SlotIndex s1, SlotIndex s2)
		{
			return s1.Index - s2.Index;
		}

		public static SlotIndex operator ++(SlotIndex s)
		{
			return new SlotIndex(s, true);
		}

		public static SlotIndex operator --(SlotIndex s)
		{
			return new SlotIndex(s, false);
		}

		public int CompareTo(SlotIndex other)
		{
			return Value - other.Value;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			return Value == ((SlotIndex)obj).Value;
		}

		public override int GetHashCode()
		{
			return Value;
		}

		public override string ToString()
		{
			if (IsNull)
				return "Null";

			if (IsBeforeSlot)
				return $"{Index}-";

			if (IsAfterSlot)
				return $"{Index}+";

			return $"{Index}";
		}
	}
}
