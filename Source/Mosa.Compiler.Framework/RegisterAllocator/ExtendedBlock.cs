// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	public sealed class ExtendedBlock
	{
		public BasicBlock BasicBlock { get; }

		public int Sequence { get { return BasicBlock.Sequence; } }

		public int LoopDepth { get; }

		public SlotInterval Interval { get; set; }

		public SlotIndex Start { get { return Interval.Start; } }

		public SlotIndex End { get { return Interval.End; } }

		public BitArray LiveGen { get; set; }

		public BitArray LiveKill { get; set; }

		public BitArray LiveOut { get; set; }

		public BitArray LiveIn { get; set; }

		public BitArray LiveKillNot { get; set; }

		public ExtendedBlock(BasicBlock basicBlock, int registerCount, int loopDepth)
		{
			BasicBlock = basicBlock;
			LiveGen = new BitArray(registerCount);
			LiveKill = new BitArray(registerCount);
			LiveOut = new BitArray(registerCount);
			LiveIn = new BitArray(registerCount);

			//LiveKillNot = new BitArray(registerCount);
			LoopDepth = loopDepth;
		}

		public bool Contains(SlotIndex slotIndex)
		{
			return Interval.Contains(slotIndex) || slotIndex == Interval.End;
		}

		public override string ToString()
		{
			return BasicBlock.ToString();
		}
	}
}
