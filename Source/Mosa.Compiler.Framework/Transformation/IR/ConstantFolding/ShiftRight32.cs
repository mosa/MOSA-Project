// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class ShiftRight32 : BaseTransformation
	{
		public ShiftRight32() : base(IRInstruction.ShiftRight32, OperandFilter.ResolvedConstant, OperandFilter.ResolvedConstant)
		{
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			SetConstantResult(context, (context.Operand1.ConstantUnsignedLongInteger >> (int)context.Operand2.ConstantUnsignedLongInteger) & 0xFFFFFFFF);
		}
	}
}
