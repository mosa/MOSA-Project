/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Compiler.Common;
using Mosa.Compiler.InternalTrace;

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
		private int virtualRegisterCount;
		private int physicalRegisterCount;
		private int registerCount;
		private List<ExtendedBlock> extendedBlocks;
		private List<VirtualRegister> virtualRegisters;

		private SimpleKeyPriorityQueue<LiveInterval> priorityQueue;
		private List<LiveIntervalUnion> liveIntervalUnions;

		private Register stackFrameRegister;
		private Register stackPointerRegister;

		private List<LiveInterval> spilledIntervals;

		private CompilerTrace trace;

		public GreedyRegisterAllocator(BasicBlocks basicBlocks, VirtualRegisters compilerVirtualRegisters, InstructionSet instructionSet, IArchitecture architecture, CompilerTrace trace)
		{
			this.trace = trace;

			this.basicBlocks = basicBlocks;
			this.instructionSet = instructionSet;

			this.virtualRegisterCount = compilerVirtualRegisters.Count;
			this.physicalRegisterCount = architecture.RegisterSet.Length;
			this.registerCount = virtualRegisterCount + physicalRegisterCount;

			this.liveIntervalUnions = new List<LiveIntervalUnion>(physicalRegisterCount);
			this.virtualRegisters = new List<VirtualRegister>(registerCount);
			this.extendedBlocks = new List<ExtendedBlock>(basicBlocks.Count);

			stackFrameRegister = architecture.StackFrameRegister;
			stackPointerRegister = architecture.StackPointerRegister;

			// Setup extended physical registers
			foreach (var physicalRegister in architecture.RegisterSet)
			{
				Debug.Assert(physicalRegister.Index == virtualRegisters.Count);
				Debug.Assert(physicalRegister.Index == liveIntervalUnions.Count);

				bool reserved = (physicalRegister == stackFrameRegister || physicalRegister == stackPointerRegister);

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

			Start();
		}

		private void Start()
		{
			// Order all the blocks
			CreateExtendedBlocks();

			// Number all the instructions in block order
			NumberInstructions();

			// Computer Local Live Sets
			ComputeLocalLiveSets();

			// Computer Global Live Sets
			ComputeGlobalLiveSets();

			BuildLiveIntervals();

			TraceLiveIntervals();

			// Calculate Spill Costs for Live Intervals
			CalculateSpillCosts();

			// Populate Priority Queue
			PopulatePriorityQueue();

			//// Process Priority Queue
			ProcessPriorityQueue();

			// MORE
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

		private void TraceLiveIntervals()
		{
			if (!trace.Active)
				return;

			var sectionTrace = new CompilerTrace(trace, "Extended Blocks");
			var registerTrace = new CompilerTrace(trace, "Registers");

			foreach (var block in extendedBlocks)
			{
				sectionTrace.Log("Block # " + block.BasicBlock.Sequence.ToString());
				sectionTrace.Log(" LiveIn:   " + ToString(block.LiveIn));
				sectionTrace.Log(" LiveGen:  " + ToString(block.LiveGen));
				sectionTrace.Log(" LiveKill: " + ToString(block.LiveKill));
				sectionTrace.Log(" LiveOut:  " + ToString(block.LiveOut));
			}


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

				registerTrace.Log("Live Intervals (" + virtualRegister.LiveIntervals.Count.ToString() + ")");

				foreach (var liveInterval in virtualRegister.LiveIntervals)
				{
					registerTrace.Log(" [" + liveInterval.Start.ToString() + ", " + liveInterval.End.ToString() + "]");
				}

				registerTrace.Log("Use Positions (" + virtualRegister.UsePositions.Count.ToString() + ")");

				foreach (var use in virtualRegister.UsePositions)
				{
					registerTrace.Log(" " + use.ToString());
				}
			}
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
			basicBlocks.ReorderBlocks(loopAwareBlockOrder.NewBlockOrder);

			// Allocate and setup extended blocks
			for (int i = 0; i < basicBlocks.Count; i++)
			{
				extendedBlocks.Add(new ExtendedBlock(basicBlocks[i], registerCount, loopAwareBlockOrder.GetLoopDepth(basicBlocks[i])));
			}
		}

		private void NumberInstructions()
		{
			int index = SlotIncrement;

			foreach (BasicBlock block in basicBlocks)
			{
				for (Context context = new Context(instructionSet, block); ; context.GotoNext())
				{
					if (!context.IsEmpty)
					{
						context.SlotNumber = index;
						index = index + SlotIncrement;
					}

					if (context.IsLastInstruction)
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

				for (Context context = new Context(instructionSet, block.BasicBlock); !context.IsLastInstruction; context.GotoNext())
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
			int blockMax = basicBlocks.Count - 1;

			for (int i = blockMax; i >= 0; i--)
			{
				var block = extendedBlocks[i];

				for (int s = 0; s < registerCount; s++)
				{
					if (!block.LiveOut.Get(s))
						continue;

					var register = virtualRegisters[s];

					if (i != blockMax)
					{
						register.AddLiveInterval(block.From, extendedBlocks[i + 1].From);
					}
					else
					{
						register.AddLiveInterval(block.Interval);
					}
				}

				Context context = new Context(instructionSet, block.BasicBlock, block.BasicBlock.EndIndex);
				SlotIndex prevSlotIndex = new SlotIndex(context);

				while (!context.IsStartInstruction)
				{
					SlotIndex currentSlotIndex = new SlotIndex(context);

					if (!context.IsEmpty)
					{
						OperandVisitor visitor = new OperandVisitor(context);

						if (context.Instruction.FlowControl == FlowControl.Call)
						{
							for (int s = 0; s < physicalRegisterCount; s++)
							{
								var register = virtualRegisters[s];
								register.AddLiveInterval(currentSlotIndex, prevSlotIndex);
							}
						}

						foreach (var result in visitor.Output)
						{
							var register = virtualRegisters[GetIndex(result)];

							if (register.IsReserved)
								continue;

							var first = register.FirstRange;

							if (first != null)
							{
								register.LiveIntervals[0] = new LiveInterval(register, currentSlotIndex, first.End);
							}
							else
							{
								// This is necesary to handled a result that is never used!
								// Common with instructions which more than one result
								register.AddLiveInterval(currentSlotIndex, prevSlotIndex);
							}

							if (!register.IsPhysicalRegister)
								register.AddUsePosition(currentSlotIndex);
						}

						foreach (var result in visitor.Temp)
						{
							var register = virtualRegisters[GetIndex(result)];

							if (register.IsReserved)
								continue;

							register.AddLiveInterval(currentSlotIndex, prevSlotIndex);
							if (!register.IsPhysicalRegister)
								register.AddUsePosition(currentSlotIndex);
						}

						foreach (var result in visitor.Input)
						{
							var register = virtualRegisters[GetIndex(result)];

							if (register.IsReserved)
								continue;

							register.AddLiveInterval(block.From, prevSlotIndex);
							if (!register.IsPhysicalRegister)
								register.AddUsePosition(currentSlotIndex);
						}

						prevSlotIndex = currentSlotIndex;
					}

					context.GotoPrevious();
				}
			}
		}

		private int GetLoopDepth(SlotIndex slotIndex)
		{
			foreach (var block in extendedBlocks)
			{
				if (block.Contains(slotIndex))
				{
					return block.LoopDepth;
				}
			}

			Debug.Assert(false, "GetLoopDepth");

			return 0;
		}

		private void CalculateSpillCost(LiveInterval liveInterval)
		{
			int spillvalue = 0;

			foreach (var use in liveInterval.VirtualRegister.UsePositions)
			{
				spillvalue += (100 * SlotIncrement) * (1 + GetLoopDepth(use));
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
						liveInterval.SpillValue = Int32.MaxValue;
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
						//Debug.Assert(!liveIntervalUnions[liveInterval.VirtualRegister.PhysicalRegister.Index].Intersects(liveInterval));

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
			if (trace.Active) trace.Log("Processing Interval: " + liveInterval.ToString() + " / Length: " + liveInterval.Length.ToString() + " / Spill cost: " + liveInterval.SpillCost.ToString() + " / Stage: " + liveInterval.Stage.ToString());

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
				if (trace.Active) trace.Log("  Re-queued for prespilled stage");
				liveInterval.Stage = LiveInterval.AllocationStage.PreSpill;
				AddPriorityQueue(liveInterval);
				return;
			}

			// split live interval
			if (liveInterval.Stage == LiveInterval.AllocationStage.PreSpill)
			{
				if (trace.Active) trace.Log("  Attempting to split interval");

				if (TrySplitInterval(liveInterval))
				{
					// TODO: ?

					return;
				}
				else
				{
					if (trace.Active) trace.Log("  Re-queued for spillable stage");

					liveInterval.Stage = LiveInterval.AllocationStage.Spillable;
					AddPriorityQueue(liveInterval);
					return;
				}
			}

			// spill interval to stack slot
			if (liveInterval.Stage == LiveInterval.AllocationStage.Spillable)
			{
				if (trace.Active) trace.Log("  Spilled interval");

				spilledIntervals.Add(liveInterval);
				return;
			}

			// TODO
			return;
		}

		private bool TrySplitInterval(LiveInterval liveInterval)
		{
			if (TrySimplePartialFreeIntervalSplit(liveInterval))
				return true;

			if (TrySimpleFurthestIntervalSplit(liveInterval))
				return true;

			return false;
		}

		private bool TrySimplePartialFreeIntervalSplit(LiveInterval liveInterval)
		{
			SlotIndex maxFree = null;

			foreach (var liveIntervalUnion in liveIntervalUnions)
			{
				if (liveIntervalUnion.IsReserved)
					continue;

				if (liveIntervalUnion.IsFloatingPoint != liveInterval.VirtualRegister.IsFloatingPoint)
					continue;

				var lastFree = liveIntervalUnion.GetMaximunFreeSlotAfter(liveInterval.Start);

				if (lastFree == null)
					continue;

				Debug.Assert(lastFree > liveInterval.Start);

				if (trace.Active) trace.Log("  Free at " + liveIntervalUnion.ToString() + " up to " + lastFree.ToString());

				if (maxFree == null || maxFree < lastFree)
					maxFree = lastFree;
			}

			if (maxFree == null)
			{
				if (trace.Active) trace.Log("  No partial free space available");

				return false;
			}

			if (trace.Active) trace.Log("  Partial free up to: " + maxFree.ToString());

			SplitInterval(liveInterval, maxFree);

			return true;
		}

		private bool TrySimpleFurthestIntervalSplit(LiveInterval liveInterval)
		{
			SlotIndex furthestSlot = null;
			LiveInterval candidateLiveInterval = null;

			foreach (var liveIntervalUnion in liveIntervalUnions)
			{
				if (liveIntervalUnion.IsReserved)
					continue;

				if (liveIntervalUnion.IsFloatingPoint != liveInterval.VirtualRegister.IsFloatingPoint)
					continue;

				var intersect = liveIntervalUnion.GetLiveIntervalAt(liveInterval.Start);

				if (intersect == null)
					continue;

				if (intersect.IsPhysicalRegister)
					continue;

				var minUseSlot = GetNextUsePosition(intersect.VirtualRegister, liveInterval.Start);

				if (trace.Active) trace.Log("  Allocated " + intersect.ToString() + " next use " + (minUseSlot != null ? minUseSlot.ToString() : "none"));

				if (candidateLiveInterval == null)
				{
					candidateLiveInterval = intersect;
					furthestSlot = minUseSlot;

					if (furthestSlot == null)
						break;

					continue;
				}

				if (minUseSlot == null || minUseSlot > furthestSlot)
				{
					candidateLiveInterval = intersect;
					furthestSlot = minUseSlot;
				}

			}

			if (candidateLiveInterval == null)
			{
				if (trace.Active) trace.Log("  No allocated interval available to split");

				return false;
			}

			if (trace.Active) trace.Log("  Found allocated interval to split: " + candidateLiveInterval.ToString());
			if (trace.Active) trace.Log("  Next use is: " + (furthestSlot != null ? furthestSlot.ToString() : "none"));

			// evict furthestIntersect
			candidateLiveInterval.Evict();
			if (trace.Active) trace.Log("  Evicted: " + candidateLiveInterval.ToString());

			// split furthestIntersect 
			var splitLocation = furthestSlot == null ? liveInterval.End : furthestSlot;
			SplitInterval(candidateLiveInterval, splitLocation);

			// split liveInterval at furthestSlot
			if (furthestSlot != null)
			{
				SplitInterval(liveInterval, furthestSlot);
			}

			return true;
		}

		private SlotIndex GetNextUsePosition(VirtualRegister virtualRegister, SlotIndex start)
		{
			SlotIndex minSlot = null;

			foreach (var useSlot in virtualRegister.UsePositions)
			{
				if (useSlot > start)
				{
					if (minSlot == null || useSlot < minSlot)
					{
						minSlot = useSlot;
						continue;
					}

				}
			}

			return minSlot;
		}

		private void SplitInterval(LiveInterval liveInterval, SlotIndex at)
		{
			// TODO: Find start/end ranges based on uses
			var virtualRegister = liveInterval.VirtualRegister;

			var first = new LiveInterval(virtualRegister, liveInterval.Start, at);
			var second = new LiveInterval(virtualRegister, at, liveInterval.End);

			virtualRegister.Remove(liveInterval);
			virtualRegister.Add(first);
			virtualRegister.Add(second);

			CalculateSpillCost(first);
			CalculateSpillCost(second);

			AddPriorityQueue(first);
			AddPriorityQueue(second);

			if (trace.Active) trace.Log("  Split interval 1/2: " + first.ToString());
			if (trace.Active) trace.Log("  Split interval 2/2: " + second.ToString());
		}
	}
}