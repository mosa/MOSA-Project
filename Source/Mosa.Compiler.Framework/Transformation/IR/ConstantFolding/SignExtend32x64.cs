// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class SignExtend32x64 : BaseTransformation
	{
		public SignExtend32x64() : base(IRInstruction.SignExtend32x64, OperandFilter.ResolvedConstant)
		{
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			transformContext.SetResultToConstant(context, SignExtend32x64((uint)context.Operand1.ConstantUnsignedLongInteger));
		}
	}
}
