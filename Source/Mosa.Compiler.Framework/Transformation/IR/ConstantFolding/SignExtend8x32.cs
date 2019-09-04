// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class SignExtend8x32 : BaseTransformation
	{
		public SignExtend8x32() : base(IRInstruction.SignExtend8x32, OperandFilter.ResolvedConstant)
		{
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			transformContext.SetResultToConstant(context, SignExtend8x32((byte)context.Operand1.ConstantUnsigned64));
		}
	}
}
