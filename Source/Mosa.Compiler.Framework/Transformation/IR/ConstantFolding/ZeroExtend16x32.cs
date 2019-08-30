// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class ZeroExtend16x32 : BaseTransformation
	{
		public ZeroExtend16x32() : base(IRInstruction.ZeroExtend16x32, OperandFilter.ResolvedConstant)
		{
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			transformContext.SetResultToConstant(context, (ushort)context.Operand1.ConstantUnsignedLongInteger);
		}
	}
}
