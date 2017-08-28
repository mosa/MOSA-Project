// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections;

namespace Mosa.Compiler.Framework.Analysis.LiveVariableAnalysis
{
	public sealed class ExtendedBlock2
	{
		public BasicBlock BasicBlock { get; }

		public int Sequence { get { return BasicBlock.Sequence; } }

		public int LoopDepth { get; }

		public Range Range { get; set; }

		public int Start { get { return Range.Start; } }

		public int End { get { return Range.End; } }

		public BitArray LiveGen { get; set; }

		public BitArray LiveKill { get; set; }

		public BitArray LiveOut { get; set; }

		public BitArray LiveIn { get; set; }

		public BitArray LiveKillNot { get; set; }

		public ExtendedBlock2(BasicBlock basicBlock, int registerCount, int loopDepth)
		{
			BasicBlock = basicBlock;
			LiveGen = new BitArray(registerCount);
			LiveKill = new BitArray(registerCount);
			LiveOut = new BitArray(registerCount);
			LiveIn = new BitArray(registerCount);
			LiveKillNot = new BitArray(registerCount);
			LoopDepth = loopDepth;
		}

		public bool Contains(int index)
		{
			return Range.Contains(index) || index == Range.End;
		}

		public override string ToString()
		{
			return BasicBlock.ToString();
		}
	}
}
