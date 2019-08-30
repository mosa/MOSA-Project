// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class Truncation64x32 : BaseTransformation
	{
		public Truncation64x32() : base(IRInstruction.Truncation64x32, OperandFilter.ResolvedConstant)
		{
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			transformContext.SetResultToConstant(context, (uint)context.Operand1.ConstantUnsignedInteger);
		}
	}
}
