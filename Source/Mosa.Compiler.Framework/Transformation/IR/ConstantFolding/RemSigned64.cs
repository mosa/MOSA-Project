// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class RemSigned64 : BaseTransformation
	{
		public RemSigned64() : base(IRInstruction.RemSigned64, OperandFilter.ResolvedConstant, OperandFilter.ResolvedConstant)
		{
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			SetConstantResult(context, (ulong)(context.Operand1.ConstantSignedLongInteger % context.Operand2.ConstantSignedLongInteger));
		}
	}
}
