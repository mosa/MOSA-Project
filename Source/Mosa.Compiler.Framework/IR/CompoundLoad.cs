// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	public sealed class CompoundLoad : ThreeOperandInstruction
	{
		public CompoundLoad()
		{
		}

		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.CompoundLoad(context);
		}
	}
}
