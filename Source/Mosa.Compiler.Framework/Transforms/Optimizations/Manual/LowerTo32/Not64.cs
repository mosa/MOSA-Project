// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.LowerTo32
{
	public sealed class Not64 : BaseTransformation
	{
		public Not64() : base(IRInstruction.Not64, TransformationType.Manual | TransformationType.Optimization)
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

			var op0Low = transform.AllocateVirtualRegister32();
			var op0High = transform.AllocateVirtualRegister32();
			var resultLow = transform.AllocateVirtualRegister32();
			var resultHigh = transform.AllocateVirtualRegister32();

			context.SetInstruction(IRInstruction.GetLow32, op0Low, operand1);
			context.AppendInstruction(IRInstruction.GetHigh32, op0High, operand1);

			context.AppendInstruction(IRInstruction.Not32, resultLow, op0Low);
			context.AppendInstruction(IRInstruction.Not32, resultHigh, op0High);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}
	}
}
