/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Trace;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	/// <summary>
	///
	/// </summary>
	public sealed class GreedyRegisterAllocator : BasicRegisterAllocator
	{
		private Dictionary<SlotIndex, MoveHint> moveHints;

		public GreedyRegisterAllocator(BasicBlocks basicBlocks, VirtualRegisters compilerVirtualRegisters, InstructionSet instructionSet, StackLayout stackLayout, BaseArchitecture architecture, ITraceFactory traceFactory)
			: base(basicBlocks, compilerVirtualRegisters, instructionSet, stackLayout, architecture, traceFactory)
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

		private int GetSpillCost(SlotIndex use, int factor)
		{
			return factor * GetLoopDepth(use) * 100;
		}

		protected override void CalculateSpillCost(LiveInterval liveInterval)
		{
			int spillvalue = 0;

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
			return liveInterval.Length | ((int)(((int)LiveInterval.AllocationStage.Max - liveInterval.Stage)) << 28);
		}

		private bool PlaceLiveIntervalOnTrack(LiveInterval liveInterval, MoveHint[] hints)
		{
			if (hints == null)
				return false;

			foreach (var moveHint in hints)
			{
				LiveIntervalTrack track = null;

				var register = (liveInterval.Start == moveHint.Slot) ? moveHint.FromRegister : moveHint.ToRegister;

				if (register == null)
					continue;	// no usable hint

				if (Trace.Active) Trace.Log("  Trying move hint: " + register.ToString() + "  [ " + moveHint.ToString() + " ]");

				if (PlaceLiveIntervalOnTrack(liveInterval, LiveIntervalTracks[register.Index]))
				{
					return true;
				}
			}

			return false;
		}

		private MoveHint[] GetMoveHints(LiveInterval liveInterval)
		{
			MoveHint startMoveHint = null;
			MoveHint endMoveHint = null;
			moveHints.TryGetValue(liveInterval.Start, out startMoveHint);

			if (!liveInterval.End.IsBlockStartInstruction)
				moveHints.TryGetValue(liveInterval.End, out endMoveHint);

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
				if (startMoveHint != null)
				{
					hints[0] = startMoveHint;
				}
				else
				{
					hints[0] = endMoveHint;
				}
			}

			return hints;
		}

		private void UpdateMoveHints(LiveInterval liveInterval, MoveHint[] moveHints)
		{
			if (moveHints == null)
				return;

			if (moveHints.Length >= 1)
				moveHints[0].Update(liveInterval);
			else
				if (moveHints.Length >= 2)
					moveHints[1].Update(liveInterval);
		}

		private void UpdateMoveHints(LiveInterval liveInterval)
		{
			MoveHint MoveHint = null;

			if (moveHints.TryGetValue(liveInterval.Start, out MoveHint))
			{
				MoveHint.Update(liveInterval);
			}

			if (moveHints.TryGetValue(liveInterval.End, out MoveHint))
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

			IList<LiveInterval> intervals;

			if (liveInterval.Start == low)
			{
				if (!liveInterval.LiveRange.CanSplitAt(high))
					return false;

				intervals = liveInterval.SplitAt(high);
			}
			else if (high == liveInterval.End)
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
			SlotIndex furthestUsed = null;

			foreach (var track in LiveIntervalTracks)
			{
				if (track.IsReserved)
					continue;

				if (track.IsFloatingPoint != liveInterval.VirtualRegister.IsFloatingPoint)
					continue;

				var next = track.GetNextLiveRange(liveInterval.Start);

				if (next == null)
					continue;

				Debug.Assert(next > liveInterval.Start);

				if (Trace.Active) Trace.Log("  Register " + track.ToString() + " free up to " + next.ToString());

				if (furthestUsed == null || furthestUsed < next)
					furthestUsed = next;
			}

			if (furthestUsed == null)
			{
				if (Trace.Active) Trace.Log("  No partial free space available");
				return false;
			}

			if (furthestUsed < liveInterval.Minimum)
			{
				if (Trace.Active) Trace.Log("  No partial free space available");
				return false;
			}

			if (furthestUsed.IsBlockStartInstruction)
			{
				return false;
			}

			if (furthestUsed <= liveInterval.Start)
			{
				if (Trace.Active) Trace.Log("  No partial free space available");
				return false;
			}

			if (Trace.Active) Trace.Log("  Partial free up destination: " + furthestUsed.ToString());

			if (liveInterval.UsePositions.Contains(furthestUsed) && liveInterval.Contains(furthestUsed.HalfStepBack))
				furthestUsed = furthestUsed.HalfStepBack;

			return PreferBlockBoundaryIntervalSplit(liveInterval, furthestUsed, true);
		}

		private bool IntervalSplitAtFirstUseOrDef(LiveInterval liveInterval)
		{
			if (liveInterval.IsEmpty)
				return false;

			SlotIndex at = null;

			var firstUse = liveInterval.UsePositions.Count != 0 ? liveInterval.UsePositions[0] : null;
			var firstDef = liveInterval.DefPositions.Count != 0 ? liveInterval.DefPositions[0] : null;

			if (at == null)
			{
				at = firstUse;
			}

			if (at != null && firstDef != null && firstDef < at)
			{
				at = firstDef;
			}

			if (at >= liveInterval.End)
				return false;

			if (at <= liveInterval.Start)
				return false;

			if (Trace.Active) Trace.Log(" Splitting around first use/def");

			return PreferBlockBoundaryIntervalSplit(liveInterval, at, true);
		}

		private SlotIndex GetMaximum(SlotIndex a, SlotIndex b, SlotIndex c, SlotIndex d)
		{
			var max = a;

			if (max == null || (b != null && b > max))
				max = b;

			if (max == null || (c != null && c > max))
				max = c;

			if (max == null || (d != null && d > max))
				max = d;

			return max;
		}

		private SlotIndex GetMinimum(SlotIndex a, SlotIndex b, SlotIndex c, SlotIndex d)
		{
			var min = a;

			if (min == null || (b != null && b < min))
				min = b;

			if (min == null || (c != null && c < min))
				min = c;

			if (min == null || (d != null && d < min))
				min = d;

			return min;
		}

		private SlotIndex GetLowerOptimalSplitLocation(LiveInterval liveInterval, SlotIndex at)
		{
			if (Trace.Active) Trace.Log("--Low Splitting: " + liveInterval.ToString() + " move: " + at.ToString());

			//a = liveInterval.Start.IsOnHalfStep ? liveInterval.Start : liveInterval.Start.HalfStepForward;
			var a = liveInterval.Start;

			var blockStart = GetBlockStart(at);
			var b = blockStart > liveInterval.Start ? blockStart : null;
			if (Trace.Active) Trace.Log("   Block Start : " + (b != null ? b.ToString() : "null"));

			var c = liveInterval.LiveRange.GetPreviousUsePosition(at);
			if (c != null && c.HalfStepForward <= at)
				c = c.HalfStepForward;
			if (Trace.Active) Trace.Log("  Previous Use : " + (c != null ? c.ToString() : "null"));

			var d = liveInterval.LiveRange.GetPreviousDefPosition(at);
			if (d != null && d.HalfStepForward <= at)
				d = d.HalfStepForward;
			if (Trace.Active) Trace.Log("  Previous Def : " + (d != null ? d.ToString() : "null"));

			var max = GetMaximum(a, b, c, d);

			if (Trace.Active) Trace.Log("   Low Optimal : " + max.ToString());

			return max;
		}

		private SlotIndex GetUpperOptimalSplitLocation(LiveInterval liveInterval, SlotIndex at)
		{
			if (Trace.Active) Trace.Log("-High Splitting: " + liveInterval.ToString() + " move: " + at.ToString());

			var a = liveInterval.End;

			var blockEnd = GetBlockEnd(at);
			var b = blockEnd > liveInterval.End ? blockEnd : null;
			if (Trace.Active) Trace.Log("     Block End : " + (b != null ? b.ToString() : "null"));

			var c = liveInterval.LiveRange.GetNextUsePosition(at);
			if (c != null && c.HalfStepBack > at)
				c = c.HalfStepBack;
			if (Trace.Active) Trace.Log("      Next Use : " + (c != null ? c.ToString() : "null"));

			var d = liveInterval.LiveRange.GetNextDefPosition(at);
			if (Trace.Active) Trace.Log("      Next Def : " + (d != null ? d.ToString() : "null"));

			var min = GetMinimum(a, b, c, d);

			if (Trace.Active) Trace.Log("  High Optimal : " + min.ToString());

			return min;
		}

		private void CollectMoveHints()
		{
			foreach (var block in BasicBlocks)
			{
				for (Context context = new Context(InstructionSet, block); !context.IsBlockEndInstruction; context.GotoNext())
				{
					if (context.IsEmpty || context.IsBlockStartInstruction || context.IsBlockEndInstruction)
						continue;

					if (!Architecture.IsInstructionMove(context.Instruction))
						continue;

					if (!((context.Result.IsVirtualRegister && context.Operand1.IsVirtualRegister)
						|| (context.Result.IsVirtualRegister && context.Operand1.IsCPURegister)
						|| (context.Result.IsCPURegister && context.Operand1.IsVirtualRegister)))
						continue;

					var from = VirtualRegisters[GetIndex(context.Operand1)];
					var to = VirtualRegisters[GetIndex(context.Result)];

					var slot = new SlotIndex(context);

					int factor = (from.IsPhysicalRegister || to.IsPhysicalRegister) ? 150 : 125;

					int bonus = GetLoopDepth(slot);

					moveHints.Add(slot, new MoveHint(slot, from, to, bonus));
				}
			}
		}

		private void TraceMoveHints()
		{
			var moveHintTrace = CreateTrace("MoveHints");

			if (!moveHintTrace.Active)
				return;

			foreach (var moveHint in moveHints)
			{
				moveHintTrace.Log(moveHint.Value.ToString());
			}
		}
	}
}