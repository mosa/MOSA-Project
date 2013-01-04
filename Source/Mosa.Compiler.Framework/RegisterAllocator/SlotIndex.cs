/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	public class SlotIndex
	{
		private int index;
		private InstructionSet instructionSet;

		public SlotIndex(InstructionSet instructionSet, int index)
		{
			this.index = index;
			this.instructionSet = instructionSet;
		}

		public SlotIndex(Context context)
			: this(context.InstructionSet, context.Index)
		{
		}

		public int Index { get { return instructionSet.Data[index].SlotNumber; } }

	}
}
