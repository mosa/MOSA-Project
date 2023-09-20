// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Compiler.Framework.RegisterAllocator;

public struct SlotIndex : IComparable<SlotIndex>
{
	public static SlotIndex NullSlot = new SlotIndex(0);

	public readonly int Value;

	public int Index => Value >> 2;

	public SlotIndex Before => new SlotIndex(this, false);

	public SlotIndex After => new SlotIndex(this, true);

	private SlotIndex(int index) => Value = (index << 2) | 0b01;

	public SlotIndex(InstructionNode node)
		: this(node.Offset)
	{
	}

	private SlotIndex(SlotIndex slot, bool after)
	{
		Debug.Assert(slot.IsOnSlot);

		Value = (slot.Value & ~0b11) | (after ? 0b11 : 0b00);
	}

	public bool IsBeforeSlot => (Value & 0b11) == 0b00;

	public bool IsOnSlot => (Value & 0b11) == 0b01;

	public bool IsAfterSlot => (Value & 0b11) == 0b11;

	public bool IsNull => Value == 0b01;

	public bool IsNotNull => !IsNull;

	public static bool operator ==(SlotIndex s1, SlotIndex s2) => s1.Value == s2.Value;

	public static bool operator !=(SlotIndex s1, SlotIndex s2) => s1.Value != s2.Value;

	public static bool operator >=(SlotIndex s1, SlotIndex s2) => s1.Value >= s2.Value;

	public static bool operator <=(SlotIndex s1, SlotIndex s2) => s1.Value <= s2.Value;

	public static bool operator >(SlotIndex s1, SlotIndex s2) => s1.Value > s2.Value;

	public static bool operator <(SlotIndex s1, SlotIndex s2) => s1.Value < s2.Value;

	public static int operator -(SlotIndex s1, SlotIndex s2) => s1.Index - s2.Index;

	public static SlotIndex operator ++(SlotIndex s) => new SlotIndex(s, true);

	public static SlotIndex operator --(SlotIndex s) => new SlotIndex(s, false);

	public readonly int CompareTo(SlotIndex other) => Value - other.Value;

	public override readonly bool Equals(object obj)
	{
		if (obj == null)
			return false;

		return Value == ((SlotIndex)obj).Value;
	}

	public override readonly int GetHashCode() => Value;

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
