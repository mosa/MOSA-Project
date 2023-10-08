// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Compiler.Framework.RegisterAllocator;

public sealed class LiveRange
{
	private readonly VirtualRegister VirtualRegister;

	private readonly int StartIndex;
	private readonly int EndIndex;

	private readonly int FirstUseIndex;
	private readonly int LastUseIndex;

	private readonly int FirstDefIndex;
	private readonly int LastDefIndex;

	public readonly SlotIndex Start;
	public readonly SlotIndex End;

	public readonly bool IsEmpty;

	public readonly SlotIndex First;
	public readonly SlotIndex Last;

	public readonly SlotIndex FirstUse;
	public readonly SlotIndex FirstDef;

	public readonly SlotIndex LastUse;
	public readonly SlotIndex LastDef;

	public readonly int UseCount;

	public readonly int DefCount;

	public readonly bool IsDefFirst;

	public IEnumerable<SlotIndex> UsePositions
	{
		get
		{
			if (UseCount == 0)
				yield break;

			for (var i = FirstUseIndex; i <= LastUseIndex; i++)
			{
				yield return VirtualRegister.UsePositions[i];
			}
		}
	}

	public IEnumerable<SlotIndex> DefPositions
	{
		get
		{
			if (DefCount == 0)
				yield break;

			for (var i = FirstDefIndex; i <= LastDefIndex; i++)
			{
				yield return VirtualRegister.DefPositions[i];
			}
		}
	}

	public override string ToString() => $"({Start} to {End})";

	public LiveRange(SlotIndex start, SlotIndex end, VirtualRegister virtualRegister, int startIndex = 0, int endIndex = Int32.MaxValue)
	{
		VirtualRegister = virtualRegister;
		Start = start;
		End = end;

		if (virtualRegister.IsPhysicalRegister)
			return;

		var lastUseIndex = -1;
		var firstUseIndex = -1;
		var lastDefIndex = -1;
		var firstDefIndex = -1;
		var useCount = 0;
		var defCount = 0;

		for (var i = startIndex; i < virtualRegister.UsePositions.Count && i <= endIndex; i++)
		{
			var use = virtualRegister.UsePositions[i];

			if (use > end)
				break;

			if (use >= start)
			{
				if (firstUseIndex < 0)
				{
					firstUseIndex = i;
				}
				useCount++;
				lastUseIndex = i;
			}
		}

		for (var i = startIndex; i < virtualRegister.DefPositions.Count && i <= endIndex; i++)
		{
			var def = virtualRegister.DefPositions[i];

			if (def > end)
				break;

			if (def >= start)
			{
				if (firstDefIndex < 0)
				{
					firstDefIndex = i;
				}
				defCount++;
				lastDefIndex = i;
			}
		}

		StartIndex = LowestGreaterThanZero(firstUseIndex, firstDefIndex);
		EndIndex = Math.Max(lastUseIndex, lastDefIndex);

		FirstUseIndex = firstUseIndex;
		LastUseIndex = lastUseIndex;

		FirstDefIndex = firstDefIndex;
		LastDefIndex = lastDefIndex;

		UseCount = useCount;
		DefCount = defCount;

		IsEmpty = useCount + defCount == 0;

		FirstUse = useCount == 0 ? SlotIndex.Null : VirtualRegister.UsePositions[firstUseIndex];
		FirstDef = defCount == 0 ? SlotIndex.Null : VirtualRegister.DefPositions[firstDefIndex];
		LastUse = useCount == 0 ? SlotIndex.Null : VirtualRegister.UsePositions[lastUseIndex];
		LastDef = defCount == 0 ? SlotIndex.Null : VirtualRegister.DefPositions[lastDefIndex];

		First = SlotIndex.Min(FirstUse, FirstDef);
		Last = SlotIndex.Max(LastUse, LastDef);

		IsDefFirst = defCount != 0 && (useCount == 0 || FirstDefIndex < FirstUseIndex);
	}

	private static int LowestGreaterThanZero(int a, int b)
	{
		if (a < 0)
			return b;

		if (b < 0)
			return a;

		if (a > b)
			return b;
		else
			return a;
	}

