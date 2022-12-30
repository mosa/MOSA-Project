// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.LowerTo32
{
	public sealed class LoadParamZeroExtend32x64 : BaseTransformation
	{
		public LoadParamZeroExtend32x64() : base(IRInstruction.LoadParamZeroExtend32x64, TransformationType.Manual | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return transform.LowerTo32;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;
			var operand1 = context.Operand1;

			var resultLow = transform.AllocateVirtualRegister32();

			transform.SplitLongOperand(operand1, out Operand op0Low, out Operand _);

			context.SetInstruction(IRInstruction.LoadParam32, resultLow, op0Low);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, transform.ConstantZero32);
		}
	}
}
