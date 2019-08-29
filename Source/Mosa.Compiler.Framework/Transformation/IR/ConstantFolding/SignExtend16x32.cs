// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public class SignExtend16x32 : BaseTransformation
	{
		public override BaseInstruction Instruction { get { return IRInstruction.SignExtend16x32; } }

		public override bool Match(Context context, TransformContext transformContext)
		{
			return context.Operand1.IsResolvedConstant;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			SetConstantResult(context, SignExtend16x32((ushort)context.Operand1.ConstantUnsignedLongInteger));
		}
	}
}
