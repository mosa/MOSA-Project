// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.RegisterAllocator;

/// <summary>
/// Greedy Register Allocator
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.RegisterAllocator.BaseRegisterAllocator" />
public sealed class GreedyRegisterAllocator : BaseRegisterAllocator
{
	private Dictionary<SlotIndex, MoveHint> moveHints;

	public GreedyRegisterAllocator(BasicBlocks basicBlocks, VirtualRegisters virtualRegisters, BaseArchitecture architecture, LocalStack localStack, Operand stackFrame, BaseMethodCompilerStage.CreateTraceHandler createTrace)
		: base(basicBlocks, virtualRegisters, architecture, localStack, stackFrame, createTrace)
	{
	}

	protected override void AdditionalSetup()
	{
		moveHints = new Dictionary<SlotIndex, MoveHint>();

		// Collect move locations
		CollectMoveHints();

		// Generate trace information for move hints
		TraceMoveHints();

		// Split intervals at call sites
		SplitIntervalsAtCallSites();
	}

	protected override void CalculateSpillCost(LiveInterval liveInterval)
	{
		var spillvalue = 0;

		foreach (var use in liveInterval.UsePositions)
		{
			spillvalue += GetSpillCost(use, 100);
		}

		foreach (var use in liveInterval.DefPositions)
		{
			spillvalue += GetSpillCost(use, 115);
		}

		liveInterval.SpillValue = spillvalue;
	}

	protected override int CalculatePriorityValue(LiveInterval liveInterval)
	{
		return liveInterval.Length | ((int)((int)LiveInterval.AllocationStage.Max - liveInterval.Stage) << 20);
	}

	private void SplitIntervalsAtCallSites()
	{
		foreach (var virtualRegister in VirtualRegisters)
		{
			if (virtualRegister.IsPhysicalRegister)
				continue;

			for (var i = 0; i < virtualRegister.LiveIntervals.Count; i++)
			{
				var liveInterval = virtualRegister.LiveIntervals[i];

				if (liveInterval.ForceSpill)
					continue;

				if (liveInterval.IsEmpty)
					continue;

				var callSite = FindKillAllSite(liveInterval);

				if (callSite.IsNull)
					continue;

				if (liveInterval.End == callSite)
					continue;

				SplitIntervalAtCallSite(liveInterval, callSite.Next);

				i = 0; // reset - list was modified
			}
		}
	}

	private bool PlaceLiveIntervalOnTrack(LiveInterval liveInterval, MoveHint a, MoveHint b)
	{
		if (a == null)
			return false;

		if (TryHint(liveInterval, a))
			return true;

		if (TryHint(liveInterval, b))
			return true;

		return false;
	}

	private bool TryHint(LiveInterval liveInterval, MoveHint moveHint)
	{
		if (moveHint == null)
			return false;

		var register = liveInterval.Start == moveHint.Slot ? moveHint.FromRegister : moveHint.ToRegister;

		if (register == null)
			return false;

		Trace?.Log($"  Trying move hint: {register}  [ {moveHint} ]");

		return TryPlaceLiveIntervalOnTrack(liveInterval, LiveIntervalTracks[register.Index]);
	}

	private (MoveHint a, MoveHint b) GetMoveHints(LiveInterval liveInterval)
	{
		moveHints.TryGetValue(liveInterval.Start, out var a);

		MoveHint b = null;

		if (!GetNode(liveInterval.End).IsBlockStartInstruction)
		{
			moveHints.TryGetValue(liveInterval.End, out b);
		}

		if (a != null && b != null)
		{
			// sorted by bonus
			if (a.Bonus > b.Bonus)
			{
				return (a, b);
			}
			else
			{
				return (b, a);
			}
		}

		return (a ?? b, null);
	}

	private void UpdateMoveHints(LiveInterval liveInterval, MoveHint a, MoveHint b)
	{
		if (a != null)
		{
			a.Update(liveInterval);
		}
		else if (b != null)
		{
			b.Update(liveInterval);
		}
	}

