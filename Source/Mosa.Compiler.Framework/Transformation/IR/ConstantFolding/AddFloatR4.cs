// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class AddFloatR4 : BaseTransformation
	{
		public AddFloatR4() : base(IRInstruction.AddFloatR4, OperandFilter.ResolvedConstant, OperandFilter.ResolvedConstant)
		{
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			transformContext.SetResultToConstant(context, context.Operand1.ConstantFloat + context.Operand2.ConstantFloat);
		}
	}
}
