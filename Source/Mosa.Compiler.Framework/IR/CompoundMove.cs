// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	public sealed class CompoundMove : TwoOperandInstruction
	{
		public CompoundMove()
		{
		}

		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.CompoundMove(context);
		}
	}
}
