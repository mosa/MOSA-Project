// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class LogicalNot64 : BaseTransformation
	{
		public LogicalNot64() : base(IRInstruction.LogicalNot64, OperandFilter.ResolvedConstant)
		{
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			transformContext.SetResultToConstant(context, ~context.Operand1.ConstantUnsignedLongInteger);
		}
	}
}
