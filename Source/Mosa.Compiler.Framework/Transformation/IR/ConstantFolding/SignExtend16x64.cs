// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class SignExtend16x64 : BaseTransformation
	{
		public SignExtend16x64() : base(IRInstruction.SignExtend16x64, OperandFilter.ResolvedConstant)
		{
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			SetConstantResult(context, SignExtend16x64((ushort)context.Operand1.ConstantUnsignedLongInteger));
		}
	}
}
