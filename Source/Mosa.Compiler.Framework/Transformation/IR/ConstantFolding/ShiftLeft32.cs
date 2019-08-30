// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class ShiftLeft32 : BaseTransformation
	{
		public ShiftLeft32() : base(IRInstruction.ShiftLeft32, OperandFilter.ResolvedConstant, OperandFilter.ResolvedConstant)
		{
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			SetConstantResult(context, (context.Operand1.ConstantUnsignedLongInteger << (int)context.Operand2.ConstantUnsignedLongInteger) & 0xFFFFFFFF);
		}
	}
}
