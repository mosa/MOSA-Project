// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public class ConvertInt64ToFloatR8 : BaseTransformation
	{
		public override BaseInstruction Instruction { get { return IRInstruction.ConvertInt64ToFloatR8; } }

		public override bool Match(Context context, TransformContext transformContext)
		{
			return context.Operand1.IsResolvedConstant;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			SetConstantResult(context, (double)context.Operand1.ConstantUnsignedLongInteger);
		}
	}
}
