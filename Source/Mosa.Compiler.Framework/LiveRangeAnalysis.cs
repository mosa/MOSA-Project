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

namespace Mosa.Compiler.Framework
{

	/// <summary>
	/// 
	/// </summary>
	public sealed class LiveRangeAnalysis
	{

		public struct LiveRange
		{
			int Start { get; set; }
			int End { get; set; }

			public LiveRange(int start, int end)
				: this()
			{
				Start = start;
				End = end;
			}
		}

		public sealed class ExtendedVirtualRegister
		{
			public List<LiveRange> liveRanges = new List<LiveRange>(1);

			public Operand VirtualRegister { get; private set; }
			public int Sequence { get { return VirtualRegister.Sequence; } }
			public List<LiveRange> LiveRanges { get { return liveRanges; } }
			public int Count { get { return liveRanges.Count; } }

			public void AddRange(LiveRange liveRange)
			{
				// TODO: Merge if ranges connect
				liveRanges.Add(liveRange);
			}

			public ExtendedVirtualRegister(Operand virtualRegister)
			{
				VirtualRegister = virtualRegister;
			}
		}

		public sealed class ExtendedBlock
		{
			public BasicBlock Block { get; private set; }
			public int Sequence { get { return Block.Sequence; } }

			public int From { get; set; }
			public int To { get; set; }

			public BitArray LiveGen { get; set; }
			public BitArray LiveKill { get; set; }
			public BitArray LiveOut { get; set; }
			public BitArray LiveIn { get; set; }
			public BitArray LiveKillNot { get; set; }

			public ExtendedBlock(BasicBlock basicBlock, int virtualRegisterCount)
			{
				this.Block = basicBlock;
				this.LiveGen = new BitArray(virtualRegisterCount);
				this.LiveKill = new BitArray(virtualRegisterCount);
				this.LiveOut = new BitArray(virtualRegisterCount);
				this.LiveIn = new BitArray(virtualRegisterCount);
				this.LiveKillNot = new BitArray(virtualRegisterCount);
			}
		}

		private BasicBlocks basicBlocks;
		private InstructionSet instructionSet;
		private VirtualRegisters virtualRegisters;
		private int virtualRegisterCount;

		private int[] instructionNumbering;
		private ExtendedBlock[] extendedBlocks;
		private ExtendedVirtualRegister[] extendedVirtualRegisters;

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		public LiveRangeAnalysis(BasicBlocks basicBlocks, VirtualRegisters virtualRegisters, InstructionSet instructionSet)
		{
			this.basicBlocks = basicBlocks;
			this.instructionSet = instructionSet;
			this.virtualRegisters = virtualRegisters;

			this.virtualRegisterCount = virtualRegisters.Count;
			this.instructionNumbering = new int[instructionSet.Size];
			this.extendedVirtualRegisters = new ExtendedVirtualRegister[virtualRegisterCount];

			// Allocate and setup extended blocks
			this.extendedBlocks = new ExtendedBlock[basicBlocks.Count];
			for (int i = 0; i < basicBlocks.Count; i++)
			{
				this.extendedBlocks[i] = new ExtendedBlock(basicBlocks[i], virtualRegisterCount);
			}

			// Allocate and setup extended virtual registers
			foreach (var virtualRegister in virtualRegisters)
			{
				extendedVirtualRegisters[virtualRegister.Sequence] = new ExtendedVirtualRegister(virtualRegister);
			}

			// Number all the instructions in block order
			NumberInstructions();

			ComputeLocalLiveSets();

			ComputeGlobalLiveSets();
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
				BitArray liveGen = new BitArray(virtualRegisterCount);
				BitArray liveKill = new BitArray(virtualRegisterCount);

				for (Context context = new Context(instructionSet, block.Block); !context.EndOfInstruction; context.GotoNext())
				{
					OperandVisitor visitor = new OperandVisitor(context);

					foreach (var ops in visitor.Input)
						if (ops.IsVirtualRegister)
							if (!liveKill.Get(ops.Sequence))
								liveGen.Set(ops.Sequence, true);

					foreach (var ops in visitor.Temp)
						if (ops.IsVirtualRegister)
							if (!liveKill.Get(ops.Sequence))
								liveGen.Set(ops.Sequence, true);

					foreach (var ops in visitor.Output)
						if (!liveKill.Get(ops.Sequence))
							liveKill.Set(ops.Sequence, true);
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

					BitArray liveOut = new BitArray(virtualRegisterCount);

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
					var operand = extendedVirtualRegisters[s];

					operand.AddRange(new LiveRange(blockFrom, blockTo));
				}

				Context context = new Context(instructionSet, block.Block);
				context.GotoLast();

				while (!context.IsFirstInstruction)
				{
					OperandVisitor visitor = new OperandVisitor(context);

					if (context.Instruction.FlowControl == FlowControl.Call)
					{
						//foreach physical register reg do
						//intervals[reg].add_range(op.id, op.id + 1)
					}


					foreach (var result in visitor.Output)
					{
						//intervals[opr].first_range.from = op.id
						//intervals[opr].add_use_pos(op.id, use_kind_for(op, opr))
					}


					context.GotoPrevious();
				}
			}
		}
	}

}

