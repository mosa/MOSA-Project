// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class LogicalAnd64 : BaseTransformation
	{
		public LogicalAnd64() : base(IRInstruction.LogicalAnd64, OperandFilter.ResolvedConstant, OperandFilter.ResolvedConstant)
		{
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			SetConstantResult(context, context.Operand1.ConstantUnsignedLongInteger & context.Operand2.ConstantUnsignedLongInteger);
		}
	}
}
