// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.Simplification
{
	public sealed class Phi : BaseTransformation
	{
		public Phi() : base(IRInstruction.Phi, OperandFilter.Any)
		{
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			context.SetInstruction(GetMove(context.Result), context.Result, context.Operand1);
		}
	}
}
