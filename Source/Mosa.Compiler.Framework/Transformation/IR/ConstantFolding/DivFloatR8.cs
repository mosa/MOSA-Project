// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public class DivFloatR8 : BaseTransformation
	{
		public override BaseInstruction Instruction { get { return IRInstruction.Add64; } }

		public override bool Match(Context context, TransformContext transformContext)
		{
			return context.Operand1.IsResolvedConstant && context.Operand2.IsResolvedConstant;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			SetConstantResult(context, context.Operand1.ConstantDoubleFloatingPoint / context.Operand2.ConstantDoubleFloatingPoint);
		}
	}
}
