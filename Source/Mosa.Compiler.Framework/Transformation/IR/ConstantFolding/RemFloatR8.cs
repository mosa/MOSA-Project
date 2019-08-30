// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class RemFloatR8 : BaseTransformation
	{
		public RemFloatR8() : base(IRInstruction.RemFloatR8, OperandFilter.ResolvedConstant, OperandFilter.ResolvedConstant)
		{
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			SetConstantResult(context, context.Operand1.ConstantDoubleFloatingPoint % context.Operand2.ConstantDoubleFloatingPoint);
		}
	}
}
