// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.LowerTo32
{
	public sealed class LoadParam64 : BaseTransformation
	{
		public LoadParam64() : base(IRInstruction.LoadParam64, TransformationType.Manual | TransformationType.Optimization)
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
			var resultHigh = transform.AllocateVirtualRegister32();

			transform.SplitLongOperand(operand1, out Operand op1Low, out Operand op1High);

			context.SetInstruction(IRInstruction.LoadParam32, resultLow, op1Low);
			context.AppendInstruction(IRInstruction.LoadParam32, resultHigh, op1High);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}
	}
}
