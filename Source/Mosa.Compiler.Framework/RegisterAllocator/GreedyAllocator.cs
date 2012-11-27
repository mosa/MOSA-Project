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
		private VirtualRegisters virtualRegisters;
		private int virtualRegisterCount;
		private int physicalRegisterCount;
		private int registerCount;
		private int[] instructionNumbering;
		private ExtendedBlock[] extendedBlocks;
		private LiveInterval[] liveInterval;

		private bool[] floatingPositions;
		private bool[] integerPositions;

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		public GreedyAllocator(BasicBlocks basicBlocks, VirtualRegisters virtualRegisters, InstructionSet instructionSet, IArchitecture architecture)
		{
			this.basicBlocks = basicBlocks;
			this.instructionSet = instructionSet;
			this.virtualRegisters = virtualRegisters;

			this.virtualRegisterCount = virtualRegisters.Count;
			this.physicalRegisterCount = architecture.RegisterSet[architecture.RegisterSet.Length - 1].Index;
			this.registerCount = virtualRegisterCount + physicalRegisterCount;
			//foreach (var register in architecture.RegisterSet)
			//	if (register.Index > this.physicalRegisterCount)
			//		this.physicalRegisterCount = register.Index;

			this.instructionNumbering = new int[instructionSet.Size];
			this.liveInterval = new LiveInterval[registerCount];

			// Allocate and setup extended blocks
			this.extendedBlocks = new ExtendedBlock[basicBlocks.Count];
			for (int i = 0; i < basicBlocks.Count; i++)
			{
				this.extendedBlocks[i] = new ExtendedBlock(basicBlocks[i], registerCount);
			}

			// Setup extended physical registers
			foreach (var physicalRegister in architecture.RegisterSet)
			{
				int sequence = physicalRegister.Index;
				liveInterval[sequence] = new LiveInterval(physicalRegister, sequence);
			}

			// Setup extended virtual registers
			foreach (var virtualRegister in virtualRegisters)
			{
				int sequence = virtualRegister.Sequence + physicalRegisterCount;
				liveInterval[sequence] = new LiveInterval(virtualRegister, sequence);
			}

			// Mark which physical registers are integers and floating point
			integerPositions = new bool[architecture.RegisterSet.Length];
			floatingPositions = new bool[architecture.RegisterSet.Length];
			foreach (var physicalRegister in architecture.RegisterSet)
			{
				int sequence = physicalRegister.Index;
				integerPositions[sequence] = physicalRegister.IsInteger;
				floatingPositions[sequence] = physicalRegister.IsFloatingPoint;
			}

			// Number all the instructions in block order
			NumberInstructions();

			// Computer Local Live Sets
			ComputeLocalLiveSets();

			// Computer Global Live Sets
			ComputeGlobalLiveSets();

			// TODO: Walking Intervals
			WalkIntervals();
		}

		private int GetIndex(Operand operand)
		{
			return (operand.IsCPURegister) ? operand.Sequence : operand.Sequence + physicalRegisterCount;
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

				for (Context context = new Context(instructionSet, block.Block); !context.EndOfInstruction; context.GotoNext())
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

					foreach (var next in block.Block.NextBlocks)
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

		private void BuildIntervals()
		{
			for (int i = basicBlocks.Count - 1; i >= 0; i++)
			{
				var block = extendedBlocks[i];

				int blockFrom = block.From;
				int blockTo = block.To + 2;

				for (int s = 0; s < block.LiveOut.Count; s++)
				{
					var register = liveInterval[s];

					register.AddRange(blockFrom, blockTo);
				}

				Context context = new Context(instructionSet, block.Block);
				context.GotoLast();

				while (!context.IsFirstInstruction)
				{
					OperandVisitor visitor = new OperandVisitor(context);
					int index = instructionNumbering[context.Index];

					if (context.Instruction.FlowControl == FlowControl.Call)
					{
						for (int s = 0; s < physicalRegisterCount; s++)
						{
							var register = liveInterval[s];
							register.AddRange(index, index + 1);
						}
					}

					foreach (var result in visitor.Output)
					{
						var register = liveInterval[GetIndex(result)];
						register.LiveRanges[0] = new Interval(index, register.FirstRange.End);
						if (register.IsPhysicalRegister)
							register.AddUsePosition(index);
					}

					foreach (var result in visitor.Temp)
					{
						var register = liveInterval[GetIndex(result)];
						register.AddRange(index, index + 1);
						if (register.IsPhysicalRegister)
							register.AddUsePosition(index);
					}

					foreach (var result in visitor.Input)
					{
						var register = liveInterval[GetIndex(result)];
						register.AddRange(blockFrom, index);
						if (register.IsPhysicalRegister)
							register.AddUsePosition(index);
					}

					context.GotoPrevious();
				}
			}
		}

		private void WalkIntervals()
		{
			SortedList<int, LiveRange> unhandled = new SortedList<int, LiveRange>((int)(liveInterval.Length * 1.2)); // 1.2 is an estimate

			List<LiveRange> active = new List<LiveRange>();
			List<LiveRange> inactive = new List<LiveRange>();

			foreach (var register in liveInterval)
			{
				foreach (var interval in register.LiveRanges)
				{
					// sorting in reverse order
					unhandled.Add(-interval.Start, new LiveRange(register, interval));
				}
			}
		}

	}
}

