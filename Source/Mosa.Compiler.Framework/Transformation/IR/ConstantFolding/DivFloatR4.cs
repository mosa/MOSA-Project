// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class DivFloatR4 : BaseTransformation
	{
		public DivFloatR4() : base(IRInstruction.DivFloatR4, OperandFilter.ResolvedConstant, OperandFilter.ResolvedConstant)
		{
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			transformContext.SetResultToConstant(context, context.Operand1.ConstantSingleFloatingPoint / context.Operand2.ConstantSingleFloatingPoint);
		}
	}
}
