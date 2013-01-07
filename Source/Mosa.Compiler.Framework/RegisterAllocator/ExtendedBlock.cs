/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	public sealed class ExtendedBlock
	{
		public BasicBlock BasicBlock { get; private set; }

		public int Sequence { get { return BasicBlock.Sequence; } }

		public int LoopDepth { get; private set; }

		public int From { get; set; }

		public int To { get; set; }

		public BitArray LiveGen { get; set; }

		public BitArray LiveKill { get; set; }

		public BitArray LiveOut { get; set; }

		public BitArray LiveIn { get; set; }

		public BitArray LiveKillNot { get; set; }

		public ExtendedBlock(BasicBlock basicBlock, int virtualRegisterCount, int loopDepth)
		{
			this.BasicBlock = basicBlock;
			this.LiveGen = new BitArray(virtualRegisterCount);
			this.LiveKill = new BitArray(virtualRegisterCount);
			this.LiveOut = new BitArray(virtualRegisterCount);
			this.LiveIn = new BitArray(virtualRegisterCount);
			this.LiveKillNot = new BitArray(virtualRegisterCount);
			this.LoopDepth = loopDepth;
		}
	}
}