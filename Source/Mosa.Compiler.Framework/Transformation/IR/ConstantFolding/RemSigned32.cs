// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class RemSigned32 : BaseTransformation
	{
		public RemSigned32() : base(IRInstruction.RemSigned32, OperandFilter.ResolvedConstant, OperandFilter.ResolvedConstant)
		{
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			transformContext.SetResultToConstant(context, ((ulong)(context.Operand1.ConstantSignedLongInteger % context.Operand2.ConstantSignedLongInteger)) & 0xFFFFFFFF);
		}
	}
}
