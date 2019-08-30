// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class SubFloatR4SameRegister : BaseTransformation
	{
		public SubFloatR4SameRegister() : base(IRInstruction.SubFloatR4, OperandFilter.VirtualRegister, OperandFilter.VirtualRegister)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			return context.Operand1 == context.Operand2;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			transformContext.CreateConstant(0.0f);
		}
	}
}
