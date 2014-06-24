/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.InternalTrace;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	/// <summary>
	///
	/// </summary>
	public sealed class GreedyRegisterAllocator : BaseRegisterAllocator
	{
		private Dictionary<SlotIndex, MoveHint> moveHints;

		public GreedyRegisterAllocator(BasicBlocks basicBlocks, VirtualRegisters compilerVirtualRegisters, InstructionSet instructionSet, StackLayout stackLayout, BaseArchitecture architecture, CompilerTrace trace)
			: base(basicBlocks, compilerVirtualRegisters, instructionSet, stackLayout, architecture, trace)
		{
			moveHints = new Dictionary<SlotIndex, MoveHint>();
		}

		protected override void AdditionalSetup()
		{
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

		private void SplitIntervalsAtCallSites()
		{
			foreach (var virtualRegister in VirtualRegisters)
			{
				if (virtualRegister.IsPhysicalRegister)
					continue;

				for (int i = 0; i < virtualRegister.LiveIntervals.Count; i++)
				{
					LiveInterval liveInterval = virtualRegister.LiveIntervals[i];

					if (liveInterval.ForceSpilled)
						continue;

					if (liveInterval.IsEmpty)
						continue;

					var callSite = FindCallSiteInInterval(liveInterval);

					if (callSite == null)
						continue;

					SplitInterval(liveInterval, callSite, false);

					i = 0; // list was modified
				}
			}
		}

		protected override bool TrySplitInterval(LiveInterval liveInterval, int level)
		{
			if (liveInterval.IsEmpty)
				return false;

			if (TrySimplePartialFreeIntervalSplit(liveInterval))
				return true;

			if (level <= 1)
				return false;

			if (IntervalSplitAtFirstUseOrDef(liveInterval))
				return true;

			return false;
		}

		private bool SplitInterval(LiveInterval liveInterval, SlotIndex at, bool addToQueue)
		{
			// can not split on use position
			if (liveInterval.UsePositions.Contains(at))
				return false;

			var low = GetLowerOptimalSplitLocation(liveInterval, at);

			var high = GetUpperOptimalSplitLocation(liveInterval, at);

			if (high == liveInterval.End)
			{
				high = low;
			}

			var first = liveInterval.Split(liveInterval.Start, low);
			var last = liveInterval.Split(high, liveInterval.End);

			var middle = (low != high) ? liveInterval.Split(low, high) : null;

			if (Trace.Active) Trace.Log("   Low Split   : " + first.ToString());
			if (Trace.Active) Trace.Log("   Middle Split: " + (middle == null ? "n/a" : middle.ToString()));
			if (Trace.Active) Trace.Log("   High Split  : " + last.ToString());

			CalculateSpillCost(first);
			CalculateSpillCost(last);

			liveInterval.IsSplit = true;

			var virtualRegister = liveInterval.VirtualRegister;

			virtualRegister.Remove(liveInterval);

			virtualRegister.Add(first);
			virtualRegister.Add(last);

			if (addToQueue)
			{
				AddPriorityQueue(first);
				AddPriorityQueue(last);
			}

			if (middle != null)
			{
				//middle.ForceSpilled = true;
				CalculateSpillCost(middle);
				virtualRegister.Add(middle);
				if (addToQueue)
				{
					AddPriorityQueue(middle);
				}
			}

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

			if (Trace.Active) Trace.Log("  Partial free up to: " + furthestUsed.ToString());

			return SplitInterval(liveInterval, furthestUsed, true);
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

			return SplitInterval(liveInterval, at, true);
		}

		private SlotIndex GetMaximum(SlotIndex[] slots)
		{
			SlotIndex max = null;

			foreach (var slot in slots)
			{
				if (slot == null)
					continue;

				if (max == null || slot > max)
				{
					max = slot;
				}
			}

			return max;
		}

		private SlotIndex GetMinimum(SlotIndex[] slots)
		{
			SlotIndex min = null;

			foreach (var slot in slots)
			{
				if (slot == null)
					continue;

				if (min == null || slot < min)
				{
					min = slot;
				}
			}

			return min;
		}

		private SlotIndex GetLowerOptimalSplitLocation(LiveInterval liveInterval, SlotIndex at)
		{
			if (Trace.Active) Trace.Log("--Low Splitting: " + liveInterval.ToString() + " move: " + at.ToString());

			var blockStart = GetBlockStart(at);

			var slots = new SlotIndex[4];

			slots[0] = liveInterval.Start.IsOnHalfStep ? liveInterval.Start : liveInterval.Start.HalfStepForward;

			slots[1] = blockStart > liveInterval.Start ? blockStart : null;
			if (Trace.Active) Trace.Log("   Block Start : " + (slots[1] != null ? slots[1].ToString() : "null"));

			slots[2] = liveInterval.LiveRange.GetPreviousUsePosition(at);
			if (Trace.Active) Trace.Log("  Previous Use : " + (slots[2] != null ? slots[2].ToString() : "null"));

			slots[3] = liveInterval.LiveRange.GetPreviousDefPosition(at);
			if (Trace.Active) Trace.Log("  Previous Def : " + (slots[3] != null ? slots[3].ToString() : "null"));

			var max = GetMaximum(slots);

			if (Trace.Active) Trace.Log("   Low Optimal : " + max.ToString());

			return max;
		}

		private SlotIndex GetUpperOptimalSplitLocation(LiveInterval liveInterval, SlotIndex at)
		{
			if (Trace.Active) Trace.Log("-High Splitting: " + liveInterval.ToString() + " move: " + at.ToString());

			var next = liveInterval.LiveRange.GetNextUsePosition(at);

			if (next == null)
			{
				return liveInterval.End;
			}

			var blockEnd = GetBlockEnd(at);

			var slots = new SlotIndex[4];

			slots[0] = liveInterval.End.HalfStepBack;

			slots[1] = blockEnd > liveInterval.End ? blockEnd : null;
			if (Trace.Active) Trace.Log("     Block End : " + (slots[1] != null ? slots[1].ToString() : "null"));

			slots[2] = next != null ? next.HalfStepBack : null;
			if (Trace.Active) Trace.Log("      Next Use : " + (slots[2] != null ? slots[2].ToString() : "null"));

			slots[3] = liveInterval.LiveRange.GetNextDefPosition(at);
			if (Trace.Active) Trace.Log("      Next Def : " + (slots[3] != null ? slots[3].ToString() : "null"));

			var min = GetMinimum(slots);

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
			if (!Trace.Active)
				return;

			var moveHintTrace = new CompilerTrace(Trace, "MoveHints");

			foreach (var moveHint in moveHints)
			{
				moveHintTrace.Log(moveHint.Value.ToString());
			}
		}
	}
}