// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Compiler.Framework.RegisterAllocator;

public sealed class LiveInterval
{
	public enum AllocationStage
	{
		Initial = 0,
		SplitLevel1 = 1,
		SplitLevel2 = 2,
		SplitLevel3 = 3,
		SplitFinal = 4,
		Max = 5,
	}

	public readonly VirtualRegister VirtualRegister;

	public readonly SlotIndex Start;
	public readonly SlotIndex End;

	public readonly LiveRange LiveRange;

	public int Length => End - Start + 1;

	public int StartValue => LiveRange.Start.Index;

	public int EndValue => LiveRange.End.Index;

	public int SpillValue;

	public int SpillCost => NeverSpill || IsUnsplitable ? int.MaxValue : SpillValue / Length;

	public RegisterTrack LiveIntervalTrack;

	public AllocationStage Stage;

	public bool IsPhysicalRegister => VirtualRegister.IsPhysicalRegister;

	public PhysicalRegister AssignedPhysicalRegister => LiveIntervalTrack?.Register;

	public Operand AssignedPhysicalOperand;

	public Operand AssignedOperand => AssignedPhysicalRegister != null ? AssignedPhysicalOperand : VirtualRegister.SpillSlotOperand;

	public bool ForceSpill;

	public bool NeverSpill;

	public bool IsUnsplitable { get; }

	public LiveInterval(VirtualRegister virtualRegister, SlotIndex start, SlotIndex end)
	{
		Debug.Assert(start <= end);

		VirtualRegister = virtualRegister;
		Start = start;
		End = end;

		LiveRange = new LiveRange(start, end, virtualRegister);

		SpillValue = 0;
		Stage = AllocationStage.Initial;
		ForceSpill = false;
		NeverSpill = false;

		IsUnsplitable = Start == End;
	}

	#region Short Cuts

	public IEnumerable<SlotIndex> UsePositions => LiveRange.UsePositions;

	public IEnumerable<SlotIndex> DefPositions => LiveRange.DefPositions;

	public bool IsEmpty => LiveRange.IsEmpty;

	public SlotIndex First => LiveRange.First;

	public SlotIndex Last => LiveRange.Last;

	#endregion Short Cuts

	public bool Contains(SlotIndex at)
	{
		return at >= Start && at <= End;
	}

	public bool IsAdjacent(SlotIndex start, SlotIndex end)
	{
		return Start == end.Next || End == start.Previous;
	}

	public bool IsAdjacent(LiveInterval other)
	{
		return IsAdjacent(other.Start, other.End);
	}

	public bool Intersects(SlotIndex start, SlotIndex end)
	{
		return Contains(start) || Contains(end);
	}

	public bool Intersects(LiveInterval other)
	{
		return Intersects(other.Start, other.End);
	}

	public LiveInterval CreateExpandedLiveInterval(LiveInterval interval)
	{
		Debug.Assert(VirtualRegister == interval.VirtualRegister);

		var start = Start <= interval.Start ? Start : interval.Start;
		var end = End >= interval.End ? End : interval.End;

		return new LiveInterval(VirtualRegister, start, end);
	}

	public LiveInterval CreateExpandedLiveRange(SlotIndex start, SlotIndex end)
	{
		var mergedStart = Start <= start ? Start : start;
		var mergedEnd = End >= end ? End : end;

		return new LiveInterval(VirtualRegister, mergedStart, mergedEnd);
	}

	private LiveInterval CreateSplit(LiveRange liveRange)
	{
		return new LiveInterval(VirtualRegister, liveRange.Start, liveRange.End);
	}

	public override string ToString() => $"{VirtualRegister} between {LiveRange}";

	public void Evict() => LiveIntervalTrack.Evict(this);

	public List<LiveInterval> SplitAt(SlotIndex at)
	{
		var liveRanges = LiveRange.SplitAt(at);

		var intervals = new List<LiveInterval>(liveRanges.Count);

		foreach (var liveRange in liveRanges)
		{
			intervals.Add(CreateSplit(liveRange));
		}

		return intervals;
	}

	public List<LiveInterval> SplitAt(SlotIndex low, SlotIndex high)
	{
		var liveRanges = LiveRange.SplitAt(low, high);

		var intervals = new List<LiveInterval>(liveRanges.Count);

		foreach (var liveRange in liveRanges)
		{
			intervals.Add(CreateSplit(liveRange));
		}

		return intervals;
	}
}
