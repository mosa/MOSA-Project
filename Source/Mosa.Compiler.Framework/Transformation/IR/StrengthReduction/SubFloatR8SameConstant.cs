// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class SubFloatR8SameConstant : BaseTransformation
	{
		public SubFloatR8SameConstant() : base(IRInstruction.SubFloatR4, OperandFilter.ResolvedConstant, OperandFilter.ResolvedConstant)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			return context.Operand1.ConstantDoubleFloatingPoint == context.Operand2.ConstantDoubleFloatingPoint;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			transformContext.CreateConstant(0.0d);
		}
	}
}
