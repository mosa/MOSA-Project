// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.RegisterAllocator;

public struct SlotIndex : IComparable<SlotIndex>
{
	public static SlotIndex Null = new(0);

	public readonly int Index;

	public readonly SlotIndex Previous => new(Index - 1);

	public readonly SlotIndex Next => new(Index + 1);

	public readonly bool IsOnSlot => (Index % 2) == 0;

	public readonly bool IsAfterSlot => (Index % 2) == 1;

	public readonly bool IsNull => Index == 0;

	public readonly bool IsNotNull => !IsNull;

	private SlotIndex(int index) => Index = index;

	public static SlotIndex Use(InstructionNode node) => new SlotIndex(node.Offset);

	public static SlotIndex Def(InstructionNode node) => new SlotIndex(node.Offset + 1);

	public SlotIndex(InstructionNode node)
		: this(node.Offset)
	{
	}

	public static bool operator ==(SlotIndex s1, SlotIndex s2) => s1.Index == s2.Index;

	public static bool operator !=(SlotIndex s1, SlotIndex s2) => s1.Index != s2.Index;

	public static bool operator >=(SlotIndex s1, SlotIndex s2) => s1.Index >= s2.Index;

	public static bool operator <=(SlotIndex s1, SlotIndex s2) => s1.Index <= s2.Index;

	public static bool operator >(SlotIndex s1, SlotIndex s2) => s1.Index > s2.Index;

	public static bool operator <(SlotIndex s1, SlotIndex s2) => s1.Index < s2.Index;

	public static int operator -(SlotIndex s1, SlotIndex s2) => s1.Index - s2.Index;

	public static SlotIndex operator ++(SlotIndex s) => new SlotIndex(s.Index + 1);

	public static SlotIndex operator --(SlotIndex s) => new SlotIndex(s.Index - 1);

	public readonly int CompareTo(SlotIndex other) => Index - other.Index;

	public static SlotIndex Max(SlotIndex a, SlotIndex b)
	{
		if (a.IsNull) return b;
		else if (b.IsNull) return a;
		else if (a == b) return a;

		return a > b ? a : b;
	}

	public static SlotIndex Min(SlotIndex a, SlotIndex b)
	{
		if (a.IsNull) return b;
		else if (b.IsNull) return a;
		else if (a == b) return a;

		return a < b ? a : b;
	}

	public static SlotIndex Max(SlotIndex a, SlotIndex b, SlotIndex c)
	{
		return Max(a, Max(b, c));
	}

	public static SlotIndex Min(SlotIndex a, SlotIndex b, SlotIndex c)
	{
		return Min(a, Min(b, c));
	}

	public SlotIndex GetNext()
	{
		return IsNull ? SlotIndex.Null : Next;
	}

	public SlotIndex GetPrevious()
	{
		return IsNull ? SlotIndex.Null : Previous;
	}

	public override readonly bool Equals(object obj)
	{
		if (obj == null)
			return false;

		return Index == ((SlotIndex)obj).Index;
	}

	public static SlotIndex Clamp(SlotIndex slot, SlotIndex start, SlotIndex end)
	{
		return slot >= start && slot <= end ? slot : SlotIndex.Null;
	}

	public readonly SlotIndex GetClamp(SlotIndex start, SlotIndex end)
	{
		return this >= start && this <= end ? this : SlotIndex.Null;
	}

	public override readonly int GetHashCode() => Index;

	public override string ToString() => $"{Index}";
}
