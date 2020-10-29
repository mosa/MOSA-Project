// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.LowerTo32
{
	public sealed class LoadParamZeroExtend32x64 : BaseTransformation
	{
		public LoadParamZeroExtend32x64() : base(IRInstruction.LoadParamZeroExtend32x64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			return transformContext.LowerTo32;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;
			var operand1 = context.Operand1;

			var resultLow = transformContext.AllocateVirtualRegister32();

			transformContext.SplitLongOperand(operand1, out Operand op0Low, out Operand _);

			context.SetInstruction(IRInstruction.LoadParam32, resultLow, op0Low);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, transformContext.ConstantZero32);
		}
	}
}
