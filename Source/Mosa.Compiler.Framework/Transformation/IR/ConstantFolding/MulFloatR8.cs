// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class MulFloatR8 : BaseTransformation
	{
		public MulFloatR8() : base(IRInstruction.MulFloatR8, OperandFilter.ResolvedConstant, OperandFilter.ResolvedConstant)
		{
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			transformContext.SetResultToConstant(context, context.Operand1.ConstantDoubleFloatingPoint * context.Operand2.ConstantDoubleFloatingPoint);
		}
	}
}
