// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.LowerTo32
{
	public sealed class LoadParam64 : BaseTransformation
	{
		public LoadParam64() : base(IRInstruction.LoadParam64)
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
			var resultHigh = transformContext.AllocateVirtualRegister32();

			transformContext.SplitLongOperand(operand1, out Operand op1Low, out Operand op1High);

			context.SetInstruction(IRInstruction.LoadParam32, resultLow, op1Low);
			context.AppendInstruction(IRInstruction.LoadParam32, resultHigh, op1High);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}
	}
}
