// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.LowerTo32
{
	public sealed class LoadParamSignExtend16x64 : BaseTransformation
	{
		public LoadParamSignExtend16x64() : base(IRInstruction.LoadParamSignExtend16x64)
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

			transformContext.SplitLongOperand(operand1, out Operand op0Low, out _);

			var resultLow = transformContext.AllocateVirtualRegister32();
			var resultHigh = transformContext.AllocateVirtualRegister32();

			context.SetInstruction(IRInstruction.LoadParamSignExtend16x32, resultLow, op0Low);
			context.AppendInstruction(IRInstruction.ArithShiftRight32, resultHigh, resultLow, transformContext.CreateConstant(31));
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}
	}
}
