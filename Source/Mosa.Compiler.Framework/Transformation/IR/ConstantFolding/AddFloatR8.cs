// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class AddFloatR8 : BaseTransformation
	{
		public AddFloatR8() : base(IRInstruction.AddFloatR8, OperandFilter.ResolvedConstant, OperandFilter.ResolvedConstant)
		{
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			SetConstantResult(context, context.Operand1.ConstantDoubleFloatingPoint + context.Operand2.ConstantDoubleFloatingPoint);
		}
	}
}
