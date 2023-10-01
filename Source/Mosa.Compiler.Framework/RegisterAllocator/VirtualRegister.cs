// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;

namespace Mosa.Compiler.Framework.RegisterAllocator;

public sealed class VirtualRegister
{
	public readonly List<SlotIndex> UsePositions;

	public readonly List<SlotIndex> DefPositions;

	public readonly Operand VirtualRegisterOperand;

	public readonly PhysicalRegister PhysicalRegister;

	public bool IsPhysicalRegister => VirtualRegisterOperand == null;

	public bool IsVirtualRegister => VirtualRegisterOperand != null;

	public List<LiveInterval> LiveIntervals { get; } = new List<LiveInterval>(1);

	public int Count => LiveIntervals.Count;

	public Operand SpillSlotOperand;

	public bool IsFloatingPoint => VirtualRegisterOperand.IsFloatingPoint;

	public bool IsReserved { get; }

	public bool IsSpilled;

	public bool IsUsed => Count != 0;

	public VirtualRegister(Operand virtualRegister)
	{
		VirtualRegisterOperand = virtualRegister;
		IsReserved = false;
		IsSpilled = false;

		if (!virtualRegister.IsVirtualRegister)
			return;

		UsePositions = new List<SlotIndex>(VirtualRegisterOperand.Uses.Count);
		DefPositions = new List<SlotIndex>(VirtualRegisterOperand.Definitions.Count);
	}

	public void UpdatePositions()
	{
		foreach (var use in VirtualRegisterOperand.Uses)
		{
			UsePositions.AddIfNew(SlotIndex.Use(use));
		}

		UsePositions.Sort();

		foreach (var def in VirtualRegisterOperand.Definitions)
		{
			DefPositions.AddIfNew(SlotIndex.Def(def));
		}

		DefPositions.Sort();
	}

	public VirtualRegister(PhysicalRegister physicalRegister, bool reserved)
	{
		PhysicalRegister = physicalRegister;
		IsReserved = reserved;
		IsSpilled = false;
	}

	public void Add(LiveInterval liveInterval)
	{
		LiveIntervals.Add(liveInterval);
	}

	public void Remove(LiveInterval liveInterval)
	{
		LiveIntervals.Remove(liveInterval);
	}

	public void AddLiveInterval(SlotIndex start, SlotIndex end)
	{
		if (LiveIntervals.Count == 0)
		{
			LiveIntervals.Add(new LiveInterval(this, start, end));
			return;
		}

		for (var i = 0; i < LiveIntervals.Count; i++)
		{
			var liveRange = LiveIntervals[i];

			if (liveRange.Start == start && liveRange.End == end)
				return;

			if (liveRange.IsAdjacent(start, end) || liveRange.Intersects(start, end))
			{
				liveRange = liveRange.CreateExpandedLiveRange(start, end);
				LiveIntervals[i] = liveRange;

				for (var z = i + 1; z < LiveIntervals.Count; z++)
				{
					var nextLiveRange = LiveIntervals[z];
					if (liveRange.IsAdjacent(nextLiveRange) || liveRange.Intersects(nextLiveRange))
					{
						liveRange = liveRange.CreateExpandedLiveInterval(nextLiveRange);
						LiveIntervals[i] = liveRange;
						LiveIntervals.RemoveAt(z);

						continue;
					}

					return;
				}

				return;
			}

			if (liveRange.Start > end)
			{
				// new range is before the current range (so insert before)
				LiveIntervals.Insert(i, new LiveInterval(this, start, end));
				return;
			}
		}

		// new range is after the last range
		LiveIntervals.Add(new LiveInterval(this, start, end));
	}

	/// <summary>
	/// Gets the interval at.
	/// </summary>
	/// <param name="at">At.</param>
	/// <returns></returns>
	public LiveInterval GetIntervalAt(SlotIndex at)
	{
		foreach (var liveInterval in LiveIntervals)
		{
			if (liveInterval.Contains(at))
				return liveInterval;
		}

		return null;
	}

	/// <summary>
	/// Gets the interval at or ends at.
	/// </summary>
	/// <param name="at">At.</param>
	/// <returns></returns>
	public LiveInterval GetIntervalAtOrEndsAt(SlotIndex at)
	{
		foreach (var liveInterval in LiveIntervals)
		{
			if (liveInterval.Contains(at) || at == liveInterval.End)
				return liveInterval;
		}

		return null;
	}

	public void ReplaceWithSplit(LiveInterval source, List<LiveInterval> liveIntervals)
	{
		Remove(source);

		foreach (var liveInterval in liveIntervals)
		{
			Add(liveInterval);
		}
	}

	public override string ToString()
	{
		if (IsPhysicalRegister)
		{
			return PhysicalRegister.ToString();
		}
		else
		{
			return $"v{VirtualRegisterOperand.Index}";
		}
	}
}
