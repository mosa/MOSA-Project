// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class LogicalNot32 : BaseTransformation
	{
		public LogicalNot32() : base(IRInstruction.LogicalNot32, OperandFilter.ResolvedConstant)
		{
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			SetConstantResult(context, ~((uint)context.Operand1.ConstantUnsignedLongInteger));
		}
	}
}
