/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
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
	public sealed class GreedyRegisterAllocator
	{
		private const int SlotIncrement = 4;

		private BasicBlocks basicBlocks;
		private InstructionSet instructionSet;
		private StackLayout stackLayout;
		private BaseArchitecture architecture;

		private int virtualRegisterCount;
		private int physicalRegisterCount;
		private int registerCount;
		private List<ExtendedBlock> extendedBlocks;
		private List<VirtualRegister> virtualRegisters;

		private SimpleKeyPriorityQueue<LiveInterval> priorityQueue;
		private List<LiveIntervalUnion> liveIntervalUnions;

		private Register stackFrameRegister;
		private Register stackPointerRegister;
		private Register programCounter;

		private List<LiveInterval> spilledIntervals;

		private List<SlotIndex> callSlots;

		private CompilerTrace trace;

		private sealed class OperandVisitor
		{
			private Context context;

			public OperandVisitor(Context context)
			{
				this.context = context;
			}

			public IEnumerable<Operand> Input
			{
				get
				{
					foreach (Operand operand in context.Operands)
					{
						if (operand.IsVirtualRegister || operand.IsCPURegister)
						{
							yield return operand;
						}
						else if (operand.IsMemoryAddress && operand.OffsetBase != null)
						{
							yield return operand.OffsetBase;
						}
					}

					foreach (Operand operand in context.Results)
					{
						if (operand.IsMemoryAddress && operand.OffsetBase != null)
						{
							yield return operand.OffsetBase;
						}
					}
				}
			}

			public IEnumerable<Operand> Output
			{
				get
				{
					foreach (Operand operand in context.Results)
					{
						if (operand.IsVirtualRegister || operand.IsCPURegister)
						{
							yield return operand;
						}
					}
				}
			}
		}

		public GreedyRegisterAllocator(BasicBlocks basicBlocks, VirtualRegisters compilerVirtualRegisters, InstructionSet instructionSet, StackLayout stackLayout, BaseArchitecture architecture, CompilerTrace trace)
		{
			this.trace = trace;

			this.basicBlocks = basicBlocks;
			this.instructionSet = instructionSet;
			this.stackLayout = stackLayout;
			this.architecture = architecture;

			this.virtualRegisterCount = compilerVirtualRegisters.Count;
			this.physicalRegisterCount = architecture.RegisterSet.Length;
			this.registerCount = virtualRegisterCount + physicalRegisterCount;

			this.liveIntervalUnions = new List<LiveIntervalUnion>(physicalRegisterCount);
			this.virtualRegisters = new List<VirtualRegister>(registerCount);
			this.extendedBlocks = new List<ExtendedBlock>(basicBlocks.Count);

			stackFrameRegister = architecture.StackFrameRegister;
			stackPointerRegister = architecture.StackPointerRegister;
			programCounter = architecture.ProgramCounter;

			// Setup extended physical registers
			foreach (var physicalRegister in architecture.RegisterSet)
			{
				Debug.Assert(physicalRegister.Index == virtualRegisters.Count);
				Debug.Assert(physicalRegister.Index == liveIntervalUnions.Count);

				bool reserved = (physicalRegister == stackFrameRegister
					|| physicalRegister == stackPointerRegister
					|| (programCounter != null && physicalRegister == programCounter));

				this.virtualRegisters.Add(new VirtualRegister(physicalRegister, reserved));
				this.liveIntervalUnions.Add(new LiveIntervalUnion(physicalRegister, reserved));
			}

			// Setup extended virtual registers
			foreach (var virtualRegister in compilerVirtualRegisters)
			{
				Debug.Assert(virtualRegister.Index == virtualRegisters.Count - physicalRegisterCount + 1);

				this.virtualRegisters.Add(new VirtualRegister(virtualRegister));
			}

			priorityQueue = new SimpleKeyPriorityQueue<LiveInterval>();
			spilledIntervals = new List<LiveInterval>();

			callSlots = new List<SlotIndex>();

			Start();
		}

		private void Start()
		{
			// Order all the blocks
			CreateExtendedBlocks();

			// Number all the instructions in block order
			NumberInstructions();

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

			// Split intervals at call sites
			SplitIntervalsAtCallSites();

			// Calculate spill costs for live intervals
			CalculateSpillCosts();

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
			if (!trace.Active)
				return;

			var sectionTrace = new CompilerTrace(trace, "Extended Blocks");

			foreach (var block in extendedBlocks)
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
			if (!trace.Active)
				return;

			var registerTrace = new CompilerTrace(trace, stage);

			foreach (var virtualRegister in virtualRegisters)
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

		private string SlotsToString(IList<SlotIndex> slots)
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

		private int GetIndex(Operand operand)
		{
			return (operand.IsCPURegister) ? (operand.Register.Index) : (operand.Sequence + physicalRegisterCount - 1);
		}

		private void CreateExtendedBlocks()
		{
			var loopAwareBlockOrder = new LoopAwareBlockOrder(this.basicBlocks);

			// The re-ordering is not strictly necessary; however, it reduces "holes" in live ranges.
			// Less "holes" increase readability of the debug logs.
			//basicBlocks.ReorderBlocks(loopAwareBlockOrder.NewBlockOrder);

			// Allocate and setup extended blocks
			for (int i = 0; i < basicBlocks.Count; i++)
			{
				extendedBlocks.Add(new ExtendedBlock(basicBlocks[i], registerCount, loopAwareBlockOrder.GetLoopDepth(basicBlocks[i])));
			}
		}

		private void NumberInstructions()
		{
			var number = new CompilerTrace(trace, "InstructionNumber");

			int index = SlotIncrement;

			foreach (BasicBlock block in basicBlocks)
			{
				if (number.Active)
				{
				}

				for (Context context = new Context(instructionSet, block); ; context.GotoNext())
				{
					if (!context.IsEmpty)
					{
						context.SlotNumber = index;
						index = index + SlotIncrement;

						if (number.Active)
						{
							if (context.IsBlockStartInstruction)
							{
								number.Log(context.SlotNumber.ToString() + " = " + context.ToString() + " # " + block.ToString());
							}
							else
							{
								number.Log(context.SlotNumber.ToString() + " = " + context.ToString());
							}
						}
					}

					if (context.IsBlockEndInstruction)
						break;
				}

				SlotIndex start = new SlotIndex(instructionSet, block.StartIndex);
				SlotIndex end = new SlotIndex(instructionSet, block.EndIndex);
				extendedBlocks[block.Sequence].Interval = new Interval(start, end);
			}
		}

		private void ComputeLocalLiveSets()
		{
			var liveSetTrace = new CompilerTrace(trace, "ComputeLocalLiveSets");

			foreach (var block in extendedBlocks)
			{
				liveSetTrace.Log("Block # " + block.BasicBlock.Sequence.ToString());

				BitArray liveGen = new BitArray(registerCount, false);
				BitArray liveKill = new BitArray(registerCount, false);

				liveGen.Set(stackFrameRegister.Index, true);
				liveGen.Set(stackPointerRegister.Index, true);

				if (programCounter != null)
					liveGen.Set(programCounter.Index, true);

				for (Context context = new Context(instructionSet, block.BasicBlock); !context.IsBlockEndInstruction; context.GotoNext())
				{
					if (context.IsEmpty)
						continue;

					liveSetTrace.Log(context.ToString());

					OperandVisitor visitor = new OperandVisitor(context);

					foreach (var ops in visitor.Input)
					{
						liveSetTrace.Log("INPUT:  " + ops.ToString());
						int index = GetIndex(ops);
						if (!liveKill.Get(index))
						{
							liveGen.Set(index, true);
							liveSetTrace.Log("GEN:  " + index.ToString() + " " + ops.ToString());
						}
					}

					if (context.Instruction.FlowControl == FlowControl.Call)
					{
						for (int s = 0; s < physicalRegisterCount; s++)
						{
							liveKill.Set(s, true);
						}
						liveSetTrace.Log("KILL ALL PHYSICAL");
					}

					foreach (var ops in visitor.Output)
					{
						liveSetTrace.Log("OUTPUT: " + ops.ToString());
						int index = GetIndex(ops);
						liveKill.Set(index, true);
						liveSetTrace.Log("KILL: " + index.ToString() + " " + ops.ToString());
					}
				}

				block.LiveGen = liveGen;
				block.LiveKill = liveKill;
				block.LiveKillNot = ((BitArray)liveKill.Clone()).Not();

				liveSetTrace.Log("GEN:     " + ToString(block.LiveGen));
				liveSetTrace.Log("KILL:    " + ToString(block.LiveKill));
				liveSetTrace.Log("KILLNOT: " + ToString(block.LiveKillNot));

				liveSetTrace.Log(string.Empty);
			}
		}

		private void ComputeGlobalLiveSets()
		{
			bool changed = true;
			while (changed)
			{
				changed = false;

				for (int i = basicBlocks.Count - 1; i >= 0; i--)
				{
					var block = extendedBlocks[i];

					BitArray liveOut = new BitArray(registerCount);

					foreach (var next in block.BasicBlock.NextBlocks)
					{
						liveOut.Or(extendedBlocks[next.Sequence].LiveIn);
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
			var intervalTrace = new CompilerTrace(trace, "BuildLiveIntervals");

			for (int b = basicBlocks.Count - 1; b >= 0; b--)
			{
				var block = extendedBlocks[b];

				for (int r = 0; r < registerCount; r++)
				{
					if (!block.LiveOut.Get(r))
						continue;

					var register = virtualRegisters[r];

					if (b + 1 != basicBlocks.Count && extendedBlocks[b + 1].LiveIn.Get(r))
					{
						if (intervalTrace.Active) intervalTrace.Log("Add (LiveOut) " + register.ToString() + " : " + block.Start + " to " + extendedBlocks[b + 1].Start);
						if (intervalTrace.Active) intervalTrace.Log("   Before: " + LiveIntervalsToString(register.LiveIntervals));
						register.AddLiveInterval(block.Start, extendedBlocks[b + 1].Start);
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

				Context context = new Context(instructionSet, block.BasicBlock, block.BasicBlock.EndIndex);

				while (!context.IsBlockStartInstruction)
				{
					if (!context.IsEmpty)
					{
						SlotIndex slotIndex = new SlotIndex(context);

						OperandVisitor visitor = new OperandVisitor(context);

						if (context.Instruction.FlowControl == FlowControl.Call)
						{
							SlotIndex nextSlotIndex = slotIndex.Next;

							for (int s = 0; s < physicalRegisterCount; s++)
							{
								var register = virtualRegisters[s];
								if (intervalTrace.Active) intervalTrace.Log("Add (Call) " + register.ToString() + " : " + slotIndex + " to " + nextSlotIndex);
								if (intervalTrace.Active) intervalTrace.Log("   Before: " + LiveIntervalsToString(register.LiveIntervals));
								register.AddLiveInterval(slotIndex, nextSlotIndex);
								if (intervalTrace.Active) intervalTrace.Log("    After: " + LiveIntervalsToString(register.LiveIntervals));
							}

							callSlots.Add(slotIndex);
						}

						foreach (var result in visitor.Output)
						{
							var register = virtualRegisters[GetIndex(result)];

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
								if (intervalTrace.Active) intervalTrace.Log("Add (Unused) " + register.ToString() + " : " + slotIndex + " to " + slotIndex.Next);
								if (intervalTrace.Active) intervalTrace.Log("   Before: " + LiveIntervalsToString(register.LiveIntervals));
								register.AddLiveInterval(slotIndex, slotIndex.Next);
								if (intervalTrace.Active) intervalTrace.Log("    After: " + LiveIntervalsToString(register.LiveIntervals));
							}
						}

						foreach (var result in visitor.Input)
						{
							var register = virtualRegisters[GetIndex(result)];

							if (register.IsReserved)
								continue;

							if (!register.IsPhysicalRegister)
							{
								register.AddUsePosition(slotIndex);
							}

							if (intervalTrace.Active) intervalTrace.Log("Add (normal) " + register.ToString() + " : " + block.Start + " to " + slotIndex.Next);
							if (intervalTrace.Active) intervalTrace.Log("   Before: " + LiveIntervalsToString(register.LiveIntervals));
							register.AddLiveInterval(block.Start, slotIndex.Next);
							if (intervalTrace.Active) intervalTrace.Log("    After: " + LiveIntervalsToString(register.LiveIntervals));
						}
					}

					context.GotoPrevious();
				}
			}
		}

		private ExtendedBlock GetContainingBlock(SlotIndex slotIndex)
		{
			foreach (var block in extendedBlocks)
			{
				if (block.Contains(slotIndex))
				{
					return block;
				}
			}

			Debug.Assert(false, "GetContainingBlock");
			return null;
		}

		private int GetLoopDepth(SlotIndex slotIndex)
		{
			return GetContainingBlock(slotIndex).LoopDepth;
		}

		private SlotIndex GetBlockEnd(SlotIndex slotIndex)
		{
			return GetContainingBlock(slotIndex).End;
		}

		private SlotIndex GetBlockStart(SlotIndex slotIndex)
		{
			return GetContainingBlock(slotIndex).Start;
		}

		private void CalculateSpillCost(LiveInterval liveInterval)
		{
			int spillvalue = 0;

			foreach (var use in liveInterval.UsePositions)
			{
				spillvalue += (100 * SlotIncrement) * (1 + GetLoopDepth(use));
			}

			foreach (var use in liveInterval.DefPositions)
			{
				spillvalue += (115 * SlotIncrement) * (1 + GetLoopDepth(use));
			}

			liveInterval.SpillValue = spillvalue * 100;
		}

		private void CalculateSpillCosts()
		{
			foreach (var virtualRegister in virtualRegisters)
			{
				foreach (var liveInterval in virtualRegister.LiveIntervals)
				{
					// Skip adding live intervals for physical registers to priority queue
					if (liveInterval.VirtualRegister.IsPhysicalRegister)
					{
						// fixed and not spillable
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

		private void AddPriorityQueue(LiveInterval liveInterval)
		{
			// priority is based on allocation stage (primary, lower first) and interval size (secondary, higher first)
			priorityQueue.Enqueue(liveInterval.Length | ((int)(((int)LiveInterval.AllocationStage.Max - liveInterval.Stage)) << 28), liveInterval);
		}

		private void PopulatePriorityQueue()
		{
			foreach (var virtualRegister in virtualRegisters)
			{
				foreach (var liveInterval in virtualRegister.LiveIntervals)
				{
					// Skip adding live intervals for physical registers to priority queue
					if (liveInterval.VirtualRegister.IsPhysicalRegister)
					{
						liveIntervalUnions[liveInterval.VirtualRegister.PhysicalRegister.Index].Add(liveInterval);

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
			while (!priorityQueue.IsEmpty)
			{
				var liveInterval = priorityQueue.Dequeue();

				ProcessLiveInterval(liveInterval);
			}
		}

		private void ProcessLiveInterval(LiveInterval liveInterval)
		{
			if (trace.Active)
			{
				trace.Log("Processing Interval: " + liveInterval.ToString() + " / Length: " + liveInterval.Length.ToString() + " / Spill cost: " + liveInterval.SpillCost.ToString() + " / Stage: " + liveInterval.Stage.ToString());
				trace.Log("  Defs (" + liveInterval.DefPositions.Count.ToString() + "): " + SlotsToString(liveInterval.DefPositions));
				trace.Log("  Uses (" + liveInterval.UsePositions.Count.ToString() + "): " + SlotsToString(liveInterval.UsePositions));
			}

			if (liveInterval.ForceSpilled)
			{
				if (trace.Active) trace.Log("  Forced spilled interval");

				liveInterval.VirtualRegister.IsSpilled = true;
				spilledIntervals.Add(liveInterval);
				return;
			}

			// For now, empty intervals will stay spilled
			if (liveInterval.IsEmpty)
			{
				if (trace.Active) trace.Log("  Spilled");

				liveInterval.VirtualRegister.IsSpilled = true;
				spilledIntervals.Add(liveInterval);

				return;
			}

			// Find an available live interval union to place this live interval
			foreach (var liveIntervalUnion in liveIntervalUnions)
			{
				if (liveIntervalUnion.IsReserved)
					continue;

				if (liveIntervalUnion.IsFloatingPoint != liveInterval.VirtualRegister.IsFloatingPoint)
					continue;

				if (!liveIntervalUnion.Intersects(liveInterval))
				{
					if (trace.Active) trace.Log("  Assigned live interval to: " + liveIntervalUnion.ToString());

					liveIntervalUnion.Add(liveInterval);
					return;
				}
			}

			if (trace.Active) trace.Log("  No free register available");

			// No place for live interval; find live interval(s) to evict based on spill costs
			foreach (var liveIntervalUnion in liveIntervalUnions)
			{
				if (liveIntervalUnion.IsReserved)
					continue;

				if (liveIntervalUnion.IsFloatingPoint != liveInterval.VirtualRegister.IsFloatingPoint)
					continue;

				var intersections = liveIntervalUnion.GetIntersections(liveInterval);

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
					if (trace.Active) trace.Log("  Evicting live intervals");

					liveIntervalUnion.Evict(intersections);

					foreach (var intersection in intersections)
					{
						if (trace.Active) trace.Log("  Evicted: " + intersection.ToString());

						liveInterval.Stage = LiveInterval.AllocationStage.Initial;
						AddPriorityQueue(intersection);
					};

					liveIntervalUnion.Add(liveInterval);

					if (trace.Active) trace.Log("  Assigned live interval to: " + liveIntervalUnion.ToString());

					return;
				}
			}

			if (trace.Active) trace.Log("  No live intervals to evicts");

			// No live intervals to evict!

			// prepare to split live interval
			if (liveInterval.Stage == LiveInterval.AllocationStage.Initial)
			{
				if (trace.Active) trace.Log("  Re-queued for split level 1 stage");
				liveInterval.Stage = LiveInterval.AllocationStage.SplitLevel1;
				AddPriorityQueue(liveInterval);
				return;
			}

			// split live interval - level 1
			if (liveInterval.Stage == LiveInterval.AllocationStage.SplitLevel1)
			{
				if (trace.Active) trace.Log("  Attempting to split interval - level 1");

				if (TrySplitInterval(liveInterval, 1))
				{
					return;
				}

				// Move to split level 2 stage
				if (trace.Active) trace.Log("  Re-queued for split interval - level 2");

				liveInterval.Stage = LiveInterval.AllocationStage.SplitLevel2;
				AddPriorityQueue(liveInterval);
				return;
			}

			// split live interval - level 2
			if (liveInterval.Stage == LiveInterval.AllocationStage.SplitLevel2)
			{
				if (trace.Active) trace.Log("  Attempting to split interval - level 2");

				if (TrySplitInterval(liveInterval, 2))
				{
					return;
				}

				// Move to spill stage
				if (trace.Active) trace.Log("  Re-queued for spillable stage");

				liveInterval.Stage = LiveInterval.AllocationStage.Spillable;
				AddPriorityQueue(liveInterval);
				return;
			}

			// spill interval to stack slot
			if (liveInterval.Stage == LiveInterval.AllocationStage.Spillable)
			{
				if (trace.Active) trace.Log("  Spilled interval");

				//liveInterval.Stage = LiveInterval.AllocationStage.Spilled
				liveInterval.VirtualRegister.IsSpilled = true;
				spilledIntervals.Add(liveInterval);

				return;
			}

			// TODO
			return;
		}

		private bool TrySplitInterval(LiveInterval liveInterval, int level)
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

		private bool TrySimplePartialFreeIntervalSplit(LiveInterval liveInterval)
		{
			SlotIndex furthestUsed = null;

			foreach (var liveIntervalUnion in liveIntervalUnions)
			{
				if (liveIntervalUnion.IsReserved)
					continue;

				if (liveIntervalUnion.IsFloatingPoint != liveInterval.VirtualRegister.IsFloatingPoint)
					continue;

				var next = liveIntervalUnion.GetNextLiveRange(liveInterval.Start);

				if (next == null)
					continue;

				Debug.Assert(next > liveInterval.Start);

				if (trace.Active) trace.Log("  Register " + liveIntervalUnion.ToString() + " free up to " + next.ToString());

				if (furthestUsed == null || furthestUsed < next)
					furthestUsed = next;
			}

			if (furthestUsed == null)
			{
				if (trace.Active) trace.Log("  No partial free space available");
				return false;
			}

			if (furthestUsed < liveInterval.Minimum)
			{
				if (trace.Active) trace.Log("  No partial free space available");
				return false;
			}

			//Debug.Assert(!furthestUsed.IsBlockStartInstruction);
			if (furthestUsed.IsBlockStartInstruction)
			{
				return false;
			}

			var lastFree = furthestUsed.Previous;

			if (lastFree <= liveInterval.Start)
			{
				if (trace.Active) trace.Log("  No partial free space available");
				return false;
			}

			if (trace.Active) trace.Log("  Partial free up to: " + furthestUsed.ToString());

			var low = GetLowOptimalSplitLocation(liveInterval, lastFree, true);
			var high = GetHighOptimalSplitLocation(liveInterval, lastFree, true);

			var first = liveInterval.Split(liveInterval.Start, low);
			var last = liveInterval.Split(high, liveInterval.End);
			var middle = (low != high) ? liveInterval.Split(low, high) : null;

			if (trace.Active) trace.Log("  Low Split   : " + first.ToString());
			if (trace.Active) trace.Log("  Middle Split: " + (middle == null ? "n/a" : middle.ToString()));
			if (trace.Active) trace.Log("  High Split  : " + last.ToString());

			var virtualRegister = liveInterval.VirtualRegister;

			CalculateSpillCost(first);
			CalculateSpillCost(last);

			virtualRegister.Remove(liveInterval);
			virtualRegister.Add(first);
			virtualRegister.Add(last);

			AddPriorityQueue(first);
			AddPriorityQueue(last);

			if (middle != null)
			{
				//middle.ForceSpilled = true;
				CalculateSpillCost(middle);
				virtualRegister.Add(middle);
				AddPriorityQueue(middle);
			}

			return true;
		}

		private bool IntervalSplitAtFirstUseOrDef(LiveInterval liveInterval)
		{
			// last resort

			if (liveInterval.IsEmpty)
				return false;

			SlotIndex at;

			var firstUse = liveInterval.UsePositions.Count != 0 ? liveInterval.UsePositions[0] : null;
			var firstDef = liveInterval.DefPositions.Count != 0 ? liveInterval.DefPositions[0] : null;

			at = firstDef;

			if (at == null)
				at = firstUse;
			else if (firstUse != null && firstUse < at)
				at = firstUse;

			var atNext = at.Next;

			if (liveInterval.Start == at && liveInterval.End == atNext)
			{
				if (trace.Active) trace.Log("  Interval already smallest");
				return false;
			}

			if (trace.Active) trace.Log(" Splitting around first use/def");

			var virtualRegister = liveInterval.VirtualRegister;

			virtualRegister.Remove(liveInterval);

			if (liveInterval.Start != at)
			{
				var first = liveInterval.Split(liveInterval.Start, at);
				CalculateSpillCost(first);
				virtualRegister.Add(first);
				AddPriorityQueue(first);
				if (trace.Active) trace.Log("  First Split : " + first.ToString());
			}

			var middle = liveInterval.Split(at, atNext);
			CalculateSpillCost(middle);
			virtualRegister.Add(middle);
			AddPriorityQueue(middle);
			if (trace.Active) trace.Log("  Middle Split: " + middle.ToString());

			if (liveInterval.End != atNext)
			{
				var last = liveInterval.Split(atNext, liveInterval.End);
				CalculateSpillCost(last);
				virtualRegister.Add(last);
				AddPriorityQueue(last);
				if (trace.Active) trace.Log("  Last Split  : " + last.ToString());
			}

			return true;
		}

		private IEnumerable<VirtualRegister> GetVirtualRegisters(BitArray array)
		{
			for (int i = 0; i < array.Count; i++)
			{
				if (array.Get(i))
				{
					var virtualRegister = virtualRegisters[i];
					if (!virtualRegister.IsPhysicalRegister)
						yield return virtualRegister;
				}
			}
		}

		private SlotIndex GetLowOptimalSplitLocation(LiveInterval liveInterval, SlotIndex from, bool check)
		{
			Debug.Assert(liveInterval.Start != from);

			if (trace.Active) trace.Log("--Low Splitting: " + liveInterval.ToString() + " at: " + from.ToString());

			if (check)
			{
				if (liveInterval.UsePositions.Contains(from) || liveInterval.DefPositions.Contains(from))
				{
					if (trace.Active) trace.Log("  No optimal. Split at: " + from.ToString());
					return from;
				}
			}

			SlotIndex blockStart = GetBlockStart(from);
			if (trace.Active) trace.Log("  Block Start : " + blockStart.ToString());

			SlotIndex lowerBound = blockStart > liveInterval.Start ? blockStart : liveInterval.Start.Next;
			if (trace.Active) trace.Log("  Lower Bound : " + lowerBound.ToString());

			SlotIndex previousUse = liveInterval.GetPreviousDefOrUsePosition(from);
			if (trace.Active) trace.Log("  Previous Use: " + (previousUse != null ? previousUse.ToString() : "null"));

			if (previousUse != null && previousUse >= lowerBound)
			{
				if (trace.Active) trace.Log("  Low Optimal : " + previousUse.Next.ToString());
				return previousUse.Next;
			}

			if (trace.Active) trace.Log("  Low Optimal : " + lowerBound.ToString());
			return lowerBound;
		}

		private SlotIndex GetHighOptimalSplitLocation(LiveInterval liveInterval, SlotIndex after, bool check)
		{
			Debug.Assert(liveInterval.End != after);

			if (trace.Active) trace.Log("--High Splitting: " + liveInterval.ToString() + " at: " + after.ToString());

			SlotIndex blockEnd = GetBlockEnd(after);
			if (trace.Active) trace.Log("  Block End   : " + blockEnd.ToString());

			SlotIndex higherBound = blockEnd < liveInterval.End ? blockEnd : liveInterval.End.Previous;
			if (trace.Active) trace.Log("  Higher Bound: " + higherBound.ToString());

			SlotIndex nextUse = liveInterval.GetNextDefOrUsePosition(after);
			if (trace.Active) trace.Log("  Next Use    : " + (nextUse != null ? nextUse.ToString() : "null"));

			if (nextUse != null && nextUse <= higherBound)
			{
				if (trace.Active) trace.Log("  High Optimal: " + nextUse.Previous.ToString());
				return nextUse;
			}

			if (trace.Active) trace.Log("  High Optimal: " + higherBound.ToString());
			return higherBound;
		}

		private SlotIndex FindCallSiteInInterval(LiveInterval liveInterval)
		{
			foreach (SlotIndex slot in callSlots)
			{
				if (liveInterval.Contains(slot))
					return slot;
			}
			return null;
		}

		private void SplitIntervalsAtCallSites()
		{
			foreach (var virtualRegister in virtualRegisters)
			{
				if (virtualRegister.IsPhysicalRegister)
					continue;

				for (int i = 0; i < virtualRegister.LiveIntervals.Count; i++)
				{
					LiveInterval liveInterval = virtualRegister.LiveIntervals[i];

					if (liveInterval.ForceSpilled)
						continue;

					var callSite = FindCallSiteInInterval(liveInterval);

					if (callSite != null)
					{
						var low = GetLowOptimalSplitLocation(liveInterval, callSite, false);
						var high = GetHighOptimalSplitLocation(liveInterval, callSite, false);

						var first = liveInterval.Split(liveInterval.Start, low);
						var middle = liveInterval.Split(low, high);
						var last = liveInterval.Split(high, liveInterval.End);

						if (trace.Active) trace.Log("  Low Split   : " + first.ToString());
						if (trace.Active) trace.Log("  Middle Split: " + middle.ToString());
						if (trace.Active) trace.Log("  High Split  : " + last.ToString());

						middle.ForceSpilled = true;

						virtualRegister.LiveIntervals[i] = middle;
						virtualRegister.Add(first);
						virtualRegister.Add(last);
					}
				}
			}
		}

		private void CreateSpillSlotOperands()
		{
			foreach (var register in virtualRegisters)
			{
				if (!register.IsSpilled)
					continue;

				Debug.Assert(register.IsVirtualRegister);
				register.SpillSlotOperand = stackLayout.AddStackLocal(register.VirtualRegisterOperand.Type);
			}
		}

		private void CreatePhysicalRegisterOperands()
		{
			foreach (var register in virtualRegisters)
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

		private void InsertSpillMoves()
		{
			foreach (var register in virtualRegisters)
			{
				if (!register.IsUsed || register.IsPhysicalRegister || !register.IsSpilled)
					continue;

				foreach (var liveInterval in register.LiveIntervals)
				{
					foreach (var def in liveInterval.DefPositions)
					{
						Context context = new Context(instructionSet, def.Index);

						architecture.InsertMoveInstruction(context, register.SpillSlotOperand, liveInterval.AssignedPhysicalOperand);

						context.Marked = true;
					}
				}
			}
		}

		private void AssignRegisters()
		{
			foreach (var register in virtualRegisters)
			{
				if (!register.IsUsed || register.IsPhysicalRegister)
					continue;

				foreach (var liveInterval in register.LiveIntervals)
				{
					foreach (var use in liveInterval.UsePositions)
					{
						Context context = new Context(instructionSet, use.Index);
						AssignPhysicalRegistersToInstructions(context, register.VirtualRegisterOperand, liveInterval.AssignedPhysicalOperand == null ? liveInterval.VirtualRegister.SpillSlotOperand : liveInterval.AssignedPhysicalOperand);
					}

					foreach (var def in liveInterval.DefPositions)
					{
						Context context = new Context(instructionSet, def.Index);
						AssignPhysicalRegistersToInstructions(context, register.VirtualRegisterOperand, liveInterval.AssignedPhysicalOperand == null ? liveInterval.VirtualRegister.SpillSlotOperand : liveInterval.AssignedPhysicalOperand);
					}
				}
			}
		}

		private void AssignPhysicalRegistersToInstructions(Context context, Operand old, Operand replacement)
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

		private void ResolveDataFlow()
		{
			var resolverTrace = new CompilerTrace(trace, "ResolveDataFlow");

			MoveResolver[,] moveResolvers = new MoveResolver[2, basicBlocks.Count];

			foreach (var from in extendedBlocks)
			{
				foreach (var nextBlock in from.BasicBlock.NextBlocks)
				{
					var to = extendedBlocks[nextBlock.Sequence];

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

			for (int b = 0; b < basicBlocks.Count; b++)
			{
				for (int fromTag = 0; fromTag < 2; fromTag++)
				{
					MoveResolver moveResolver = moveResolvers[fromTag, b];

					if (moveResolver == null)
						continue;

					moveResolver.InsertResolvingMoves(architecture, instructionSet);
				}
			}
		}

		private void InsertRegisterMoves()
		{
			var insertTrace = new CompilerTrace(trace, "InsertRegisterMoves");

			// collect edge slot indexes
			Dictionary<SlotIndex, ExtendedBlock> blockEdges = new Dictionary<SlotIndex, ExtendedBlock>();

			foreach (var block in extendedBlocks)
			{
				blockEdges.Add(block.Start, block);
				blockEdges.Add(block.End, block);
			}

			foreach (var virtualRegister in virtualRegisters)
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
						if (nextInterval.Start == currentInterval.End)
						{
							// next interval is stack - stores to stack are done elsewhere
							if (nextInterval.AssignedPhysicalOperand == null)
								break;

							// check if source and destination operands of the move are the same
							if (nextInterval.AssignedOperand == currentInterval.AssignedOperand ||
								nextInterval.AssignedOperand.Register == currentInterval.AssignedOperand.Register)
								break;

							Context context = new Context(instructionSet, currentInterval.End.Index);
							context.GotoPrevious();

							while (context.IsEmpty || context.Instruction.FlowControl == FlowControl.Branch || context.Instruction.FlowControl == FlowControl.ConditionalBranch || context.Instruction.FlowControl == FlowControl.Return)
							{
								context.GotoPrevious();
							}

							architecture.InsertMoveInstruction(context,
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
}