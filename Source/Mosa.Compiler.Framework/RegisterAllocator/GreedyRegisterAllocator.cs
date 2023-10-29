// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Compiler.Framework.RegisterAllocator;

/// <summary>
/// Greedy Register Allocator
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.RegisterAllocator.BaseRegisterAllocator" />
public sealed class GreedyRegisterAllocator : BaseRegisterAllocator
{
	public GreedyRegisterAllocator(Transform transform, Operand stackFrame, BaseMethodCompilerStage.CreateTraceHandler createTrace)
		: base(transform, stackFrame, createTrace)
	{
	}

	protected override void AdditionalSetup()
	{
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

	protected override bool PlaceLiveInterval(LiveInterval liveInterval)
	{
		// For now, empty intervals will stay spilled
		if (liveInterval.IsEmpty)
		{
			Trace?.Log("  Spilled");

			liveInterval.Register.IsSpilled = true;
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

	private bool PlaceLiveIntervalOnTrack(LiveInterval liveInterval, MoveHint a, MoveHint b)
	{
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

		PhysicalRegister register = null;

		if (moveHint.Slot == liveInterval.Start)
			register = moveHint.FromRegister;

		if (register == null && moveHint.Slot == liveInterval.End)
			register = moveHint.ToRegister;

		if (register == null && moveHint.Slot == liveInterval.Start)
			register = moveHint.ToRegister;

		if (register == null && moveHint.Slot == liveInterval.End)
			register = moveHint.FromRegister;

		if (register == null)
			return false;

		Trace?.Log($"  Trying move hint: {register}  [ {moveHint} ]");

		return TryPlaceLiveIntervalOnTrack(liveInterval, Tracks[register.Index]);
	}

	private (MoveHint a, MoveHint b) GetMoveHints(LiveInterval liveInterval)
	{
		MoveHints.TryGetValue(liveInterval.Start, out var a);

		MoveHint b = null;

		if (!GetNode(liveInterval.End).IsBlockStartInstruction)
		{
			MoveHints.TryGetValue(liveInterval.End, out b);
		}

		if (a == b)
		{
			return (a, null);
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
		else
		{
			b?.Update(liveInterval);
		}
	}

	private bool TrySimplePartialFreeIntervalSplit(LiveInterval liveInterval)
	{
		var furthest = SlotIndex.Null;

		foreach (var track in Tracks)
		{
			if (track.IsReserved)
				continue;

			if (track.IsFloatingPoint != liveInterval.Register.IsFloatingPoint)
				continue;

			if (track.Intersects(liveInterval.Start))
				continue;

			var intersections = track.GetIntersections(liveInterval);

			Debug.Assert(intersections.Count != 0);

			foreach (var interval in intersections)
			{
				var start = interval.Start;

				if (furthest.IsNull || start > furthest)
				{
					furthest = start;
				}
			}

			Trace?.Log($"  Register {track} free up to {furthest}");
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

		var low = FindSplit_LowerBoundary(liveInterval.LiveRange, furthest);
		var high = FindSplit_UpperBoundary(liveInterval.LiveRange, furthest);

		Trace?.Log($"   Low Split:  {low}");
		Trace?.Log($"   High Split: {high}");

		if (low.IsNull && high.IsNull)
			return false;

		if (high == liveInterval.End)
			high = SlotIndex.Null;

		if (low.IsNotNull && high.IsNotNull)
		{
			var newInternals = liveInterval.SplitAt(low, high);
			UpdateLiveIntervals(liveInterval, newInternals, true);
			return true;
		}
		else if (low.IsNotNull) // && high.IsNull
		{
			var newInternals = liveInterval.SplitAt(low);
			UpdateLiveIntervals(liveInterval, newInternals, true);
			return true;
		}
		else if (high.IsNotNull) // && low.IsNull
		{
			var newInternals = liveInterval.SplitAt(high);
			UpdateLiveIntervals(liveInterval, newInternals, true);
			return true;
		}

		return false;
	}

	protected override void CollectMoveHints()
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
					  || (node.Result.IsVirtualRegister && node.Operand1.IsPhysicalRegister)
					  || (node.Result.IsPhysicalRegister && node.Operand1.IsVirtualRegister)))
					continue;

				var from = Registers[GetIndex(node.Operand1)];
				var to = Registers[GetIndex(node.Result)];

				var factor = (from.IsPhysicalRegister ? 5 : 1) + (to.IsPhysicalRegister ? 20 : 1);

				var bonus = 10 + (ExtendedBlocks[block.Sequence].LoopDepth * factor);

				if (from.IsPhysicalRegister)
					bonus += 2;

				if (to.IsPhysicalRegister)
					bonus += 4;

				var slot = new SlotIndex(node);

				if (to.IsVirtualRegister)
					slot = slot.Next;

				MoveHints.Add(slot, new MoveHint(slot, from, to, bonus));
			}
		}
	}
}
