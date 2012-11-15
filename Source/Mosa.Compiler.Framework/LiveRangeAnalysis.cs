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
	public sealed class LinearRegisterAllocator
	{

		public class ExtendedInternal : Interval
		{
			public Register Register { get; set; }

			public ExtendedInternal(int start, int end, Register register)
				: base(start, end)
			{
				this.Register = register;
			}
		}

		public sealed class ExtendedRegister
		{
			private List<Interval> liveRanges = new List<Interval>(1);
			private List<int> usePositions = new List<int>();

			public Operand VirtualRegister { get; private set; }
			public Register PhysicalRegister { get; private set; }
			public int Sequence { get; private set; }
			public List<Interval> LiveRanges { get { return liveRanges; } }
			public int Count { get { return liveRanges.Count; } }
			public List<int> UsePositions { get { return usePositions; } }

			public bool IsPhysicalRegister { get { return VirtualRegister == null; } }
			public Interval LastRange { get { return liveRanges.Count == 0 ? null : liveRanges[liveRanges.Count - 1]; } }
			public Interval FirstRange { get { return liveRanges.Count == 0 ? null : liveRanges[0]; } }

			private List<ExtendedInternal> assignedRegisters = new List<ExtendedInternal>();
			private Register CurrentWalkPhysicalRegister { get; set; }

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

			public void AddRange(Interval liveRange)
			{
				Interval.AddRangeToList(liveRanges, liveRange.Start, liveRange.End);
			}

			public void AddRange(int start, int end)
			{
				Interval.AddRangeToList(liveRanges, start, end);
			}

			public void AddUsePosition(int position)
			{
				Debug.Assert(!usePositions.Contains(position));
				usePositions.Add(position);
			}

			public bool Contains(int position)
			{
				foreach (var range in liveRanges)
				{
					// TODO: early exit optimization
					if (range.IsInside(position))
					{
						return true;
					}
				}

				return false;
			}

			public void AssignRegister(Interval interval, Register register)
			{
				// TODO: Keep this list sorted
				CurrentWalkPhysicalRegister = register;
				assignedRegisters.Add(new ExtendedInternal(interval.Start, interval.End, register));
			}

			public Register GetCurrentWalkRegister()
			{
				return CurrentWalkPhysicalRegister;
			}

			public Register GetAssignedRegister(int position)
			{
				foreach (var assigned in assignedRegisters)
				{
					if (assigned.IsInside(position))
					{
						return assigned.Register;
					}
				}

				return null;
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
		public LinearRegisterAllocator(BasicBlocks basicBlocks, VirtualRegisters virtualRegisters, InstructionSet instructionSet, IArchitecture architecture)
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
					var register = extendedRegisters[s];

					register.AddRange(new Interval(blockFrom, blockTo));
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
							register.AddRange(new Interval(index, index + 1));
						}
					}

					foreach (var result in visitor.Output)
					{
						var register = extendedRegisters[GetIndex(result)];
						register.LiveRanges[0] = new Interval(index, (register.FirstRange.End));
						if (register.IsPhysicalRegister)
							register.AddUsePosition(index);
					}

					foreach (var result in visitor.Temp)
					{
						var register = extendedRegisters[GetIndex(result)];
						register.AddRange(new Interval(index, index + 1));
						if (register.IsPhysicalRegister)
							register.AddUsePosition(index);
					}

					foreach (var result in visitor.Input)
					{
						var register = extendedRegisters[GetIndex(result)];
						register.AddRange(new Interval(blockFrom, index));
						if (register.IsPhysicalRegister)
							register.AddUsePosition(index);
					}

					context.GotoPrevious();
				}
			}
		}

		private void WalkIntervals()
		{
			SortedList<int, ExtendedRegister> unhandled = new SortedList<int, ExtendedRegister>(extendedRegisters.Length);

			List<ExtendedRegister> active = new List<ExtendedRegister>();
			List<ExtendedRegister> inactive = new List<ExtendedRegister>();

			foreach (var register in extendedRegisters)
			{
				var firstRange = register.FirstRange;
				if (firstRange != null)
				{
					// sorting in reverse order
					unhandled.Add(-firstRange.Start, register);
				}
			}

			List<ExtendedRegister> removed = new List<ExtendedRegister>();

			while (unhandled.Count != 0)
			{
				var current = unhandled[unhandled.Count - 1];
				unhandled.RemoveAt(unhandled.Count - 1);

				int position = current.FirstRange.Start;

				foreach (var it in active)
				{
					if (it.LastRange.End < position)
					{
						removed.Add(it);
					}
					else if (it.Contains(position))
					{
						inactive.Add(it);
						removed.Add(it);
						break;
					}
				}

				// remove items from active
				foreach (var remove in removed)
				{
					active.Remove(remove);
				}
				removed.Clear();

				foreach (var it in inactive)
				{
					if (it.LastRange.End < position)
					{
						removed.Add(it);
					}
					else if (it.Contains(position))
					{
						active.Add(it);
						removed.Add(it);
					}
				}

				// remove items from inactive
				foreach (var remove in removed)
				{
					inactive.Remove(remove);
				}
				removed.Clear();

				TryAllocateFreeRegister(current, active);
			}

		}

		private bool TryAllocateFreeRegister(ExtendedRegister current, List<ExtendedRegister> active)
		{
			int[] freepos = new int[physicalRegisterCount];

			for (int i = 0; i < physicalRegisterCount; i++)
			{
				freepos[i] = Int32.MaxValue;
			}

			foreach (var it in active)
			{
				if (it.IsPhysicalRegister)
				{
					freepos[it.PhysicalRegister.Index] = 0;
				}
				else
				{
					freepos[it.GetCurrentWalkRegister().Index] = 0;
				}
			}

			// TODO: 

			return false;
		}

		private void AllocateBlockedRegister()
		{
		}
	}
}

