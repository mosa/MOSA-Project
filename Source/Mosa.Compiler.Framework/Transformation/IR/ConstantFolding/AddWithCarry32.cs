// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class AddWithCarry32 : BaseTransformation
	{
		public AddWithCarry32() : base(IRInstruction.AddWithCarry32, OperandFilter.ResolvedConstant, OperandFilter.ResolvedConstant, OperandFilter.ResolvedConstant)
		{
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			transformContext.SetResultToConstant(context, (uint)(context.Operand1.ConstantUnsigned64 + context.Operand2.ConstantUnsigned64 + (context.Operand3.IsConstantZero ? 0u : 1u)));
		}
	}
}
