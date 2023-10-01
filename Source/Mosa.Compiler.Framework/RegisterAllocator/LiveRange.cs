// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.RegisterAllocator;

public sealed class LiveRange
{
	// Live ranges includes:
	// 1. Use > start && Use <= end
	// 2. Def >= start && Def < end

	// Notes:
	// Uses can not be at the start of a live range
	// Uses can end at the end of a live range
	// Defs can be at the start of a live range
	// Defs can not end at the end of a live range

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

		//Debug.Assert(InternalValidation());
	}

	private bool InternalValidation()
	{
		foreach (var use in UsePositions)
		{
			if (!ContainsUseAt(use))
				return false;

			if (UseCount == 0)
				return false;
		}

		foreach (var def in DefPositions)
		{
			if (!ContainsDefAt(def))
				return false;

			if (DefCount == 0)
				return false;
		}

		if (FirstUse.IsNotNull && !ContainsUseAt(FirstUse))
			return false;

		if (LastUse.IsNotNull && !ContainsUseAt(LastUse))
			return false;

		if (FirstDef.IsNotNull && !ContainsDefAt(FirstDef))
			return false;

		if (LastDef.IsNotNull && !ContainsDefAt(LastDef))
			return false;

		return true;
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

	public SlotIndex GetNextUsePosition(SlotIndex at)
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

	public SlotIndex GetNextDefPosition(SlotIndex at)
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

	public SlotIndex GetPreviousUsePosition(SlotIndex at)
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

	public SlotIndex GetPreviousDefPosition(SlotIndex at)
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
		if (at <= Start || at >= End)
			return false;

		return !ContainsUseAt(at) && !ContainsDefAt(at);
	}

	public bool CanSplitAt(SlotIndex low, SlotIndex high)
	{
		return CanSplitAt(low) && CanSplitAt(high);
	}

	public List<LiveRange> SplitAt(SlotIndex at)
	{
		//Debug.Assert(CanSplitAt(at));

		if (!CanSplitAt(at))
			throw new Exception($"Can not split at {at}");

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

		if (!CanSplitAt(low, high))
			throw new Exception($"Can not split at {low} and {high}");

		//Debug.Assert(CanSplitAt(low, high));

		return new List<LiveRange>(3)
		{
			new LiveRange(Start, low,  VirtualRegister, StartIndex, EndIndex),
			new LiveRange(low.Next, high.Previous,  VirtualRegister, StartIndex, EndIndex),
			new LiveRange(high, End,  VirtualRegister, StartIndex, EndIndex)
		};
	}
}