	protected override bool PlaceLiveInterval(LiveInterval liveInterval)
	{
		// For now, empty intervals will stay spilled
		if (liveInterval.IsEmpty)
		{
			Trace?.Log("  Spilled");

			liveInterval.VirtualRegister.IsSpilled = true;
			AddSpilledInterval(liveInterval);

			return true;
		}

		var moveHints = GetMoveHints(liveInterval);

		// Try to place using move hints first
		if (PlaceLiveIntervalOnTrack(liveInterval, moveHints.a, moveHints.b))
		{
			UpdateMoveHints(liveInterval, moveHints.a, moveHints.b);
			return true;
		}

		// TODO - try move hints first, allow evictions

		// Find any available track and place interval there
		if (PlaceLiveIntervalOnAnyAvailableTrack(liveInterval))
		{
			UpdateMoveHints(liveInterval, moveHints.a, moveHints.b);
			return true;
		}

		Trace?.Log("  No free register available");

		// No place for live interval; find live interval(s) to evict based on spill costs
		if (PlaceLiveIntervalOnTrackAllowEvictions(liveInterval))
		{
			UpdateMoveHints(liveInterval, moveHints.a, moveHints.b);
			return true;
		}

		return false;
	}

	private void SplitIntervalAtCallSite(LiveInterval liveInterval, SlotIndex callSite)
	{
		PreferBlockBoundaryIntervalSplit(liveInterval, callSite, false);
	}

	protected override bool TrySplitInterval(LiveInterval liveInterval, int level)
	{
		//if (level <= 1)
		//	return false;

		if (liveInterval.IsEmpty)
			return false;

		if (TrySimplePartialFreeIntervalSplit(liveInterval))
			return true;

		if (level <= 1)
			return false;

		//if (IntervalSplitAtFirstUseOrDef(liveInterval))
		//	return true;

		return false;
	}

	private bool PreferBlockBoundaryIntervalSplit(LiveInterval liveInterval, SlotIndex at, bool addToQueue)
	{
		var low = GetLowerOptimalSplitLocation(liveInterval, at);
		var high = GetUpperOptimalSplitLocation(liveInterval, at);

		List<LiveInterval> intervals;

		if (low.IsNull && high.IsNotNull)
		{
			if (!liveInterval.LiveRange.CanSplitAt(high))
				return false;

			intervals = liveInterval.SplitAt(high);
		}
		else if (high.IsNull && low.IsNotNull)
		{
			if (!liveInterval.LiveRange.CanSplitAt(low))
				return false;

			intervals = liveInterval.SplitAt(low);
		}
		else if (low.IsNotNull && high.IsNotNull)
		{
			if (!liveInterval.LiveRange.CanSplitAt(low, high))
				return false;

			intervals = liveInterval.SplitAt(low, high);
		}
		else
		{
			return false;
		}

		ReplaceIntervals(liveInterval, intervals, addToQueue);

		return true;
	}

	private bool TrySimplePartialFreeIntervalSplit(LiveInterval liveInterval)
	{
		var furthest = SlotIndex.Null;

		foreach (var track in LiveIntervalTracks)
		{
			if (track.IsReserved)
				continue;

			if (track.IsFloatingPoint != liveInterval.VirtualRegister.IsFloatingPoint)
				continue;

			if (track.Intersects(liveInterval.Start))
				continue;

			var intersections = track.GetIntersections(liveInterval);

			Debug.Assert(intersections.Count != 0);

			foreach (var interval in intersections)
			{
				var start = interval.Start;

				Trace?.Log($"  Register {track} free up to {start}");

				if (furthest.IsNull || start > furthest)
				{
					furthest = start;
				}
			}
		}

		if (furthest.IsNull)
		{
			Trace?.Log("  No partial free space available");
			return false;
		}

		if (furthest < liveInterval.First)
		{
			Trace?.Log("  No partial free space available");
			return false;
		}

		if (GetNode(furthest).IsBlockStartInstruction)
		{
			return false;
		}

		if (furthest <= liveInterval.Start)
		{
			Trace?.Log("  No partial free space available");
			return false;
		}

		Trace?.Log($"  Partial free up destination: {furthest}");

		if (liveInterval.LiveRange.ContainsUseAt(furthest))
		{
			var nextfurthest = furthest.Previous;

			if (liveInterval.Contains(nextfurthest))
			{
				furthest = nextfurthest;
			}
		}

		return PreferBlockBoundaryIntervalSplit(liveInterval, furthest, true);
	}

