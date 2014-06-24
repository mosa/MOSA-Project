/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.InternalTrace;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	/// <summary>
	///
	/// </summary>
	public abstract class BaseRegisterAllocator
	{
		protected readonly BasicBlocks BasicBlocks;
		protected readonly InstructionSet InstructionSet;
		protected readonly BaseArchitecture Architecture;
		private readonly StackLayout StackLayout;

		private readonly int VirtualRegisterCount;
		private readonly int PhysicalRegisterCount;
		private readonly int RegisterCount;

		private readonly List<ExtendedBlock> ExtendedBlocks;
		protected readonly List<VirtualRegister> VirtualRegisters;

		private readonly SimpleKeyPriorityQueue<LiveInterval> PriorityQueue;
		protected readonly List<LiveIntervalTrack> LiveIntervalTracks;

		private readonly Register StackFrameRegister;
		private readonly Register StackPointerRegister;
		private readonly Register ProgramCounter;

		private readonly List<LiveInterval> SpilledIntervals;

		private readonly List<SlotIndex> CallSlots;

		protected readonly CompilerTrace Trace;

		public BaseRegisterAllocator(BasicBlocks basicBlocks, VirtualRegisters compilerVirtualRegisters, InstructionSet instructionSet, StackLayout stackLayout, BaseArchitecture architecture, CompilerTrace trace)
		{
			this.Trace = trace;

			this.BasicBlocks = basicBlocks;
			this.InstructionSet = instructionSet;
			this.StackLayout = stackLayout;
			this.Architecture = architecture;

			this.VirtualRegisterCount = compilerVirtualRegisters.Count;
			this.PhysicalRegisterCount = architecture.RegisterSet.Length;
			this.RegisterCount = VirtualRegisterCount + PhysicalRegisterCount;

			this.LiveIntervalTracks = new List<LiveIntervalTrack>(PhysicalRegisterCount);
			this.VirtualRegisters = new List<VirtualRegister>(RegisterCount);
			this.ExtendedBlocks = new List<ExtendedBlock>(basicBlocks.Count);

			StackFrameRegister = architecture.StackFrameRegister;
			StackPointerRegister = architecture.StackPointerRegister;
			ProgramCounter = architecture.ProgramCounter;

			// Setup extended physical registers
			foreach (var physicalRegister in architecture.RegisterSet)
			{
				Debug.Assert(physicalRegister.Index == VirtualRegisters.Count);
				Debug.Assert(physicalRegister.Index == LiveIntervalTracks.Count);

				bool reserved = (physicalRegister == StackFrameRegister
					|| physicalRegister == StackPointerRegister
					|| (ProgramCounter != null && physicalRegister == ProgramCounter));

				this.VirtualRegisters.Add(new VirtualRegister(physicalRegister, reserved));
				this.LiveIntervalTracks.Add(new LiveIntervalTrack(physicalRegister, reserved));
			}

			// Setup extended virtual registers
			foreach (var virtualRegister in compilerVirtualRegisters)
			{
				Debug.Assert(virtualRegister.Index == VirtualRegisters.Count - PhysicalRegisterCount + 1);

				this.VirtualRegisters.Add(new VirtualRegister(virtualRegister));
			}

			PriorityQueue = new SimpleKeyPriorityQueue<LiveInterval>();
			SpilledIntervals = new List<LiveInterval>();

			CallSlots = new List<SlotIndex>();
		}

		public virtual void Start()
		{
			// Order all the blocks
			CreateExtendedBlocks();

			// Number all the instructions in block order
			NumberInstructions();

			// Generate trace information for instruction numbering
			TraceNumberInstructions();

			// Computer local live sets
			ComputeLocalLiveSets();

			// Computer global live sets
			ComputeGlobalLiveSets();

			// Build the live intervals
			BuildLiveIntervals();

			// Generate trace information for blocks
			TraceBlocks();

			// Generate trace information for live intervals
			TraceLiveIntervals("InitialLiveIntervals", false);

			AdditionalSetup();

			// Calculate spill costs for live intervals
			AssignSpillCosts();

			// Populate priority queue
			PopulatePriorityQueue();

			// Process priority queue
			ProcessPriorityQueue();

			// Create spill slots operands
			CreateSpillSlotOperands();

			// Create physical register operands
			CreatePhysicalRegisterOperands();

			// Insert spill moves
			InsertSpillMoves();

			// Assign physical registers
			AssignRegisters();

			// Insert register moves
			InsertRegisterMoves();

			// Resolve data flow
			ResolveDataFlow();

			// Generate trace information for live intervals
			TraceLiveIntervals("PostLiveIntervals", true);
		}

		protected abstract void AdditionalSetup();

		private static string ToString(BitArray bitArray)
		{
			var builder = new StringBuilder();

			foreach (bool bit in bitArray)
			{
				builder.Append(bit ? "X" : ".");
			}

			return builder.ToString();
		}

		private void TraceBlocks()
		{
			if (!Trace.Active)
				return;

			var sectionTrace = new CompilerTrace(Trace, "Extended Blocks");

			foreach (var block in ExtendedBlocks)
			{
				sectionTrace.Log("Block # " + block.BasicBlock.Sequence.ToString() + " (" + block.Start + " to " + block.End + ")");
				sectionTrace.Log(" LiveIn:   " + ToString(block.LiveIn));
				sectionTrace.Log(" LiveGen:  " + ToString(block.LiveGen));
				sectionTrace.Log(" LiveKill: " + ToString(block.LiveKill));
				sectionTrace.Log(" LiveOut:  " + ToString(block.LiveOut));
			}
		}

		private void TraceLiveIntervals(string stage, bool operand)
		{
			if (!Trace.Active)
				return;

			var registerTrace = new CompilerTrace(Trace, stage);

			foreach (var virtualRegister in VirtualRegisters)
			{
				if (virtualRegister.IsPhysicalRegister)
				{
					registerTrace.Log("Physical Register # " + virtualRegister.PhysicalRegister.ToString());
				}
				else
				{
					registerTrace.Log("Virtual Register # " + virtualRegister.VirtualRegisterOperand.Index.ToString());
				}

				registerTrace.Log("Live Intervals (" + virtualRegister.LiveIntervals.Count.ToString() + "): " + LiveIntervalsToString(virtualRegister.LiveIntervals, operand));

				registerTrace.Log("Def Positions (" + virtualRegister.DefPositions.Count.ToString() + "): " + SlotsToString(virtualRegister.DefPositions));
				registerTrace.Log("Use Positions (" + virtualRegister.UsePositions.Count.ToString() + "): " + SlotsToString(virtualRegister.UsePositions));
			}
		}

		private String LiveIntervalsToString(List<LiveInterval> liveIntervals)
		{
			return LiveIntervalsToString(liveIntervals, false);
		}

		private String LiveIntervalsToString(List<LiveInterval> liveIntervals, bool operand)
		{
			if (liveIntervals.Count == 0)
				return string.Empty;

			StringBuilder sb = new StringBuilder();

			foreach (var liveInterval in liveIntervals)
			{
				if (operand && !liveInterval.IsPhysicalRegister)
					sb.Append("[" + liveInterval.Start.ToString() + ", " + liveInterval.End.ToString() + "]/" + liveInterval.AssignedOperand.ToString() + ",");
				else
					sb.Append("[" + liveInterval.Start.ToString() + ", " + liveInterval.End.ToString() + "],");
			}

			if (sb[sb.Length - 1] == ',')
				sb.Length = sb.Length - 1;

			return sb.ToString();
		}

		protected string SlotsToString(IList<SlotIndex> slots)
		{
			if (slots.Count == 0)
				return string.Empty;

			StringBuilder sb = new StringBuilder();

			foreach (var use in slots)
			{
				sb.Append(use.ToString() + ",");
			}

			if (sb[sb.Length - 1] == ',')
				sb.Length = sb.Length - 1;

			return sb.ToString();
		}

		protected int GetIndex(Operand operand)
		{
			//FUTURE: Make private by refactoring
			return (operand.IsCPURegister) ? (operand.Register.Index) : (operand.Index + PhysicalRegisterCount - 1);
		}

		private void CreateExtendedBlocks()
		{
			var blockOrder = new LoopAwareBlockOrder(this.BasicBlocks);

			// The re-ordering is not strictly necessary; however, it reduces "holes" in live ranges.
			// Less "holes" increase readability of the debug logs.
			//basicBlocks.ReorderBlocks(loopAwareBlockOrder.NewBlockOrder);

			// Allocate and setup extended blocks
			for (int i = 0; i < BasicBlocks.Count; i++)
			{
				ExtendedBlocks.Add(new ExtendedBlock(BasicBlocks[i], RegisterCount, blockOrder.GetLoopDepth(BasicBlocks[i])));
			}
		}

		protected void AddSpilledInterval(LiveInterval liveInterval)
		{
			SpilledIntervals.Add(liveInterval);
		}

		public static void NumberInstructions(BasicBlocks basicBlocks, InstructionSet instructionSet)
		{
			int index = SlotIndex.Increment;

			foreach (var block in basicBlocks)
			{
				for (var context = new Context(instructionSet, block); ; context.GotoNext())
				{
					if (!context.IsEmpty)
					{
						context.SlotNumber = index;
						index = index + SlotIndex.Increment;
					}

					if (context.IsBlockEndInstruction)
						break;
				}
			}
		}

		private void NumberInstructions()
		{
			NumberInstructions(BasicBlocks, InstructionSet);

			foreach (BasicBlock block in BasicBlocks)
			{
				SlotIndex start = new SlotIndex(InstructionSet, block.StartIndex);
				SlotIndex end = new SlotIndex(InstructionSet, block.EndIndex);
				ExtendedBlocks[block.Sequence].Interval = new Interval(start, end);
			}
		}

		private void TraceNumberInstructions()
		{
			if (!Trace.Active)
				return;

			var number = new CompilerTrace(Trace, "InstructionNumber");

			int index = SlotIndex.Increment;

			foreach (BasicBlock block in BasicBlocks)
			{
				for (Context context = new Context(InstructionSet, block); ; context.GotoNext())
				{
					if (context.IsEmpty)
						continue;

					if (context.IsBlockStartInstruction)
					{
						number.Log(context.SlotNumber.ToString() + " = " + context.ToString() + " # " + block.ToString());
					}
					else
					{
						number.Log(context.SlotNumber.ToString() + " = " + context.ToString());
					}

					if (context.IsBlockEndInstruction)
						break;
				}
			}
		}

		private void ComputeLocalLiveSets()
		{
			var liveSetTrace = new CompilerTrace(Trace, "ComputeLocalLiveSets");

			foreach (var block in ExtendedBlocks)
			{
				if (liveSetTrace.Active)
					liveSetTrace.Log("Block # " + block.BasicBlock.Sequence.ToString());

				BitArray liveGen = new BitArray(RegisterCount, false);
				BitArray liveKill = new BitArray(RegisterCount, false);

				liveGen.Set(StackFrameRegister.Index, true);
				liveGen.Set(StackPointerRegister.Index, true);

				if (ProgramCounter != null)
					liveGen.Set(ProgramCounter.Index, true);

				for (Context context = new Context(InstructionSet, block.BasicBlock); !context.IsBlockEndInstruction; context.GotoNext())
				{
					if (context.IsEmpty)
						continue;

					if (liveSetTrace.Active)
						liveSetTrace.Log(context.ToString());

					OperandVisitor visitor = new OperandVisitor(context);

					foreach (var ops in visitor.Input)
					{
						if (liveSetTrace.Active)
							liveSetTrace.Log("INPUT:  " + ops.ToString());

						int index = GetIndex(ops);
						if (!liveKill.Get(index))
						{
							liveGen.Set(index, true);

							if (liveSetTrace.Active)
								liveSetTrace.Log("GEN:  " + index.ToString() + " " + ops.ToString());
						}
					}

					if (context.Instruction.FlowControl == FlowControl.Call)
					{
						for (int s = 0; s < PhysicalRegisterCount; s++)
						{
							liveKill.Set(s, true);
						}

						if (liveSetTrace.Active)
							liveSetTrace.Log("KILL ALL PHYSICAL");
					}

					foreach (var ops in visitor.Output)
					{
						if (liveSetTrace.Active)
							liveSetTrace.Log("OUTPUT: " + ops.ToString());

						int index = GetIndex(ops);
						liveKill.Set(index, true);

						if (liveSetTrace.Active)
							liveSetTrace.Log("KILL: " + index.ToString() + " " + ops.ToString());
					}
				}

				block.LiveGen = liveGen;
				block.LiveKill = liveKill;
				block.LiveKillNot = ((BitArray)liveKill.Clone()).Not();

				if (liveSetTrace.Active)
				{
					liveSetTrace.Log("GEN:     " + ToString(block.LiveGen));
					liveSetTrace.Log("KILL:    " + ToString(block.LiveKill));
					liveSetTrace.Log("KILLNOT: " + ToString(block.LiveKillNot));
					liveSetTrace.Log(string.Empty);
				}
			}
		}

		private void ComputeGlobalLiveSets()
		{
			bool changed = true;
			while (changed)
			{
				changed = false;

				for (int i = BasicBlocks.Count - 1; i >= 0; i--)
				{
					var block = ExtendedBlocks[i];

					BitArray liveOut = new BitArray(RegisterCount);

					foreach (var next in block.BasicBlock.NextBlocks)
					{
						liveOut.Or(ExtendedBlocks[next.Sequence].LiveIn);
					}

					BitArray liveIn = (BitArray)block.LiveOut.Clone();
					liveIn.And(block.LiveKillNot);
					liveIn.Or(block.LiveGen);

					// compare them for any changes
					if (!changed)
						if (!block.LiveOut.AreSame(liveOut) || !block.LiveIn.AreSame(liveIn))
							changed = true;

					block.LiveOut = liveOut;
					block.LiveIn = liveIn;
				}
			}
		}

		private void BuildLiveIntervals()
		{
			var intervalTrace = new CompilerTrace(Trace, "BuildLiveIntervals");

			for (int b = BasicBlocks.Count - 1; b >= 0; b--)
			{
				var block = ExtendedBlocks[b];

				for (int r = 0; r < RegisterCount; r++)
				{
					if (!block.LiveOut.Get(r))
						continue;

					var register = VirtualRegisters[r];

					if (b + 1 != BasicBlocks.Count && ExtendedBlocks[b + 1].LiveIn.Get(r))
					{
						if (intervalTrace.Active) intervalTrace.Log("Add (LiveOut) " + register.ToString() + " : " + block.Start + " to " + ExtendedBlocks[b + 1].Start);
						if (intervalTrace.Active) intervalTrace.Log("   Before: " + LiveIntervalsToString(register.LiveIntervals));
						register.AddLiveInterval(block.Start, ExtendedBlocks[b + 1].Start);
						if (intervalTrace.Active) intervalTrace.Log("    After: " + LiveIntervalsToString(register.LiveIntervals));
					}
					else
					{
						if (intervalTrace.Active) intervalTrace.Log("Add (!LiveOut) " + register.ToString() + " : " + block.Interval.Start + " to " + block.Interval.End);
						if (intervalTrace.Active) intervalTrace.Log("   Before: " + LiveIntervalsToString(register.LiveIntervals));
						register.AddLiveInterval(block.Interval);
						if (intervalTrace.Active) intervalTrace.Log("    After: " + LiveIntervalsToString(register.LiveIntervals));
					}
				}

				Context context = new Context(InstructionSet, block.BasicBlock, block.BasicBlock.EndIndex);

				while (!context.IsBlockStartInstruction)
				{
					if (!context.IsEmpty)
					{
						SlotIndex slotIndex = new SlotIndex(context);

						OperandVisitor visitor = new OperandVisitor(context);

						if (context.Instruction.FlowControl == FlowControl.Call)
						{
							for (int s = 0; s < PhysicalRegisterCount; s++)
							{
								var register = VirtualRegisters[s];
								if (intervalTrace.Active) intervalTrace.Log("Add (Call) " + register.ToString() + " : " + slotIndex + " to " + slotIndex.HalfStepForward);
								if (intervalTrace.Active) intervalTrace.Log("   Before: " + LiveIntervalsToString(register.LiveIntervals));
								register.AddLiveInterval(slotIndex, slotIndex.HalfStepForward);
								if (intervalTrace.Active) intervalTrace.Log("    After: " + LiveIntervalsToString(register.LiveIntervals));
							}

							CallSlots.Add(slotIndex);
						}

						foreach (var result in visitor.Output)
						{
							var register = VirtualRegisters[GetIndex(result)];

							if (register.IsReserved)
								continue;

							var first = register.FirstRange;

							if (!register.IsPhysicalRegister)
							{
								register.AddDefPosition(slotIndex);
							}

							if (first != null)
							{
								if (intervalTrace.Active) intervalTrace.Log("Replace First " + register.ToString() + " : " + slotIndex + " to " + first.End);
								if (intervalTrace.Active) intervalTrace.Log("   Before: " + LiveIntervalsToString(register.LiveIntervals));
								register.FirstRange = new LiveInterval(register, slotIndex, first.End);
								if (intervalTrace.Active) intervalTrace.Log("    After: " + LiveIntervalsToString(register.LiveIntervals));
							}
							else
							{
								// This is necesary to handled a result that is never used!
								// Common with instructions which more than one result
								if (intervalTrace.Active) intervalTrace.Log("Add (Unused) " + register.ToString() + " : " + slotIndex + " to " + slotIndex);
								if (intervalTrace.Active) intervalTrace.Log("   Before: " + LiveIntervalsToString(register.LiveIntervals));
								register.AddLiveInterval(slotIndex, slotIndex.HalfStepForward);
								if (intervalTrace.Active) intervalTrace.Log("    After: " + LiveIntervalsToString(register.LiveIntervals));
							}
						}

						foreach (var result in visitor.Input)
						{
							var register = VirtualRegisters[GetIndex(result)];

							if (register.IsReserved)
								continue;

							if (!register.IsPhysicalRegister)
							{
								register.AddUsePosition(slotIndex);
							}

							if (intervalTrace.Active) intervalTrace.Log("Add (normal) " + register.ToString() + " : " + block.Start + " to " + slotIndex);
							if (intervalTrace.Active) intervalTrace.Log("   Before: " + LiveIntervalsToString(register.LiveIntervals));
							register.AddLiveInterval(block.Start, slotIndex);
							if (intervalTrace.Active) intervalTrace.Log("    After: " + LiveIntervalsToString(register.LiveIntervals));
						}
					}

					context.GotoPrevious();
				}
			}
		}

		private ExtendedBlock GetContainingBlock(SlotIndex slotIndex)
		{
			foreach (var block in ExtendedBlocks)
			{
				if (block.Contains(slotIndex))
				{
					return block;
				}
			}

			Debug.Assert(false, "GetContainingBlock");
			return null;
		}

		protected int GetLoopDepth(SlotIndex slotIndex)
		{
			return GetContainingBlock(slotIndex).LoopDepth + 1;
		}

		protected SlotIndex GetBlockEnd(SlotIndex slotIndex)
		{
			return GetContainingBlock(slotIndex).End;
		}

		protected SlotIndex GetBlockStart(SlotIndex slotIndex)
		{
			return GetContainingBlock(slotIndex).Start;
		}

		protected abstract void CalculateSpillCost(LiveInterval liveInterval);

		private void AssignSpillCosts()
		{
			foreach (var virtualRegister in VirtualRegisters)
			{
				foreach (var liveInterval in virtualRegister.LiveIntervals)
				{
					// Skip adding live intervals for physical registers to priority queue
					if (liveInterval.VirtualRegister.IsPhysicalRegister)
					{
						// fixed and not spillable
						liveInterval.NeverSpill = true;
					}
					else if (liveInterval.VirtualRegister.VirtualRegisterOperand.Type.IsUserValueType)
					{
						// Value types will never be spilled (they are already allocated in stack anyways)
						liveInterval.NeverSpill = true;
					}
					else
					{
						// Calculate spill costs for live interval
						CalculateSpillCost(liveInterval);
					}
				}
			}
		}

		protected abstract int CalculatePriorityValue(LiveInterval liveInterval);

		protected void AddPriorityQueue(LiveInterval liveInterval)
		{
			Debug.Assert(!liveInterval.IsSplit);
			Debug.Assert(liveInterval.LiveIntervalTrack == null);

			// priority is based on allocation stage (primary, lower first) and interval size (secondary, higher first)
			int value = CalculatePriorityValue(liveInterval);

			PriorityQueue.Enqueue(value, liveInterval);
		}

		private void PopulatePriorityQueue()
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

					// Add live intervals for virtual registers to priority queue
					AddPriorityQueue(liveInterval);
				}
			}
		}

		private void ProcessPriorityQueue()
		{
			while (!PriorityQueue.IsEmpty)
			{
				var liveInterval = PriorityQueue.Dequeue();

				ProcessLiveInterval(liveInterval);
			}
		}

		protected bool PlaceLiveIntervalOnTrack(LiveInterval liveInterval, LiveIntervalTrack track)
		{
			if (track.IsReserved)
				return false;

			if (track.IsFloatingPoint != liveInterval.VirtualRegister.IsFloatingPoint)
				return false;

			if (track.Intersects(liveInterval))
				return false;

			if (Trace.Active) Trace.Log("  Assigned live interval to: " + track.ToString());

			track.Add(liveInterval);

			return true;
		}

		protected bool PlaceLiveIntervalOnAnyAvailableTrack(LiveInterval liveInterval)
		{
			foreach (var track in LiveIntervalTracks)
			{
				if (PlaceLiveIntervalOnTrack(liveInterval, track))
				{
					return true;
				}
			}

			return false;
		}

		protected bool PlaceLiveIntervalOnTrackAllowEvictions(LiveInterval liveInterval)
		{
			// find live interval(s) to evict based on spill costs
			foreach (var track in LiveIntervalTracks)
			{
				if (track.IsReserved)
					continue;

				if (track.IsFloatingPoint != liveInterval.VirtualRegister.IsFloatingPoint)
					continue;

				var intersections = track.GetIntersections(liveInterval);

				bool evict = true;

				foreach (var intersection in intersections)
				{
					if (intersection.SpillCost >= liveInterval.SpillCost || intersection.SpillCost == Int32.MaxValue || intersection.VirtualRegister.IsPhysicalRegister || intersection.IsPhysicalRegister)
					{
						evict = false;
						break;
					}
				}

				if (evict)
				{
					if (intersections.Count != 0)
					{
						if (Trace.Active) Trace.Log("  Evicting live intervals");

						track.Evict(intersections);

						foreach (var intersection in intersections)
						{
							if (Trace.Active) Trace.Log("  Evicted: " + intersection.ToString());

							liveInterval.Stage = LiveInterval.AllocationStage.Initial;
							AddPriorityQueue(intersection);
						};
					}

					track.Add(liveInterval);

					if (Trace.Active) Trace.Log("  Assigned live interval to: " + track.ToString());

					return true;
				}
			}

			return false;
		}

		protected abstract bool PlaceLiveInterval(LiveInterval liveInterval);

		private void ProcessLiveInterval(LiveInterval liveInterval)
		{
			Debug.Assert(liveInterval.LiveIntervalTrack == null);
			Debug.Assert(!liveInterval.IsSplit);

			if (Trace.Active)
			{
				Trace.Log("Processing Interval: " + liveInterval.ToString() + " / Length: " + liveInterval.Length.ToString() + " / Spill Cost: " + liveInterval.SpillCost.ToString() + " / Stage: " + liveInterval.Stage.ToString());
				Trace.Log("  Defs (" + liveInterval.DefPositions.Count.ToString() + "): " + SlotsToString(liveInterval.DefPositions));
				Trace.Log("  Uses (" + liveInterval.UsePositions.Count.ToString() + "): " + SlotsToString(liveInterval.UsePositions));
			}

			if (PlaceLiveInterval(liveInterval))
			{
				return;
			}

			if (Trace.Active) Trace.Log("  No live intervals to evicts");

			// No live intervals to evict!

			// prepare to split live interval
			if (liveInterval.Stage == LiveInterval.AllocationStage.Initial)
			{
				if (Trace.Active) Trace.Log("  Re-queued for split level 1 stage");
				liveInterval.Stage = LiveInterval.AllocationStage.SplitLevel1;
				AddPriorityQueue(liveInterval);
				return;
			}

			// split live interval - level 1
			if (liveInterval.Stage == LiveInterval.AllocationStage.SplitLevel1)
			{
				if (Trace.Active) Trace.Log("  Attempting to split interval - level 1");

				if (TrySplitInterval(liveInterval, 1))
				{
					return;
				}

				// Move to split level 2 stage
				if (Trace.Active) Trace.Log("  Re-queued for split interval - level 2");

				liveInterval.Stage = LiveInterval.AllocationStage.SplitLevel2;
				AddPriorityQueue(liveInterval);
				return;
			}

			// split live interval - level 2
			if (liveInterval.Stage == LiveInterval.AllocationStage.SplitLevel2)
			{
				if (Trace.Active) Trace.Log("  Attempting to split interval - level 2");

				if (TrySplitInterval(liveInterval, 2))
				{
					return;
				}

				// Move to spill stage
				if (Trace.Active) Trace.Log("  Re-queued for spillable stage");

				liveInterval.Stage = LiveInterval.AllocationStage.SplitLevel3;
				AddPriorityQueue(liveInterval);
				return;
			}

			// split live interval - level 3
			if (liveInterval.Stage == LiveInterval.AllocationStage.SplitLevel3)
			{
				if (Trace.Active) Trace.Log("  Attempting to split interval - level 3");

				if (TrySplitInterval(liveInterval, 3))
				{
					return;
				}

				// Move to spill stage
				if (Trace.Active) Trace.Log("  Re-queued for spillable stage");

				liveInterval.Stage = LiveInterval.AllocationStage.SplitFinal;
				AddPriorityQueue(liveInterval);
				return;
			}

			// Final split option
			if (liveInterval.Stage == LiveInterval.AllocationStage.SplitFinal)
			{
				if (Trace.Active) Trace.Log("  Final split option");

				SplitLastResort(liveInterval);
				return;
			}

			return;
		}

		protected abstract bool TrySplitInterval(LiveInterval liveInterval, int level);

		private void SplitLastResort(LiveInterval liveInterval)
		{
			// This is the last option when all other split option fail.
			// Split interval up into very small pieces that can always be placed.

			// TODO
		}

		private IEnumerable<VirtualRegister> GetVirtualRegisters(BitArray array)
		{
			for (int i = 0; i < array.Count; i++)
			{
				if (array.Get(i))
				{
					var virtualRegister = VirtualRegisters[i];
					if (!virtualRegister.IsPhysicalRegister)
						yield return virtualRegister;
				}
			}
		}

		protected SlotIndex FindCallSiteInInterval(LiveInterval liveInterval)
		{
			foreach (SlotIndex slot in CallSlots)
			{
				if (liveInterval.Contains(slot))
					return slot;
			}
			return null;
		}

		protected void CreateSpillSlotOperands()
		{
			foreach (var register in VirtualRegisters)
			{
				if (!register.IsSpilled)
					continue;

				Debug.Assert(register.IsVirtualRegister);
				register.SpillSlotOperand = StackLayout.AddStackLocal(register.VirtualRegisterOperand.Type);
			}
		}

		protected void CreatePhysicalRegisterOperands()
		{
			foreach (var register in VirtualRegisters)
			{
				if (!register.IsUsed || register.IsPhysicalRegister)
					continue;

				foreach (var liveInterval in register.LiveIntervals)
				{
					if (liveInterval.AssignedPhysicalRegister == null)
						continue;

					liveInterval.AssignedPhysicalOperand = Operand.CreateCPURegister(liveInterval.VirtualRegister.VirtualRegisterOperand.Type, liveInterval.AssignedPhysicalRegister);
				}
			}
		}

		protected void InsertSpillMoves()
		{
			foreach (var register in VirtualRegisters)
			{
				if (!register.IsUsed || register.IsPhysicalRegister || !register.IsSpilled)
					continue;

				foreach (var liveInterval in register.LiveIntervals)
				{
					foreach (var def in liveInterval.DefPositions)
					{
						Context context = new Context(InstructionSet, def.Index);

						Architecture.InsertMoveInstruction(context, register.SpillSlotOperand, liveInterval.AssignedPhysicalOperand);

						context.Marked = true;
					}
				}
			}
		}

		protected void AssignRegisters()
		{
			foreach (var register in VirtualRegisters)
			{
				if (!register.IsUsed || register.IsPhysicalRegister)
					continue;

				foreach (var liveInterval in register.LiveIntervals)
				{
					foreach (var use in liveInterval.UsePositions)
					{
						Context context = new Context(InstructionSet, use.Index);
						AssignPhysicalRegistersToInstructions(context, register.VirtualRegisterOperand, liveInterval.AssignedPhysicalOperand == null ? liveInterval.VirtualRegister.SpillSlotOperand : liveInterval.AssignedPhysicalOperand);
					}

					foreach (var def in liveInterval.DefPositions)
					{
						Context context = new Context(InstructionSet, def.Index);
						AssignPhysicalRegistersToInstructions(context, register.VirtualRegisterOperand, liveInterval.AssignedPhysicalOperand == null ? liveInterval.VirtualRegister.SpillSlotOperand : liveInterval.AssignedPhysicalOperand);
					}
				}
			}
		}

		protected void AssignPhysicalRegistersToInstructions(Context context, Operand old, Operand replacement)
		{
			for (int i = 0; i < context.OperandCount; i++)
			{
				var operand = context.GetOperand(i);

				if (operand.IsVirtualRegister)
				{
					if (operand == old)
					{
						context.SetOperand(i, replacement);
						continue;
					}
				}
				else if (operand.IsMemoryAddress && operand.OffsetBase != null)
				{
					if (operand.OffsetBase == old)
					{
						// FIXME: Creates a lot of duplicate single operands
						context.SetOperand(i, Operand.CreateMemoryAddress(operand.Type, replacement, operand.Displacement));
					}
				}
			}

			for (int i = 0; i < context.ResultCount; i++)
			{
				var operand = context.GetResult(i);

				if (operand.IsVirtualRegister)
				{
					if (operand == old)
					{
						context.SetResult(i, replacement);
						continue;
					}
				}
				else if (operand.IsMemoryAddress && operand.OffsetBase != null)
				{
					if (operand.OffsetBase == old)
					{
						// FIXME: Creates a lot of duplicate single operands
						context.SetResult(i, Operand.CreateMemoryAddress(operand.Type, replacement, operand.Displacement));
					}
				}
			}
		}

		protected void ResolveDataFlow()
		{
			var resolverTrace = new CompilerTrace(Trace, "ResolveDataFlow");

			MoveResolver[,] moveResolvers = new MoveResolver[2, BasicBlocks.Count];

			foreach (var from in ExtendedBlocks)
			{
				foreach (var nextBlock in from.BasicBlock.NextBlocks)
				{
					var to = ExtendedBlocks[nextBlock.Sequence];

					// determine where to insert resolving moves
					bool fromAnchorFlag = (from.BasicBlock.NextBlocks.Count == 1);

					ExtendedBlock anchor = fromAnchorFlag ? from : to;

					MoveResolver moveResolver = moveResolvers[fromAnchorFlag ? 0 : 1, anchor.Sequence];

					if (moveResolver == null)
					{
						moveResolver = new MoveResolver(anchor.BasicBlock, from.BasicBlock, to.BasicBlock);
						moveResolvers[fromAnchorFlag ? 0 : 1, anchor.Sequence] = moveResolver;
					}

					foreach (var virtualRegister in GetVirtualRegisters(to.LiveIn))
					{
						//if (virtualRegister.IsPhysicalRegister)
						//continue;

						var fromLiveInterval = virtualRegister.GetIntervalAtOrEndsAt(from.End);
						var toLiveInterval = virtualRegister.GetIntervalAt(to.Start);

						Debug.Assert(fromLiveInterval != null);
						Debug.Assert(toLiveInterval != null);

						if (fromLiveInterval.AssignedPhysicalRegister != toLiveInterval.AssignedPhysicalRegister)
						{
							if (resolverTrace.Active)
							{
								resolverTrace.Log("REGISTER: " + fromLiveInterval.VirtualRegister.ToString());
								resolverTrace.Log("    FROM: " + from.ToString().PadRight(7) + " " + fromLiveInterval.AssignedOperand.ToString());
								resolverTrace.Log("      TO: " + to.ToString().PadRight(7) + " " + toLiveInterval.AssignedOperand.ToString());

								resolverTrace.Log("  INSERT: " + (fromAnchorFlag ? "FROM (bottom)" : "TO (top)") + ((toLiveInterval.AssignedPhysicalOperand == null) ? "  ****SKIPPED***" : string.Empty));
								resolverTrace.Log("");
							}

							// interval was spilled (spill moves are inserted elsewhere)
							if (toLiveInterval.AssignedPhysicalOperand == null)
								continue;

							Debug.Assert(from.BasicBlock.NextBlocks.Count == 1 || to.BasicBlock.PreviousBlocks.Count == 1);

							moveResolver.AddMove(fromLiveInterval.AssignedOperand, toLiveInterval.AssignedOperand);
						}
					}
				}
			}

			for (int b = 0; b < BasicBlocks.Count; b++)
			{
				for (int fromTag = 0; fromTag < 2; fromTag++)
				{
					MoveResolver moveResolver = moveResolvers[fromTag, b];

					if (moveResolver == null)
						continue;

					moveResolver.InsertResolvingMoves(Architecture, InstructionSet);
				}
			}
		}

		protected void InsertRegisterMoves()
		{
			var insertTrace = new CompilerTrace(Trace, "InsertRegisterMoves");

			// collect edge slot indexes
			Dictionary<SlotIndex, ExtendedBlock> blockEdges = new Dictionary<SlotIndex, ExtendedBlock>();

			foreach (var block in ExtendedBlocks)
			{
				blockEdges.Add(block.Start, block);
				blockEdges.Add(block.End, block);
			}

			foreach (var virtualRegister in VirtualRegisters)
			{
				if (virtualRegister.IsPhysicalRegister)
					continue;

				if (virtualRegister.LiveIntervals.Count <= 1)
					continue;

				foreach (var currentInterval in virtualRegister.LiveIntervals)
				{
					if (blockEdges.ContainsKey(currentInterval.End))
						continue;

					// List is not sorted, so scan thru each one
					foreach (var nextInterval in virtualRegister.LiveIntervals)
					{
						if (nextInterval.Start != currentInterval.End)
							continue;

						// next interval is stack - stores to stack are done elsewhere
						if (nextInterval.AssignedPhysicalOperand == null)
							break;

						// check if source and destination operands of the move are the same
						if (nextInterval.AssignedOperand == currentInterval.AssignedOperand ||
							nextInterval.AssignedOperand.Register == currentInterval.AssignedOperand.Register)
							break;

						Context context = new Context(InstructionSet, currentInterval.End.Index);

						if (!currentInterval.End.IsOnHalfStepForward)
						{
							context.GotoPrevious();

							while (context.IsEmpty || context.Instruction.FlowControl == FlowControl.UnconditionalBranch || context.Instruction.FlowControl == FlowControl.ConditionalBranch || context.Instruction.FlowControl == FlowControl.Return)
							{
								context.GotoPrevious();
							}
						}

						Architecture.InsertMoveInstruction(context,
							nextInterval.AssignedOperand,
							currentInterval.AssignedOperand
						);

						context.Marked = true;

						if (insertTrace.Active)
						{
							insertTrace.Log("REGISTER: " + virtualRegister.ToString());
							insertTrace.Log("POSITION: " + currentInterval.End.ToString());
							insertTrace.Log("    FROM: " + currentInterval.AssignedOperand.ToString());
							insertTrace.Log("      TO: " + nextInterval.AssignedOperand.ToString());

							insertTrace.Log("");
						}

						break;
					}
				}
			}
		}
	}
}