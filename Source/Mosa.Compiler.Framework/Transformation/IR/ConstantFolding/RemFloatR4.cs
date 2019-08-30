// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class RemFloatR4 : BaseTransformation
	{
		public RemFloatR4() : base(IRInstruction.RemFloatR4, OperandFilter.ResolvedConstant, OperandFilter.ResolvedConstant)
		{
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			SetConstantResult(context, context.Operand1.ConstantSingleFloatingPoint % context.Operand2.ConstantSingleFloatingPoint);
		}
	}
}
