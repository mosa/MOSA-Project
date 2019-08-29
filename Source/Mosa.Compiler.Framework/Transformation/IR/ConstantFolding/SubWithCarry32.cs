// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public class SubWithCarry32 : BaseTransformation
	{
		public override BaseInstruction Instruction { get { return IRInstruction.SubWithCarry32; } }

		public override bool Match(Context context, TransformContext transformContext)
		{
			return context.Operand1.IsResolvedConstant && context.Operand2.IsResolvedConstant && context.Operand3.IsResolvedConstant;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			SetConstantResult(context, (uint)(context.Operand1.ConstantUnsignedLongInteger - context.Operand2.ConstantUnsignedLongInteger - (context.Operand3.IsConstantZero ? 0u : 1u)));
		}
	}
}
