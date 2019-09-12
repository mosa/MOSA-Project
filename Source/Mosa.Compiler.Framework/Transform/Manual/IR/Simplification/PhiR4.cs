// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transform.Manual.IR.Simplification
{
	public sealed class PhiR4 : BaseTransformation
	{
		public PhiR4() : base(IRInstruction.PhiR4)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			return context.OperandCount == 1;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			context.SetInstruction(IRInstruction.MoveR4, context.Result, context.Operand1);
		}
	}
}