	public bool ContainsUseAt(SlotIndex at)
	{
		if (!at.IsOnSlot)
			return false;

		if (UseCount == 0 || at < FirstUse || at > LastUse)
			return false;

		for (var i = FirstUseIndex; i <= LastUseIndex; i++)
		{
			var use = VirtualRegister.UsePositions[i];

			if (at == use)
				return true;

			if (use > at)   // list is sorted, so fast out
				return false;
		}

		return false;
	}

	public bool ContainsDefAt(SlotIndex at)
	{
		if (!at.IsOnSlot)
			return false;

		if (DefCount == 0 || at < FirstDef || at > LastDef)
			return false;

		for (var i = FirstDefIndex; i <= LastDefIndex; i++)
		{
			var def = VirtualRegister.DefPositions[i];

			if (at == def)
				return true;

			if (def > at)   // list is sorted, so fast out
				return false;
		}

		return false;
	}

	public SlotIndex GetNextUse(SlotIndex at)
	{
		if (UseCount == 0 || at < Start || at > LastUse) // || at > End
			return SlotIndex.Null;

		for (var i = FirstUseIndex; i <= LastUseIndex; i++)
		{
			var use = VirtualRegister.UsePositions[i];

			if (use > at)
				return use;
		}

		return SlotIndex.Null;
	}

	public SlotIndex GetNextDef(SlotIndex at)
	{
		if (DefCount == 0 || at < Start || at > LastDef) // || at > End
			return SlotIndex.Null;

		for (var i = FirstDefIndex; i <= LastDefIndex; i++)
		{
			var def = VirtualRegister.DefPositions[i];

			if (def > at)
				return def;
		}

		return SlotIndex.Null;
	}

	public SlotIndex GetPreviousUse(SlotIndex at)
	{
		if (UseCount == 0 || at < Start || at < FirstUse) // || at > End
			return SlotIndex.Null;

		for (var i = LastUseIndex; i >= FirstUseIndex; i--)
		{
			var use = VirtualRegister.UsePositions[i];

			if (use < at)
				return use;
		}

		return SlotIndex.Null;
	}

	public SlotIndex GetPreviousDef(SlotIndex at)
	{
		if (DefCount == 0 || at < Start || at < FirstDef)  // || at > End
			return SlotIndex.Null;

		for (var i = LastDefIndex; i >= FirstDefIndex; i--)
		{
			var def = VirtualRegister.DefPositions[i];

			if (def < at)
				return def;
		}

		return SlotIndex.Null;
	}

	public bool CanSplitAt(SlotIndex at)
	{
		if (at < Start || at > End)
			return false;

		return !ContainsUseAt(at) && !ContainsDefAt(at);
	}

	public bool CanSplitAt(SlotIndex low, SlotIndex high)
	{
		return CanSplitAt(low) && CanSplitAt(high);
	}

	public List<LiveRange> SplitAt(SlotIndex at)
	{
		Debug.Assert(CanSplitAt(at));

		// special case
		if (at == End)
		{
			return new List<LiveRange>(2)
			{
				new LiveRange(Start, at.Previous, VirtualRegister, StartIndex, EndIndex),
				new LiveRange(at, End, VirtualRegister, StartIndex, EndIndex)
			};
		}

		// normal case
		return new List<LiveRange>(2)
		{
			new LiveRange(Start, at, VirtualRegister, StartIndex, EndIndex),
			new LiveRange(at.Next, End, VirtualRegister, StartIndex, EndIndex)
		};
	}

	public List<LiveRange> SplitAt(SlotIndex low, SlotIndex high)
	{
		if (low == high)
		{
			return SplitAt(low);
		}

		Debug.Assert(CanSplitAt(low, high));

		return new List<LiveRange>(3)
		{
			new LiveRange(Start, low,  VirtualRegister, StartIndex, EndIndex),
			new LiveRange(low.Next, high,  VirtualRegister, StartIndex, EndIndex),
			new LiveRange(high.Next, End,  VirtualRegister, StartIndex, EndIndex)
		};
	}
}
