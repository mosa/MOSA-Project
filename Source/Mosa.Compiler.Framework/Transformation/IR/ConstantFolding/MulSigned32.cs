// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class MulSigned32 : BaseTransformation
	{
		public MulSigned32() : base(IRInstruction.MulSigned32, OperandFilter.ResolvedConstant, OperandFilter.ResolvedConstant)
		{
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			transformContext.SetResultToConstant(context, (context.Operand1.ConstantUnsigned64 * context.Operand2.ConstantUnsigned64) & 0xFFFFFFFF);
		}
	}
}
