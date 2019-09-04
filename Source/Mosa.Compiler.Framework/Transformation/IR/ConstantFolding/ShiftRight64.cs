// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class ShiftRight64 : BaseTransformation
	{
		public ShiftRight64() : base(IRInstruction.ShiftRight64, OperandFilter.ResolvedConstant, OperandFilter.ResolvedConstant)
		{
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			transformContext.SetResultToConstant(context, context.Operand1.ConstantUnsigned64 >> (int)context.Operand2.ConstantUnsigned64);
		}
	}
}
