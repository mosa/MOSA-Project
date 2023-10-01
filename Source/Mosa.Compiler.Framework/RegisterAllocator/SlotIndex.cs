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
		else if (b == a) return a;

		return a > b ? a : b;
	}

	public static SlotIndex Min(SlotIndex a, SlotIndex b)
	{
		if (a.IsNull) return b;
		else if (b.IsNull) return a;
		else if (b == a) return a;

		return a < b ? a : b;
	}

	public override readonly bool Equals(object obj)
	{
		if (obj == null)
			return false;

		return Index == ((SlotIndex)obj).Index;
	}

	public override readonly int GetHashCode() => Index;

	public override string ToString() => $"{Index}";
}
