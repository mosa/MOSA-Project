// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class ZeroExtend32x64 : BaseTransformation
	{
		public ZeroExtend32x64() : base(IRInstruction.ZeroExtend32x64, OperandFilter.ResolvedConstant)
		{
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			transformContext.SetResultToConstant(context, (uint)context.Operand1.ConstantUnsignedLongInteger);
		}
	}
}
