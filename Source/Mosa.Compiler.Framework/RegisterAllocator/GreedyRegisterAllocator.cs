/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Compiler.Common;

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

		private SortedList<int, LiveInterval> priorityQueue;
		private List<LiveIntervalUnion> liveIntervalUnions;

		public GreedyRegisterAllocator(BasicBlocks basicBlocks, VirtualRegisters compilerVirtualRegisters, InstructionSet instructionSet, IArchitecture architecture)
		{
			this.basicBlocks = basicBlocks;
			this.instructionSet = instructionSet;

			this.virtualRegisterCount = compilerVirtualRegisters.Count;
			this.physicalRegisterCount = architecture.RegisterSet.Length;
			this.registerCount = virtualRegisterCount + physicalRegisterCount;

			this.liveIntervalUnions = new List<LiveIntervalUnion>(physicalRegisterCount);
			this.virtualRegisters = new List<VirtualRegister>(registerCount);
			this.extendedBlocks = new List<ExtendedBlock>(basicBlocks.Count);

			// Setup extended physical registers
			foreach (var physicalRegister in architecture.RegisterSet)
			{
				Debug.Assert(physicalRegister.Index == virtualRegisters.Count);
				Debug.Assert(physicalRegister.Index == liveIntervalUnions.Count);

				this.virtualRegisters.Add(new VirtualRegister(physicalRegister));
				this.liveIntervalUnions.Add(new LiveIntervalUnion(physicalRegister));
			}

			// Setup extended virtual registers
			foreach (var virtualRegister in compilerVirtualRegisters)
			{
				Debug.Assert(virtualRegister.Index == virtualRegisters.Count - physicalRegisterCount + 1);

				this.virtualRegisters.Add(new VirtualRegister(virtualRegister));
			}

			priorityQueue = new SortedList<int, LiveInterval>();

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

			// Calculate Spill Costs for Live Intervals
			CalculateSpillCosts();

			// Populate Priority Queue
			PopulatePriorityQueue();

			// Process Priority Queue
			ProcessPriorityQueue();

			// MORE
		}

		private int GetIndex(Operand operand)
		{
			return (operand.IsCPURegister) ? operand.Sequence : operand.Sequence - 1 + physicalRegisterCount;
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
				extendedBlocks[block.Sequence].From = index;

				for (Context context = new Context(instructionSet, block); !context.IsLastInstruction; context.GotoNext())
				{
					if (!context.IsEmpty)
					{
						context.SlotNumber = index;
						index = index + SlotIncrement;
					}
				}

				extendedBlocks[block.Sequence].To = index - SlotIncrement;
			}
		}

		private void ComputeLocalLiveSets()
		{
			foreach (var block in extendedBlocks)
			{
				BitArray liveGen = new BitArray(registerCount);
				BitArray liveKill = new BitArray(registerCount);

				for (Context context = new Context(instructionSet, block.BasicBlock); !context.IsLastInstruction; context.GotoNext())
				{
					OperandVisitor visitor = new OperandVisitor(context);

					foreach (var ops in visitor.Input)
						if (ops.IsVirtualRegister)
						{
							int index = GetIndex(ops);
							if (!liveKill.Get(index))
								liveGen.Set(index, true);
						}

					foreach (var ops in visitor.Temp)
						if (ops.IsVirtualRegister)
						{
							int index = GetIndex(ops);
							if (!liveKill.Get(index))
								liveGen.Set(index, true);
						}

					foreach (var ops in visitor.Output)
					{
						int index = GetIndex(ops);
						if (!liveKill.Get(index))
							liveKill.Set(index, true);
					}
				}

				block.LiveGen = liveGen;
				block.LiveKill = liveKill;
				block.LiveKillNot = ((BitArray)liveKill.Clone()).Not();
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
						liveOut.And(extendedBlocks[next.Sequence].LiveGen);
					}

					BitArray liveIn = (BitArray)block.LiveIn.Clone();
					liveIn.And(block.LiveKillNot);
					liveIn.And(block.LiveGen);

					// compare them for any changes
					if (!block.LiveOut.AreSame(liveOut) || !block.LiveIn.AreSame(liveIn))
						changed = true;

					block.LiveOut = liveOut;
					block.LiveIn = liveIn;
				}
			}
		}

		private void BuildLiveIntervals()
		{
			for (int i = basicBlocks.Count - 1; i >= 0; i--)
			{
				var block = extendedBlocks[i];

				int blockFrom = block.From;
				int blockTo = block.To + 2;

				for (int s = 0; s < block.LiveOut.Count; s++)
				{
					var register = virtualRegisters[s];

					register.AddRange(blockFrom, blockTo);
				}

				Context context = new Context(instructionSet, block.BasicBlock, block.BasicBlock.EndIndex);

				while (!context.IsStartInstruction)
				{
					OperandVisitor visitor = new OperandVisitor(context);
					int index = context.SlotNumber;

					if (context.Instruction.FlowControl == FlowControl.Call)
					{
						for (int s = 0; s < physicalRegisterCount; s++)
						{
							var register = virtualRegisters[s];
							register.AddRange(index, index + 1);
						}
					}

					foreach (var result in visitor.Output)
					{
						var register = virtualRegisters[GetIndex(result)];
						register.LiveIntervals[0] = new LiveInterval(register, index, register.FirstRange.End);
						if (register.IsPhysicalRegister)
							register.AddUsePosition(index);
					}

					foreach (var result in visitor.Temp)
					{
						var register = virtualRegisters[GetIndex(result)];
						register.AddRange(index, index + 1);
						if (register.IsPhysicalRegister)
							register.AddUsePosition(index);
					}

					foreach (var result in visitor.Input)
					{
						var register = virtualRegisters[GetIndex(result)];
						register.AddRange(blockFrom, index);
						if (register.IsPhysicalRegister)
							register.AddUsePosition(index);
					}

					context.GotoPrevious();
				}
			}
		}

		private int CalculateSpillCost(LiveInterval liveInterval)
		{
			// TODO: Improve this imprecise and very trivial spill cost estimator

			int maxLoopDepth = 0;

			// find max block loop depth that interval spans
			foreach (var block in extendedBlocks)
			{
				if (liveInterval.Intersects(block.From, block.To))
				{
					maxLoopDepth = Math.Max(block.LoopDepth, maxLoopDepth);
				}
			}

			// get usage count (imprecise since it looks at the entire virtual register and not just the range of the live interval)
			int usage = liveInterval.VirtualRegister.UsePositions.Count;

			return (maxLoopDepth * 100) + usage;
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
						liveInterval.SpillCost = Int32.MaxValue;
					}
					else
					{
						// Calculate spill costs for live interval
						liveInterval.SpillCost = CalculateSpillCost(liveInterval);
					}
				}
			}
		}

		private void AddPriorityQueue(LiveInterval liveInterval)
		{
			// priority is based on interval size
			priorityQueue.Add(liveInterval.Size, liveInterval);
		}

		private void AddPriorityQueue(List<LiveInterval> liveIntervals)
		{
			foreach (var liveInterval in liveIntervals)
			{
				AddPriorityQueue(liveInterval);
			}
		}

		private LiveInterval PopPriorityQueue()
		{
			var liveInterval = priorityQueue[priorityQueue.Count - 1];
			priorityQueue.RemoveAt(priorityQueue.Count - 1);
			return liveInterval;
		}

		private void PopulatePriorityQueue()
		{
			priorityQueue.Capacity = (int)(virtualRegisters.Count * 1.2); // 1.2 is an estimate

			foreach (var virtualRegister in virtualRegisters)
			{
				foreach (var liveInterval in virtualRegister.LiveIntervals)
				{
					// Skip adding live intervals for physical registers to priority queue
					if (liveInterval.VirtualRegister.IsPhysicalRegister)
					{
						Debug.Assert(!liveIntervalUnions[liveInterval.VirtualRegister.PhysicalRegister.Index].Intersects(liveInterval));

						liveIntervalUnions[liveInterval.VirtualRegister.PhysicalRegister.Index].Add(liveInterval);

						continue;
					}

					// Add live intervals for virtual registers to priority queue
					AddPriorityQueue(liveInterval);
				}
			}
		}

		private void ProcessPriorityQueue()
		{
			while (priorityQueue.Count != 0)
			{
				var liveInterval = PopPriorityQueue();

				ProcessLiveInterval(liveInterval);
			}
		}

		private void ProcessLiveInterval(LiveInterval liveInterval)
		{
			// Find an available live interval union to place this live interval
			foreach (var liveIntervalUnion in liveIntervalUnions)
			{
				if ((liveInterval.VirtualRegister.IsFloatingPoint != liveIntervalUnion.IsFloatingPoint))
					continue;

				if (!liveIntervalUnion.Intersects(liveInterval))
				{
					liveIntervalUnion.Add(liveInterval);
					return;
				}
			}

			// No place for live interval; find live interval(s) to evict based on spill costs
			foreach (var liveIntervalUnion in liveIntervalUnions)
			{
				if ((liveInterval.VirtualRegister.IsFloatingPoint != liveIntervalUnion.IsFloatingPoint))
					continue;

				var intersections = liveIntervalUnion.GetIntersections(liveInterval);

				bool evict = true;

				foreach (var intersection in intersections)
				{
					if (intersection.SpillCost > liveInterval.SpillCost || intersection.SpillCost == Int32.MaxValue || intersection.VirtualRegister.IsPhysicalRegister)
					{
						evict = false;
						break;
					}
				}

				if (evict)
				{
					liveIntervalUnion.Evict(intersections);
					AddPriorityQueue(intersections);
					liveIntervalUnion.Add(liveInterval);

					return;
				}
			}

			// No live intervals to evict; split live interval

			// TODO
		}
	}
}