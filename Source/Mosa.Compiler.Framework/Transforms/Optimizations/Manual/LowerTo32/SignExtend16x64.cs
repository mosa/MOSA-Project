// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.LowerTo32
{
	public sealed class SignExtend16x64 : BaseTransform
	{
		public SignExtend16x64() : base(IRInstruction.SignExtend16x64, TransformType.Manual | TransformType.Optimization)
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
			var v1 = transform.AllocateVirtualRegister32();

			context.SetInstruction(IRInstruction.GetLow32, v1, operand1);
			context.AppendInstruction(IRInstruction.SignExtend16x32, resultLow, v1);
			context.AppendInstruction(IRInstruction.ArithShiftRight32, resultHigh, resultLow, transform.CreateConstant(31));
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}
	}
}
