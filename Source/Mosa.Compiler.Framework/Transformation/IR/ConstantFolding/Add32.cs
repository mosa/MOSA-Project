// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class Add32 : BaseTransformation
	{
		public Add32() : base(IRInstruction.Add32, OperandFilter.ResolvedConstant, OperandFilter.ResolvedConstant)
		{
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			SetConstantResult(context, context.Operand1.ConstantUnsignedInteger + context.Operand2.ConstantUnsignedInteger);
		}
	}
}
