/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Trace;
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

		private readonly List<SlotIndex> KillAll;

		protected readonly ITraceFactory TraceFactory;

		protected readonly TraceLog Trace;

		public BaseRegisterAllocator(BasicBlocks basicBlocks, VirtualRegisters compilerVirtualRegisters, StackLayout stackLayout, BaseArchitecture architecture, ITraceFactory traceFactory)
		{
			this.TraceFactory = traceFactory;

			this.BasicBlocks = basicBlocks;
			this.StackLayout = stackLayout;
			this.Architecture = architecture;

			this.VirtualRegisterCount = compilerVirtualRegisters.Count;
			this.PhysicalRegisterCount = architecture.RegisterSet.Length;
			this.RegisterCount = VirtualRegisterCount + PhysicalRegisterCount;

			this.LiveIntervalTracks = new List<LiveIntervalTrack>(PhysicalRegisterCount);
			this.VirtualRegisters = new List<VirtualRegister>(RegisterCount);
			this.ExtendedBlocks = new List<ExtendedBlock>(basicBlocks.Count);

			this.Trace = CreateTrace("Main");

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

			KillAll = new List<SlotIndex>();
		}

		protected TraceLog CreateTrace(string name)
		{
			var sectionTrace = TraceFactory.CreateTraceLog(name);
			return sectionTrace;
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

			// Assign physical registers
			AssignRegisters();

			// Insert register moves
			InsertRegisterMoves();

			// Insert spill moves
			InsertSpillMoves();

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
			var extendedBlockTrace = CreateTrace("Extended Blocks");

			if (!extendedBlockTrace.Active)
				return;

			foreach (var block in ExtendedBlocks)
			{
				extendedBlockTrace.Log("Block # " + block.BasicBlock.Sequence.ToString() + " (" + block.Start + " destination " + block.End + ")");
				extendedBlockTrace.Log(" LiveIn:   " + ToString(block.LiveIn));
				extendedBlockTrace.Log(" LiveGen:  " + ToString(block.LiveGen));
				extendedBlockTrace.Log(" LiveKill: " + ToString(block.LiveKill));
				extendedBlockTrace.Log(" LiveOut:  " + ToString(block.LiveOut));
			}
		}

		private void TraceLiveIntervals(string stage, bool operand)
		{
			var registerTrace = CreateTrace(stage);

			if (!registerTrace.Active)
				return;

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
					sb.Append("[" + liveInterval.Start.ToString() + "," + liveInterval.End.ToString() + "]/" + liveInterval.AssignedOperand.ToString() + ",");
				else
					sb.Append("[" + liveInterval.Start.ToString() + "," + liveInterval.End.ToString() + "],");
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

		public static void NumberInstructions(BasicBlocks basicBlocks)
		{
			int increment = SlotIndex.Increment;
			int index = increment;

			foreach (var block in basicBlocks)
			{
				for (var node = block.First; ; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					node.SlotNumber = index;
					index = index + increment;

					if (node.IsBlockEndInstruction)
						break;
				}

				Debug.Assert(block.Last.SlotNumber != 0);
			}
		}

		private void NumberInstructions()
		{
			NumberInstructions(BasicBlocks);

			foreach (var block in BasicBlocks)
			{
				var start = new SlotIndex(block.First);
				var end = new SlotIndex(block.Last);
				ExtendedBlocks[block.Sequence].Interval = new Interval(start, end);
			}
		}

		private void TraceNumberInstructions()
		{
			var numberTrace = CreateTrace("InstructionNumber");

			if (!numberTrace.Active)
				return;

			int index = SlotIndex.Increment;

			foreach (var block in BasicBlocks)
			{
				for (var node = block.First; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					if (node.IsBlockStartInstruction)
					{
						numberTrace.Log(node.SlotNumber.ToString() + " = " + node.ToString() + " # " + block.ToString());
					}
					else
					{
						numberTrace.Log(node.SlotNumber.ToString() + " = " + node.ToString());
					}
				}
			}
		}

		private void ComputeLocalLiveSets()
		{
			var liveSetTrace = CreateTrace("ComputeLocalLiveSets");

			foreach (var block in ExtendedBlocks)
			{
				if (liveSetTrace.Active)
					liveSetTrace.Log("Block # " + block.BasicBlock.Sequence.ToString());

				BitArray liveGen = new BitArray(RegisterCount, false);
				BitArray liveKill = new BitArray(RegisterCount, false);

				liveGen.Set(StackFrameRegister.Index, true);
				liveGen.Set(StackPointerRegister.Index, true);

				if (ProgramCounter != null)
				{
					liveGen.Set(ProgramCounter.Index, true);
				}

				if (BasicBlocks.HeadBlocks.Contains(block.BasicBlock))
				{
					for (int s = 0; s < PhysicalRegisterCount; s++)
					{
						liveKill.Set(s, true);
					}

					if (liveSetTrace.Active)
						liveSetTrace.Log("KILL ALL PHYSICAL");
				}

				for (var node = block.BasicBlock.First; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					if (liveSetTrace.Active)
						liveSetTrace.Log(node.ToString());

					var visitor = new OperandVisitor(node);

					foreach (var op in visitor.Input)
					{
						if (op.IsCPURegister && op.Register.IsSpecial)
							continue;

						if (liveSetTrace.Active)
							liveSetTrace.Log("INPUT:  " + op.ToString());

						int index = GetIndex(op);
						if (!liveKill.Get(index))
						{
							liveGen.Set(index, true);

							if (liveSetTrace.Active)
								liveSetTrace.Log("GEN:  " + index.ToString() + " " + op.ToString());
						}
					}

					if (node.Instruction.FlowControl == FlowControl.Call || node.Instruction == IRInstruction.KillAll)
					{
						for (int s = 0; s < PhysicalRegisterCount; s++)
						{
							liveKill.Set(s, true);
						}

						if (liveSetTrace.Active)
							liveSetTrace.Log("KILL ALL PHYSICAL");
					}
					else if (node.Instruction == IRInstruction.KillAllExcept)
					{
						var except = node.Operand1.Register.Index;

						for (int s = 0; s < PhysicalRegisterCount; s++)
						{
							if (s != except)
							{
								liveKill.Set(s, true);
							}
						}

						if (liveSetTrace.Active)
							liveSetTrace.Log("KILL EXCEPT PHYSICAL: " + node.Operand1.ToString());
					}

					foreach (var op in visitor.Output)
					{
						if (op.IsCPURegister && op.Register.IsSpecial)
							continue;

						if (liveSetTrace.Active)
							liveSetTrace.Log("OUTPUT: " + op.ToString());

						int index = GetIndex(op);
						liveKill.Set(index, true);

						if (liveSetTrace.Active)
							liveSetTrace.Log("KILL: " + index.ToString() + " " + op.ToString());
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
			var intervalTrace = CreateTrace("BuildLiveIntervals");

			for (int b = BasicBlocks.Count - 1; b >= 0; b--)
			{
				var block = ExtendedBlocks[b];

				block.BasicBlock.DebugCheck();

				if (intervalTrace.Active)
					intervalTrace.Log("Block # " + block.BasicBlock.Sequence.ToString());

				for (int r = 0; r < RegisterCount; r++)
				{
					if (!block.LiveOut.Get(r))
						continue;

					var register = VirtualRegisters[r];

					if (b + 1 != BasicBlocks.Count && ExtendedBlocks[b + 1].LiveIn.Get(r))
					{
						if (intervalTrace.Active) intervalTrace.Log("Add (LiveOut) " + register.ToString() + " : " + block.Start + " destination " + ExtendedBlocks[b + 1].Start);
						if (intervalTrace.Active) intervalTrace.Log("   Before: " + LiveIntervalsToString(register.LiveIntervals));
						register.AddLiveInterval(block.Start, ExtendedBlocks[b + 1].Start);
						if (intervalTrace.Active) intervalTrace.Log("    After: " + LiveIntervalsToString(register.LiveIntervals));
					}
					else
					{
						if (intervalTrace.Active) intervalTrace.Log("Add (!LiveOut) " + register.ToString() + " : " + block.Interval.Start + " destination " + block.Interval.End);
						if (intervalTrace.Active) intervalTrace.Log("   Before: " + LiveIntervalsToString(register.LiveIntervals));
						register.AddLiveInterval(block.Interval);
						if (intervalTrace.Active) intervalTrace.Log("    After: " + LiveIntervalsToString(register.LiveIntervals));
					}
				}

				block.BasicBlock.DebugCheck();

				for (var node = block.BasicBlock.Last; !node.IsBlockStartInstruction; node = node.Previous)
				{
					if (node.IsEmpty)
						continue;

					var slotIndex = new SlotIndex(node);

					var visitor = new OperandVisitor(node);

					if (node.Instruction.FlowControl == FlowControl.Call || node.Instruction == IRInstruction.KillAll)
					{
						for (int s = 0; s < PhysicalRegisterCount; s++)
						{
							var register = VirtualRegisters[s];
							if (intervalTrace.Active) intervalTrace.Log("Add (Call) " + register.ToString() + " : " + slotIndex + " destination " + slotIndex.HalfStepForward);
							if (intervalTrace.Active) intervalTrace.Log("   Before: " + LiveIntervalsToString(register.LiveIntervals));
							register.AddLiveInterval(slotIndex, slotIndex.HalfStepForward);
							if (intervalTrace.Active) intervalTrace.Log("    After: " + LiveIntervalsToString(register.LiveIntervals));
						}

						KillAll.Add(slotIndex);
					}
					else if (node.Instruction == IRInstruction.KillAllExcept)
					{
						for (int s = 0; s < PhysicalRegisterCount; s++)
						{
							var except = node.Operand1.Register.Index;

							if (s == except)
								continue;

							var register = VirtualRegisters[s];
							if (intervalTrace.Active) intervalTrace.Log("Add (Call) " + register.ToString() + " : " + slotIndex + " destination " + slotIndex.HalfStepForward);
							if (intervalTrace.Active) intervalTrace.Log("   Before: " + LiveIntervalsToString(register.LiveIntervals));
							register.AddLiveInterval(slotIndex, slotIndex.HalfStepForward);
							if (intervalTrace.Active) intervalTrace.Log("    After: " + LiveIntervalsToString(register.LiveIntervals));
						}
					}

					foreach (var result in visitor.Output)
					{
						if (result.IsCPURegister && result.Register.IsSpecial)
							continue;

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
							if (intervalTrace.Active) intervalTrace.Log("Replace First " + register.ToString() + " : " + slotIndex + " destination " + first.End);
							if (intervalTrace.Active) intervalTrace.Log("   Before: " + LiveIntervalsToString(register.LiveIntervals));
							register.FirstRange = new LiveInterval(register, slotIndex, first.End);
							if (intervalTrace.Active) intervalTrace.Log("    After: " + LiveIntervalsToString(register.LiveIntervals));
						}
						else
						{
							// This is necesary to handled a result that is never used!
							// Common with instructions with more than one result
							if (intervalTrace.Active) intervalTrace.Log("Add (Unused) " + register.ToString() + " : " + slotIndex + " destination " + slotIndex);
							if (intervalTrace.Active) intervalTrace.Log("   Before: " + LiveIntervalsToString(register.LiveIntervals));
							register.AddLiveInterval(slotIndex, slotIndex.HalfStepForward);
							if (intervalTrace.Active) intervalTrace.Log("    After: " + LiveIntervalsToString(register.LiveIntervals));
						}
					}

					foreach (var result in visitor.Input)
					{
						if (result.IsCPURegister && result.Register.IsSpecial)
							continue;

						var register = VirtualRegisters[GetIndex(result)];

						if (register.IsReserved)
							continue;

						if (!register.IsPhysicalRegister)
						{
							register.AddUsePosition(slotIndex);
						}

						if (intervalTrace.Active) intervalTrace.Log("Add (normal) " + register.ToString() + " : " + block.Start + " destination " + slotIndex);
						if (intervalTrace.Active) intervalTrace.Log("   Before: " + LiveIntervalsToString(register.LiveIntervals));
						register.AddLiveInterval(block.Start, slotIndex);
						if (intervalTrace.Active) intervalTrace.Log("    After: " + LiveIntervalsToString(register.LiveIntervals));
					}
				}
			}
		}

		private ExtendedBlock GetContainingBlock(SlotIndex slotIndex)
		{
			// FUTURE: Cache results
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

		protected void CalculateSpillCosts(IList<LiveInterval> liveIntervals)
		{
			foreach (var liveInterval in liveIntervals)
			{
				CalculateSpillCost(liveInterval);
			}
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

		protected void AddPriorityQueue(IList<LiveInterval> liveIntervals)
		{
			foreach (var liveInterval in liveIntervals)
			{
				AddPriorityQueue(liveInterval);
			}
		}

		protected void AddPriorityQueue(LiveInterval liveInterval)
		{
			//Debug.Assert(!liveInterval.IsSplit);
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

			if (Trace.Active) Trace.Log("  Assigned live interval destination: " + track.ToString());

			track.Add(liveInterval);

			//if (Trace.Active) Trace.Log("    Track: " + track.ToString2());

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
						if (Trace.Active) Trace.Log("  Evicting live newIntervals");

						track.Evict(intersections);

						foreach (var intersection in intersections)
						{
							if (Trace.Active) Trace.Log("  Evicted: " + intersection.ToString());

							liveInterval.Stage = LiveInterval.AllocationStage.Initial;
							AddPriorityQueue(intersection);
						};
					}

					track.Add(liveInterval);

					if (Trace.Active) Trace.Log("  Assigned live interval destination: " + track.ToString());

					return true;
				}
			}

			return false;
		}

		protected virtual bool PlaceLiveInterval(LiveInterval liveInterval)
		{
			// For now, empty intervals will stay spilled
			if (liveInterval.IsEmpty)
			{
				if (Trace.Active) Trace.Log("  Spilled");

				liveInterval.VirtualRegister.IsSpilled = true;
				AddSpilledInterval(liveInterval);

				return true;
			}

			// Find any available track and place interval there
			if (PlaceLiveIntervalOnAnyAvailableTrack(liveInterval))
			{
				return true;
			}

			if (Trace.Active) Trace.Log("  No free register available");

			// No place for live interval; find live interval(s) to evict based on spill costs
			if (PlaceLiveIntervalOnTrackAllowEvictions(liveInterval))
			{
				return true;
			}

			return false;
		}

		private void ProcessLiveInterval(LiveInterval liveInterval)
		{
			Debug.Assert(liveInterval.LiveIntervalTrack == null);
			//Debug.Assert(!liveInterval.IsSplit);

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

			// No live intervals to evict!
			if (Trace.Active) Trace.Log("  No live new to evicts");

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
				if (Trace.Active) Trace.Log("  Attempting destination split interval - level 1");

				if (TrySplitInterval(liveInterval, 1))
				{
					return;
				}

				if (Trace.Active) Trace.Log("  Re-queued for split interval - level 2");

				liveInterval.Stage = LiveInterval.AllocationStage.SplitLevel2;
				AddPriorityQueue(liveInterval);
				return;
			}

			// split live interval - level 2
			if (liveInterval.Stage == LiveInterval.AllocationStage.SplitLevel2)
			{
				if (Trace.Active) Trace.Log("  Attempting destination split interval - level 2");

				if (TrySplitInterval(liveInterval, 2))
				{
					return;
				}

				if (Trace.Active) Trace.Log("  Re-queued for split interval - level 3");

				liveInterval.Stage = LiveInterval.AllocationStage.SplitLevel3;
				AddPriorityQueue(liveInterval);
				return;
			}

			// split live interval - level 3
			if (liveInterval.Stage == LiveInterval.AllocationStage.SplitLevel3)
			{
				if (Trace.Active) Trace.Log("  Attempting destination split interval - level 3");

				if (TrySplitInterval(liveInterval, 3))
				{
					return;
				}

				// Move to final split option stage
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

			// 1. Find the first use or def
			// 2. Determine split location
			// 3. Split the range into two (or maybe more ranges)

			if (liveInterval.IsEmpty)
			{
				SpilledIntervals.Add(liveInterval);
				return;
			}

			if (Trace.Active) Trace.Log(" Splitting around first use/def");

			var liveRange = liveInterval.LiveRange;
			SlotIndex splitAt = null;

			if (liveRange.IsDefFirst)
			{
				if (liveRange.FirstDef == liveRange.Start)
				{
					// must split after def
					splitAt = liveRange.FirstDef.HalfStepForward;
				}
				else
				{
					// split after def
					splitAt = liveRange.FirstDef.HalfStepBack;
				}
			}
			else
			{
				Debug.Assert(liveRange.FirstUse != liveRange.Start);

				if (liveRange.FirstUse == liveRange.End)
				{
					// must split before use
					splitAt = liveRange.FirstUse.HalfStepBack;
				}
				else if (liveRange.FirstUse.HalfStepBack == liveRange.Start)
				{
					splitAt = liveRange.FirstUse.HalfStepForward;
				}
				else
				{
					splitAt = liveRange.FirstUse.HalfStepBack;
				}
			}

			var intervals = liveInterval.SplitAt(splitAt);

			ReplaceIntervals(liveInterval, intervals, true);

			return;
		}

		protected void ReplaceIntervals(LiveInterval replaceLiveInterval, IList<LiveInterval> newIntervals, bool addToQueue)
		{
			CalculateSpillCosts(newIntervals);

			replaceLiveInterval.VirtualRegister.ReplaceWithSplit(replaceLiveInterval, newIntervals);

			if (Trace.Active)
			{
				foreach (var interval in newIntervals)
				{
					Trace.Log(" New Split: " + interval.ToString());
				}
			}

			if (addToQueue)
			{
				AddPriorityQueue(newIntervals);
			}
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
			foreach (SlotIndex slot in KillAll)
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
						var context = new Context(def.Index);

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
						AssignPhysicalRegistersToInstructions(use.Index, register.VirtualRegisterOperand, liveInterval.AssignedPhysicalOperand == null ? liveInterval.VirtualRegister.SpillSlotOperand : liveInterval.AssignedPhysicalOperand);
					}

					foreach (var def in liveInterval.DefPositions)
					{
						AssignPhysicalRegistersToInstructions(def.Index, register.VirtualRegisterOperand, liveInterval.AssignedPhysicalOperand == null ? liveInterval.VirtualRegister.SpillSlotOperand : liveInterval.AssignedPhysicalOperand);
					}
				}
			}
		}

		protected void AssignPhysicalRegistersToInstructions(InstructionNode node, Operand old, Operand replacement)
		{
			for (int i = 0; i < node.OperandCount; i++)
			{
				var operand = node.GetOperand(i);

				if (operand.IsVirtualRegister)
				{
					if (operand == old)
					{
						node.SetOperand(i, replacement);
						continue;
					}
				}
				else if (operand.IsMemoryAddress && operand.OffsetBase != null)
				{
					if (operand.OffsetBase == old)
					{
						// FIXME: Creates a lot of duplicate single operands
						node.SetOperand(i, Operand.CreateMemoryAddress(operand.Type, replacement, operand.Displacement));
					}
				}
			}

			for (int i = 0; i < node.ResultCount; i++)
			{
				var operand = node.GetResult(i);

				if (operand.IsVirtualRegister)
				{
					if (operand == old)
					{
						node.SetResult(i, replacement);
						continue;
					}
				}
				else if (operand.IsMemoryAddress && operand.OffsetBase != null)
				{
					if (operand.OffsetBase == old)
					{
						// FIXME: Creates a lot of duplicate single operands
						node.SetResult(i, Operand.CreateMemoryAddress(operand.Type, replacement, operand.Displacement));
					}
				}
			}
		}

		protected void ResolveDataFlow()
		{
			var resolverTrace = CreateTrace("ResolveDataFlow");

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

								resolverTrace.Log("  INSERT: " + (fromAnchorFlag ? "FROM (bottom)" : "TO (Before)") + ((toLiveInterval.AssignedPhysicalOperand == null) ? "  ****SKIPPED***" : string.Empty));
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
					var moveResolver = moveResolvers[fromTag, b];

					if (moveResolver == null)
						continue;

					moveResolver.InsertResolvingMoves(Architecture);
				}
			}
		}

		protected class MoveKeyedList : KeyedList<SlotIndex, Move>
		{
			public void Add(SlotIndex at, Operand source, Operand destination)
			{
				Add(at, new Move(source, destination));
			}
		}

		protected void InsertRegisterMoves()
		{
			var insertTrace = CreateTrace("InsertRegisterMoves");

			var moves = GetRegisterMoves();

			foreach (var key in moves.Keys)
			{
				var moveResolver = new MoveResolver(key.Index, !key.IsOnHalfStepForward, moves[key]);

				moveResolver.InsertResolvingMoves(Architecture);

				if (insertTrace.Active)
				{
					foreach (var move in moves[key])
					{
						//insertTrace.Log("REGISTER: " + virtualRegister.ToString());
						insertTrace.Log("  AT: " + key.ToString());
						insertTrace.Log("FROM: " + move.Source.ToString());
						insertTrace.Log("  TO: " + move.Destination.ToString());

						insertTrace.Log("");
					}
				}
			}
		}

		protected MoveKeyedList GetRegisterMoves()
		{
			MoveKeyedList keyedList = new MoveKeyedList();

			// collect edge slot indexes
			var blockEdges = new Dictionary<SlotIndex, ExtendedBlock>();

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

						keyedList.Add(currentInterval.End, currentInterval.AssignedOperand, nextInterval.AssignedOperand);

						break;
					}
				}
			}

			return keyedList;
		}
	}
}
