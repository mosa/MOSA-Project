// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class ZeroExtend8x64 : BaseTransformation
	{
		public ZeroExtend8x64() : base(IRInstruction.ZeroExtend8x64, OperandFilter.ResolvedConstant)
		{
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			transformContext.SetResultToConstant(context, (byte)context.Operand1.ConstantUnsignedLongInteger);
		}
	}
}
