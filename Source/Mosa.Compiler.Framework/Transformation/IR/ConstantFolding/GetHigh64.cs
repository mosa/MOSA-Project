// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class GetHigh64 : BaseTransformation
	{
		public GetHigh64() : base(IRInstruction.GetHigh64, OperandFilter.ResolvedConstant)
		{
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			SetConstantResult(context, (uint)(context.Operand1.ConstantUnsignedLongInteger >> 32));
		}
	}
}
