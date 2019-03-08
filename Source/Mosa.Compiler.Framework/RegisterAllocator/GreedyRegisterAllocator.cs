// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Trace;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	/// <summary>
	/// Greedy Register Allocator
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.RegisterAllocator.BasicRegisterAllocator" />
	public sealed class GreedyRegisterAllocator : BasicRegisterAllocator
	{
		private Dictionary<SlotIndex, MoveHint> moveHints;

		public GreedyRegisterAllocator(BasicBlocks basicBlocks, VirtualRegisters virtualRegisters, BaseArchitecture architecture, AddStackLocalDelegate addStackLocal, Operand stackFrame, ITraceFactory traceFactory)
			: base(basicBlocks, virtualRegisters, architecture, addStackLocal, stackFrame, traceFactory)
		{
		}

		protected override void PopulatePriorityQueue()
		{
			foreach (var virtualRegister in VirtualRegisters)
			{
				foreach (var liveInterval in virtualRegister.LiveIntervals)
				{
					// Skip adding live intervals for physical registers to priority queue
					if (liveInterval.VirtualRegister.IsPhysicalRegister)
					{
						LiveIntervalTracks[liveInterval.VirtualRegister.PhysicalRegister.Index].Add(liveInterval);
						continue;
					}

					liveInterval.Stage = LiveInterval.AllocationStage.Initial;

					ProcessLiveInterval(liveInterval);
				}
			}
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

		private bool PlaceLiveIntervalOnTrack(LiveInterval liveInterval, MoveHint[] hints)
		{
			if (hints == null)
				return false;

			foreach (var moveHint in hints)
			{
				var register = (liveInterval.StartSlot == moveHint.Slot) ? moveHint.FromRegister : moveHint.ToRegister;

				if (register == null)
					continue;   // no usable hint

				if (Trace.Active) Trace.Log($"  Trying move hint: {register}  [ {moveHint} ]");

				if (PlaceLiveIntervalOnTrack(liveInterval, LiveIntervalTracks[register.Index]))
				{
					return true;
				}
			}

			return false;
		}

		private MoveHint[] GetMoveHints(LiveInterval liveInterval)
		{
			MoveHint endMoveHint = null;
			moveHints.TryGetValue(liveInterval.StartSlot, out MoveHint startMoveHint);

			if (!GetNode(liveInterval.EndSlot).IsBlockStartInstruction)
			{
				moveHints.TryGetValue(liveInterval.EndSlot, out endMoveHint);
			}

			int cnt = (startMoveHint == null ? 0 : 1) + (endMoveHint == null ? 0 : 1);

			if (cnt == 0)
				return null;

			var hints = new MoveHint[cnt];

			if (startMoveHint != null && endMoveHint != null)
			{
				// sorted by bonus
				if (startMoveHint.Bonus > endMoveHint.Bonus)
				{
					hints[0] = startMoveHint;
					hints[1] = endMoveHint;
				}
				else
				{
					hints[0] = endMoveHint;
					hints[1] = startMoveHint;
				}
			}
			else
			{
				hints[0] = startMoveHint ?? endMoveHint;
			}

			return hints;
		}

		private void UpdateMoveHints(LiveInterval liveInterval, MoveHint[] moveHints)
		{
			if (moveHints == null)
				return;

			if (moveHints.Length >= 1)
			{
				moveHints[0].Update(liveInterval);
			}
			else if (moveHints.Length >= 2)
			{
				moveHints[1].Update(liveInterval);
			}
		}

		private void UpdateMoveHints(LiveInterval liveInterval)
		{
			if (moveHints.TryGetValue(liveInterval.StartSlot, out MoveHint MoveHint))
			{
				MoveHint.Update(liveInterval);
			}

			if (moveHints.TryGetValue(liveInterval.EndSlot, out MoveHint))
			{
				MoveHint.Update(liveInterval);
			}
		}

		protected override bool PlaceLiveInterval(LiveInterval liveInterval)
		{
			// For now, empty intervals will stay spilled
			if (liveInterval.IsEmpty)
			{
				if (Trace.Active) Trace.Log("  Spilled");

				liveInterval.VirtualRegister.IsSpilled = true;
				AddSpilledInterval(liveInterval);

				return true;
			}

			var moveHints = GetMoveHints(liveInterval);

			// Try to place using move hints first
			if (PlaceLiveIntervalOnTrack(liveInterval, moveHints))
			{
				UpdateMoveHints(liveInterval, moveHints);
				return true;
			}

			// TODO - try move hints first, allow evictions

			// Find any available track and place interval there
			if (PlaceLiveIntervalOnAnyAvailableTrack(liveInterval))
			{
				UpdateMoveHints(liveInterval, moveHints);
				return true;
			}

			if (Trace.Active) Trace.Log("  No free register available");

			// No place for live interval; find live interval(s) to evict based on spill costs
			if (PlaceLiveIntervalOnTrackAllowEvictions(liveInterval))
			{
				UpdateMoveHints(liveInterval, moveHints);
				return true;
			}

			return false;
		}

		protected override void SplitIntervalAtCallSite(LiveInterval liveInterval, SlotIndex callSite)
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

			if (liveInterval.StartSlot == low)
			{
				if (!liveInterval.LiveRange.CanSplitAt(high))
					return false;

				intervals = liveInterval.SplitAt(high);
			}
			else if (high == liveInterval.EndSlot)
			{
				if (!liveInterval.LiveRange.CanSplitAt(low))
					return false;

				intervals = liveInterval.SplitAt(low);
			}
			else
			{
				if (!liveInterval.LiveRange.CanSplitAt(low, high))
				{
					return false;
				}

				intervals = liveInterval.SplitAt(low, high);
			}

			ReplaceIntervals(liveInterval, intervals, addToQueue);

			return true;
		}

		private bool TrySimplePartialFreeIntervalSplit(LiveInterval liveInterval)
		{
			var furthest = SlotIndex.NullSlot;

			foreach (var track in LiveIntervalTracks)
			{
				if (track.IsReserved)
					continue;

				if (track.IsFloatingPoint != liveInterval.VirtualRegister.IsFloatingPoint)
					continue;

				if (track.Intersects(liveInterval.StartSlot))
					continue;

				var intersections = track.GetIntersections(liveInterval);

				Debug.Assert(intersections.Count != 0);

				foreach (var interval in intersections)
				{
					var start = interval.StartSlot;

					if (Trace.Active) Trace.Log($"  Register {track} free up to {start}");

					if (furthest.IsNull || start > furthest)
					{
						furthest = start;
					}
				}
			}

			if (furthest.IsNull)
			{
				if (Trace.Active) Trace.Log("  No partial free space available");
				return false;
			}

			if (furthest < liveInterval.Minimum)
			{
				if (Trace.Active) Trace.Log("  No partial free space available");
				return false;
			}

			if (GetNode(furthest).IsBlockStartInstruction)
			{
				return false;
			}

			if (furthest <= liveInterval.StartSlot)
			{
				if (Trace.Active) Trace.Log("  No partial free space available");
				return false;
			}

			if (Trace.Active) Trace.Log($"  Partial free up destination: {furthest}");

			if (liveInterval.UsePositions.Contains(furthest))
			{
				var nextfurthest = furthest.Before;

				if (liveInterval.Contains(nextfurthest))
				{
					furthest = nextfurthest;
				}
			}

			return PreferBlockBoundaryIntervalSplit(liveInterval, furthest, true);
		}

		private bool IntervalSplitAtFirstUseOrDef(LiveInterval liveInterval)
		{
			if (liveInterval.IsEmpty)
				return false;

			var at = SlotIndex.NullSlot;

			var firstUse = liveInterval.UsePositions.Count != 0 ? liveInterval.UsePositions[0] : SlotIndex.NullSlot;
			var firstDef = liveInterval.DefPositions.Count != 0 ? liveInterval.DefPositions[0] : SlotIndex.NullSlot;

			if (at.IsNull)
			{
				at = firstUse;
			}

			if (at.IsNotNull && firstDef.IsNotNull && firstDef < at)
			{
				at = firstDef;
			}

			if (at >= liveInterval.EndSlot)
				return false;

			if (at <= liveInterval.StartSlot)
				return false;

			if (Trace.Active) Trace.Log(" Splitting around first use/def");

			return PreferBlockBoundaryIntervalSplit(liveInterval, at, true);
		}

		private SlotIndex GetMaximum(SlotIndex a, SlotIndex b, SlotIndex c, SlotIndex d)
		{
			var max = a;

			if (max.IsNull || (b.IsNotNull && b > max))
				max = b;

			if (max.IsNull || (c.IsNotNull && c > max))
				max = c;

			if (max.IsNull || (d.IsNotNull && d > max))
				max = d;

			return max;
		}

		private SlotIndex GetMinimum(SlotIndex a, SlotIndex b, SlotIndex c, SlotIndex d)
		{
			var min = a;

			if (min.IsNull || (b.IsNotNull && b < min))
				min = b;

			if (min.IsNull || (c.IsNotNull && c < min))
				min = c;

			if (min.IsNull || (d.IsNotNull && d < min))
				min = d;

			return min;
		}

		private SlotIndex GetLowerOptimalSplitLocation(LiveInterval liveInterval, SlotIndex at)
		{
			if (Trace.Active) Trace.Log($"--Low Splitting: {liveInterval} move: {at}");

			var blockStart = GetBlockStart(at);
			var b = blockStart > liveInterval.StartSlot ? blockStart : SlotIndex.NullSlot;
			if (Trace.Active) Trace.Log($"   Block Start : {b}");

			var c = liveInterval.LiveRange.GetPreviousUsePosition(at);
			if (c.IsNotNull && c.After <= at)
			{
				c = c.After;
			}

			if (Trace.Active) Trace.Log($"  Previous Use : {c}");

			var d = liveInterval.LiveRange.GetPreviousDefPosition(at);
			if (d.IsNotNull && d.After <= at)
			{
				d = d.After;
			}

			if (Trace.Active) Trace.Log($"  Previous Def : {d}");

			var a = liveInterval.StartSlot;
			var max = GetMaximum(a, b, c, d);

			if (Trace.Active) Trace.Log($"   Low Optimal : {max}");

			return max;
		}

		private SlotIndex GetUpperOptimalSplitLocation(LiveInterval liveInterval, SlotIndex at)
		{
			if (Trace.Active) Trace.Log($"--High Splitting: {liveInterval} move: {at}");

			var a = liveInterval.EndSlot;

			var blockEnd = GetBlockEnd(at);
			var b = blockEnd > liveInterval.EndSlot ? blockEnd : SlotIndex.NullSlot;

			if (Trace.Active) Trace.Log($"     Block End : {b}");

			var c = liveInterval.LiveRange.GetNextUsePosition(at);
			if (c.IsNotNull && c.Before > at)
			{
				c = c.Before;
			}

			if (Trace.Active) Trace.Log($"      Next Use : {c}");

			var d = liveInterval.LiveRange.GetNextDefPosition(at);
			if (Trace.Active) Trace.Log($"      Next Def : {d}");

			var min = GetMinimum(a, b, c, d);

			if (Trace.Active) Trace.Log($"  High Optimal : {min}");

			return min;
		}

		private void CollectMoveHints()
		{
			foreach (var block in BasicBlocks)
			{
				for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty || node.IsBlockEndInstruction)
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

					int factor = (from.IsPhysicalRegister || to.IsPhysicalRegister) ? 150 : 125;

					int bonus = ExtendedBlocks[block.Sequence].LoopDepth;

					var slot = new SlotIndex(node);

					moveHints.Add(slot, new MoveHint(slot, from, to, bonus));
				}
			}
		}

		private void TraceMoveHints()
		{
			var moveHintTrace = CreateTraceLog("MoveHints");

			if (!moveHintTrace.Active)
				return;

			foreach (var moveHint in moveHints)
			{
				moveHintTrace.Log(moveHint.Value.ToString());
			}
		}
	}
}
