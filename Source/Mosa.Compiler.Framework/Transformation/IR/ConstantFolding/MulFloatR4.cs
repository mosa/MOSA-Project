// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class MulFloatR4 : BaseTransformation
	{
		public MulFloatR4() : base(IRInstruction.MulFloatR4, OperandFilter.ResolvedConstant, OperandFilter.ResolvedConstant)
		{
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			transformContext.SetResultToConstant(context, context.Operand1.ConstantSingleFloatingPoint * context.Operand2.ConstantSingleFloatingPoint);
		}
	}
}
