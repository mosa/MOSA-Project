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

		public Interval Interval { get; set; }		
		public SlotIndex From { get { return Interval.Start; } }
		public SlotIndex To { get { return Interval.End; } }

		public BitArray LiveGen { get; set; }

		public BitArray LiveKill { get; set; }

		public BitArray LiveOut { get; set; }

		public BitArray LiveIn { get; set; }

		public BitArray LiveKillNot { get; set; }

		public ExtendedBlock(BasicBlock basicBlock, int registerCount, int loopDepth)
		{
			this.BasicBlock = basicBlock;
			this.LiveGen = new BitArray(registerCount);
			this.LiveKill = new BitArray(registerCount);
			this.LiveOut = new BitArray(registerCount);
			this.LiveIn = new BitArray(registerCount);
			this.LiveKillNot = new BitArray(registerCount);
			this.LoopDepth = loopDepth;
		}

		public bool Contains(SlotIndex slotIndex)
		{
			return Interval.Contains(slotIndex);
		}

		public bool Contains(Context context)
		{
			// TODO: could be made faster by avoiding allocation of SlotIndex
			return Contains(new SlotIndex(context));
		}

		public override string ToString()
		{
			return BasicBlock.ToString();
		}
	}
}