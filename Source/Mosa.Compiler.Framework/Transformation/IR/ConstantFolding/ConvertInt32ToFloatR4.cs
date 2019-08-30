// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class ConvertInt32ToFloatR4 : BaseTransformation
	{
		public ConvertInt32ToFloatR4() : base(IRInstruction.ConvertInt32ToFloatR4, OperandFilter.ResolvedConstant)
		{
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			SetConstantResult(context, (float)context.Operand1.ConstantUnsignedLongInteger);
		}
	}
}