	private SlotIndex GetLowerOptimalSplitLocation(LiveInterval liveInterval, SlotIndex at)
	{
		Trace?.Log($"--Low Splitting: {liveInterval} at: {at}");

		var max = SlotIndex.Null;

		var blockStart = GetBlockStart(at);
		Trace?.Log($"   Block Start : {blockStart}");

		if (blockStart < at && blockStart > liveInterval.Start)
		{
			max = blockStart;
		}

		var prevUse = liveInterval.LiveRange.GetPreviousUse(at);
		Trace?.Log($"  Previous Use : {prevUse}");

		if (prevUse.IsNotNull && prevUse.Next < at && (max.IsNull || prevUse.Next > max))
		{
			max = prevUse.Next;
		}

		var prevDef = liveInterval.LiveRange.GetPreviousDef(at);
		Trace?.Log($"  Previous Def : {prevDef}");

		if (prevDef.IsNotNull && prevDef.Next < at && (max.IsNull || prevDef.Next > max))
		{
			max = prevDef.Next;
		}

		Trace?.Log($"   Low Optimal : {max}");

		return max;
	}

	private SlotIndex GetUpperOptimalSplitLocation(LiveInterval liveInterval, SlotIndex at)
	{
		Trace?.Log($"--High Splitting: {liveInterval} at: {at}");

		var min = SlotIndex.Null;

		var blockEnd = GetBlockEnd(at);
		Trace?.Log($"     Block End : {blockEnd}");

		if (blockEnd > at && blockEnd < liveInterval.End)
		{
			min = blockEnd;
		}

		var nextUse = liveInterval.LiveRange.GetNextUse(at);
		Trace?.Log($"      Next Use : {nextUse}");

		if (nextUse.IsNotNull && nextUse > at && (min.IsNull || nextUse < min))
		{
			min = nextUse;
		}

		var nextDef = liveInterval.LiveRange.GetNextDef(at);
		Trace?.Log($"      Next Def : {nextDef}");

		if (nextDef.IsNotNull && nextDef > at && (min.IsNull || nextDef < min))
		{
			min = nextDef;
		}

		Trace?.Log($"  High Optimal : {min}");

		return min;
	}

	private void CollectMoveHints()
	{
		foreach (var block in BasicBlocks)
		{
			for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.IsEmpty)
					continue;

				if (!Architecture.IsInstructionMove(node.Instruction))
					continue;

				if (!((node.Result.IsVirtualRegister && node.Operand1.IsVirtualRegister)
					  || (node.Result.IsVirtualRegister && node.Operand1.IsCPURegister)
					  || (node.Result.IsCPURegister && node.Operand1.IsVirtualRegister)))
				{
					continue;
				}

				var from = VirtualRegisters[GetIndex(node.Operand1)];
				var to = VirtualRegisters[GetIndex(node.Result)];

				var factor = (from.IsPhysicalRegister ? 5 : 1) + (to.IsPhysicalRegister ? 20 : 1);

				var bonus = 10 + (ExtendedBlocks[block.Sequence].LoopDepth * factor);

				if (from.IsPhysicalRegister)
					bonus += 2;

				if (to.IsPhysicalRegister)
					bonus += 4;

				var slot = new SlotIndex(node);

				moveHints.Add(slot, new MoveHint(slot, from, to, bonus));
			}
		}
	}

	private void TraceMoveHints()
	{
		var moveHintTrace = CreateTrace("MoveHints", 9);

		if (moveHintTrace == null)
			return;

		foreach (var moveHint in moveHints)
		{
			moveHintTrace.Log(moveHint.Value.ToString());
		}
	}
}
