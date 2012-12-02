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
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.RegisterAllocator
{

	/// <summary>
	/// 
	/// </summary>
	public sealed class GreedyAllocator
	{

		private BasicBlocks basicBlocks;
		private InstructionSet instructionSet;
		private int virtualRegisterCount;
		private int physicalRegisterCount;
		private int registerCount;
		private int[] instructionNumbering;
		private ExtendedBlock[] extendedBlocks;
		private VirtualRegister[] virtualRegisters;

		private SortedList<int, LiveInterval> priorityQueue;
		private LiveIntervalUnion[] liveIntervalUnions;

		public GreedyAllocator(BasicBlocks basicBlocks, VirtualRegisters compilerVirtualRegisters, InstructionSet instructionSet, IArchitecture architecture)
		{
			this.basicBlocks = basicBlocks;
			this.instructionSet = instructionSet;

			this.virtualRegisterCount = compilerVirtualRegisters.Count;
			this.physicalRegisterCount = architecture.RegisterSet[architecture.RegisterSet.Length - 1].Index;
			this.registerCount = virtualRegisterCount + physicalRegisterCount;
			//foreach (var register in architecture.RegisterSet)
			//	if (register.Index > this.physicalRegisterCount)
			//		this.physicalRegisterCount = register.Index;

			this.liveIntervalUnions = new LiveIntervalUnion[physicalRegisterCount];
			this.instructionNumbering = new int[instructionSet.Size];
			this.virtualRegisters = new VirtualRegister[registerCount];

			// Allocate and setup extended blocks
			this.extendedBlocks = new ExtendedBlock[basicBlocks.Count];

			// Setup extended physical registers
			foreach (var physicalRegister in architecture.RegisterSet)
			{
				this.virtualRegisters[physicalRegister.Index] = new VirtualRegister(physicalRegister);
				this.liveIntervalUnions[physicalRegister.Index] = new LiveIntervalUnion(physicalRegister);
			}

			// Setup extended virtual registers
			foreach (var virtualRegister in compilerVirtualRegisters)
			{
				this.virtualRegisters[virtualRegister.Sequence + physicalRegisterCount] = new VirtualRegister(virtualRegister);
			}

			priorityQueue = new SortedList<int, LiveInterval>();

			Start();
		}

		private void Start()
		{
			// Order all the blocks
			OrderBlocks();

			// Number all the instructions in block order
			NumberInstructions();

			// Computer Local Live Sets
			ComputeLocalLiveSets();

			// Computer Global Live Sets
			ComputeGlobalLiveSets();

			// Populate Priority Queue
			PopulatePriorityQueue();
		}

		private int GetIndex(Operand operand)
		{
			return (operand.IsCPURegister) ? operand.Sequence : operand.Sequence + physicalRegisterCount;
		}

		private void OrderBlocks()
		{
			var loopAwareBlockOrder = new LoopAwareBlockOrder(this.basicBlocks);

			basicBlocks.ReorderBlocks(loopAwareBlockOrder.NewBlockOrder);

			for (int i = 0; i < basicBlocks.Count; i++)
			{
				extendedBlocks[i] = new ExtendedBlock(basicBlocks[i], registerCount, loopAwareBlockOrder.GetLoopDepth(basicBlocks[i]));
			}
		}

		private void NumberInstructions()
		{
			int index = 2;
			foreach (BasicBlock block in basicBlocks)
			{
				extendedBlocks[block.Sequence].From = index;

				for (Context context = new Context(instructionSet, block); !context.EndOfInstruction; context.GotoNext())
				{
					if (!context.IsEmpty)
					{
						instructionNumbering[context.Index] = index;
						index = index + 2;
					}
				}

				extendedBlocks[block.Sequence].To = index - 2;
			}
		}

		private void ComputeLocalLiveSets()
		{
			foreach (var block in extendedBlocks)
			{
				BitArray liveGen = new BitArray(registerCount);
				BitArray liveKill = new BitArray(registerCount);

				for (Context context = new Context(instructionSet, block.BasicBlock); !context.EndOfInstruction; context.GotoNext())
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
				changed = true;

				for (int i = basicBlocks.Count - 1; i >= 0; i++)
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
					if (!block.LiveOut.Equals(liveOut) || !block.LiveIn.Equals(liveIn))
						changed = true;

					block.LiveOut = liveOut;
					block.LiveIn = liveIn;
				}
			}
		}

		private void BuildLiveIntervals()
		{
			for (int i = basicBlocks.Count - 1; i >= 0; i++)
			{
				var block = extendedBlocks[i];

				int blockFrom = block.From;
				int blockTo = block.To + 2;

				for (int s = 0; s < block.LiveOut.Count; s++)
				{
					var register = virtualRegisters[s];

					register.AddRange(blockFrom, blockTo);
				}

				Context context = new Context(instructionSet, block.BasicBlock);
				context.GotoLast();

				while (!context.IsFirstInstruction)
				{
					OperandVisitor visitor = new OperandVisitor(context);
					int index = instructionNumbering[context.Index];

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

		private void AddPriorityQueue(LiveInterval liveInterval)
		{
			priorityQueue.Add(liveInterval.Size, liveInterval);
		}

		private LiveInterval PopPriorityQueue()
		{
			var liveInterval = priorityQueue[priorityQueue.Count - 1];
			priorityQueue.RemoveAt(priorityQueue.Count - 1);
			return liveInterval;
		}

		private void PopulatePriorityQueue()
		{
			priorityQueue.Capacity = (int)(virtualRegisters.Length * 1.2); // 1.2 is an estimate

			foreach (var virtualRegisterRanges in virtualRegisters)
			{
				foreach (var liveInterval in virtualRegisterRanges.LiveIntervals)
				{
					AddPriorityQueue(liveInterval);
				}
			}
		}

		private void ProcessPriorityQueue()
		{
			while (priorityQueue.Count != 0)
			{
				var liveInterval = PopPriorityQueue();

				// Handle physical registers
				if (liveInterval.VirtualRegister.IsPhysicalRegister)
				{
					Debug.Assert(!liveIntervalUnions[liveInterval.VirtualRegister.PhysicalRegister.Index].Intersects(liveInterval));

					liveIntervalUnions[liveInterval.VirtualRegister.PhysicalRegister.Index].AddLiveInterval(liveInterval);

					continue;
				}

				// Handle virtual registers

				// Find any available live interval union to place this live interval
				foreach (var liveIntervalUnion in liveIntervalUnions)
				{
					if ((liveInterval.VirtualRegister.IsFloatingPoint == liveIntervalUnion.IsFloatingPoint))
						continue;

					if (!liveIntervalUnion.Intersects(liveInterval))
					{
						liveIntervalUnion.AddLiveInterval(liveInterval);
						continue;
					}
				}

				// No free space, find live interval(s) to evict based on spill cost

				// TODO

			}
		}

	}
}

