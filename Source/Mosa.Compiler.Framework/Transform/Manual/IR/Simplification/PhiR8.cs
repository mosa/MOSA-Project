// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transform.Manual.IR.Simplification
{
	public sealed class PhiR8 : BaseTransformation
	{
		public PhiR8() : base(IRInstruction.PhiR8)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			return context.OperandCount == 1;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			context.SetInstruction(IRInstruction.MoveR8, context.Result, context.Operand1);
		}
	}
}
