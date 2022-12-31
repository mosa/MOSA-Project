// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.LowerTo32
{
	public sealed class LoadParamZeroExtend16x64 : BaseTransform
	{
		public LoadParamZeroExtend16x64() : base(IRInstruction.LoadParamZeroExtend16x64, TransformType.Manual | TransformType.Optimization)
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

			transform.SplitLongOperand(operand1, out Operand op0Low, out Operand _);

			var resultLow = transform.AllocateVirtualRegister32();

			context.SetInstruction(IRInstruction.LoadParamZeroExtend16x32, resultLow, op0Low);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, transform.Constant32_0);
		}
	}
}
