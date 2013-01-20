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

		private BaseMethodCompilerStage stage;

		public GreedyRegisterAllocator(BasicBlocks basicBlocks, VirtualRegisters compilerVirtualRegisters, InstructionSet instructionSet, IArchitecture architecture, BaseMethodCompilerStage stage)
		{
			this.stage = stage; // for access to internal tracing

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

				bool reserved = (physicalRegister == architecture.StackFrameRegister || physicalRegister == architecture.StackPointerRegister);

				this.virtualRegisters.Add(new VirtualRegister(physicalRegister));
				this.liveIntervalUnions.Add(new LiveIntervalUnion(physicalRegister, reserved));
			}

			// Setup extended virtual registers
			foreach (var virtualRegister in compilerVirtualRegisters)
			{
				Debug.Assert(virtualRegister.Index == virtualRegisters.Count - physicalRegisterCount + 1);

				this.virtualRegisters.Add(new VirtualRegister(virtualRegister));
			}

			priorityQueue = new SimpleKeyPriorityQueue<LiveInterval>();

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
			//ProcessPriorityQueue();

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
			if (!stage.IsLogging)
				return;

			string section = "Extended Blocks";

			foreach (var block in extendedBlocks)
			{
				stage.Trace(section, "Block # " + block.BasicBlock.Sequence.ToString());
				stage.Trace(section, " LiveGen:");
				stage.Trace(section, " " + ToString(block.LiveGen));
				stage.Trace(section, " LiveIn:");
				stage.Trace(section, " " + ToString(block.LiveIn));
				stage.Trace(section, " LiveOut:");
				stage.Trace(section, " " + ToString(block.LiveOut));
				stage.Trace(section, " LiveKill:");
				stage.Trace(section, " " + ToString(block.LiveKill));
			}

			section = "Registers";

			foreach (var virtualRegister in virtualRegisters)
			{
				if (virtualRegister.IsPhysicalRegister)
				{
					stage.Trace(section, "Physical Register # " + virtualRegister.PhysicalRegister.ToString());
				}
				else
				{
					stage.Trace(section, "Virtual Register # " + virtualRegister.VirtualRegisterOperand.Index.ToString());
				}

				stage.Trace(section, "Live Intervals (" + virtualRegister.LiveIntervals.Count.ToString() + ")");

				foreach (var liveInterval in virtualRegister.LiveIntervals)
				{
					stage.Trace(section, " [" + liveInterval.Start.ToString() + ", " + liveInterval.End.ToString() + "]");
				}

				stage.Trace(section, "Use Positions (" + virtualRegister.UsePositions.Count.ToString() + ")");

				foreach (var use in virtualRegister.UsePositions)
				{
					stage.Trace(section, " " + use.ToString());
				}
			}
		}

		private int GetIndex(Operand operand)
		{
			return (operand.IsCPURegister) ? (operand.Sequence) : (operand.Sequence + physicalRegisterCount - 1);
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
			foreach (var block in extendedBlocks)
			{
				BitArray liveGen = new BitArray(registerCount);
				BitArray liveKill = new BitArray(registerCount);

				for (Context context = new Context(instructionSet, block.BasicBlock); !context.IsLastInstruction; context.GotoNext())
				{
					OperandVisitor visitor = new OperandVisitor(context);

					foreach (var ops in visitor.Input)
					{
						if (ops.IsVirtualRegister)
						{
							int index = GetIndex(ops);
							if (!liveKill.Get(index))
								liveGen.Set(index, true);
						}
					}
					foreach (var ops in visitor.Temp)
					{
						if (ops.IsVirtualRegister)
						{
							int index = GetIndex(ops);
							liveKill.Set(index, true);
						}
					}
					foreach (var ops in visitor.Output)
					{
						if (ops.IsVirtualRegister)
						{
							int index = GetIndex(ops);
							liveKill.Set(index, true);
						}
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
						liveOut.Or(extendedBlocks[next.Sequence].LiveIn);
					}

					BitArray liveIn = (BitArray)block.LiveOut.Clone();
					liveIn.And(block.LiveKillNot);
					liveIn.Or(block.LiveGen);

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
			int blockMax = basicBlocks.Count - 1;

			for (int i = blockMax; i >= 0; i--)
			{
				var block = extendedBlocks[i];

				for (int s = 0; s < registerCount; s++)
				{
					if (!block.LiveIn.Get(s))
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
							register.LiveIntervals[0] = new LiveInterval(register, currentSlotIndex, register.FirstRange.End);
							if (!register.IsPhysicalRegister)
								register.AddUsePosition(currentSlotIndex);
						}

						foreach (var result in visitor.Temp)
						{
							var register = virtualRegisters[GetIndex(result)];
							register.AddLiveInterval(currentSlotIndex, prevSlotIndex);
							if (!register.IsPhysicalRegister)
								register.AddUsePosition(currentSlotIndex);
						}

						foreach (var result in visitor.Input)
						{
							var register = virtualRegisters[GetIndex(result)];
							register.AddLiveInterval(block.From, currentSlotIndex);
							if (!register.IsPhysicalRegister)
								register.AddUsePosition(currentSlotIndex);
						}
					}

					prevSlotIndex = currentSlotIndex;
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

		private int CalculateSpillCost(LiveInterval liveInterval)
		{
			int spillcosts = 0;

			foreach (var use in liveInterval.VirtualRegister.UsePositions)
			{
				spillcosts += 100 * (1 + GetLoopDepth(use));
			}

			return (spillcosts / liveInterval.Length);
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
			priorityQueue.Enqueue(liveInterval.Length, liveInterval);
		}

		private void AddPriorityQueue(List<LiveInterval> liveIntervals)
		{
			foreach (var liveInterval in liveIntervals)
			{
				AddPriorityQueue(liveInterval);
			}
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
			// Find an available live interval union to place this live interval
			foreach (var liveIntervalUnion in liveIntervalUnions)
			{
				if (liveIntervalUnion.IsReserved)
					continue;

				if (liveIntervalUnion.IsFloatingPoint != liveInterval.VirtualRegister.IsFloatingPoint)
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
				if (liveIntervalUnion.IsReserved)
					continue;

				if (liveIntervalUnion.IsFloatingPoint != liveInterval.VirtualRegister.IsFloatingPoint)
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