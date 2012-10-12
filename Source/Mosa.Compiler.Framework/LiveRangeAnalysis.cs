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

		public sealed class ExtendedRegister
		{
			private List<LiveRange> liveRanges = new List<LiveRange>(1);
			private List<int> usePositions = new List<int>();

			public Operand VirtualRegister { get; private set; }
			public Register PhysicalRegister { get; private set; }
			public int Sequence { get; private set; }
			public List<LiveRange> LiveRanges { get { return liveRanges; } }
			public int Count { get { return liveRanges.Count; } }
			public List<int> UsePositions { get { return usePositions; } }

			public void AddRange(LiveRange liveRange)
			{
				LiveRange.AddRangeToList(liveRanges, liveRange.Start, liveRange.End);
			}

			public void AddRange(int start, int end)
			{
				LiveRange.AddRangeToList(liveRanges, start, end);
			}

			public void AddUsePosition(int position)
			{
				Debug.Assert(!usePositions.Contains(position));
				usePositions.Add(position);
			}

			public ExtendedRegister(Operand virtualRegister, int sequence)
			{
				this.VirtualRegister = virtualRegister;
				this.Sequence = sequence;
			}

			public ExtendedRegister(Register physicalRegister, int sequence)
			{
				this.PhysicalRegister = physicalRegister;
				this.Sequence = sequence;
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
		private int physicalRegisterCount;
		private int registerCount;
		private int[] instructionNumbering;
		private ExtendedBlock[] extendedBlocks;
		private ExtendedRegister[] extendedRegisters;

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		public LiveRangeAnalysis(BasicBlocks basicBlocks, VirtualRegisters virtualRegisters, InstructionSet instructionSet, IArchitecture architecture)
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
			this.extendedRegisters = new ExtendedRegister[registerCount];

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
				extendedRegisters[sequence] = new ExtendedRegister(physicalRegister, sequence);
			}

			// Setup extended virtual registers
			foreach (var virtualRegister in virtualRegisters)
			{
				int sequence = virtualRegister.Sequence + physicalRegisterCount;
				extendedRegisters[sequence] = new ExtendedRegister(virtualRegister, sequence);
			}

			// Number all the instructions in block order
			NumberInstructions();

			ComputeLocalLiveSets();

			ComputeGlobalLiveSets();
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
					var register = extendedRegisters[s];

					register.AddRange(new LiveRange(blockFrom, blockTo));
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
							var register = extendedRegisters[s];
							register.AddRange(new LiveRange(index, index + 1));
						}
					}

					foreach (var result in visitor.Output)
					{
						var register = extendedRegisters[GetIndex(result)];
						register.LiveRanges[0] = new LiveRange(index, (register.LiveRanges[0]).End);
						register.AddUsePosition(index);
					}

					foreach (var result in visitor.Temp)
					{
						var register = extendedRegisters[GetIndex(result)];
						register.AddRange(new LiveRange(index, index + 1));
						register.AddUsePosition(index);
					}

					foreach (var result in visitor.Input)
					{
						var register = extendedRegisters[GetIndex(result)];
						register.AddRange(new LiveRange(blockFrom, index));
						register.AddUsePosition(index);
					}

					context.GotoPrevious();
				}
			}
		}
	}

}

