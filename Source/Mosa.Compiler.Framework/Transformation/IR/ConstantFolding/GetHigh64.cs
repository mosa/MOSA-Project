// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public class GetHigh64 : BaseTransformation
	{
		public override BaseInstruction Instruction { get { return IRInstruction.GetHigh64; } }

		public override bool Match(Context context, TransformContext transformContext)
		{
			return context.Operand1.IsResolvedConstant;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			SetConstantResult(context, (uint)(context.Operand1.ConstantUnsignedLongInteger >> 32));
		}
	}
}
