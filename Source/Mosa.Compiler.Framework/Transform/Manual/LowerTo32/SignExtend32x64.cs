// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.LowerTo32
{
	public sealed class SignExtend32x64 : BaseTransformation
	{
		public SignExtend32x64() : base(IRInstruction.SignExtend32x64)
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

			context.SetInstruction(IRInstruction.Move32, resultLow, operand1);
			context.AppendInstruction(IRInstruction.ArithShiftRight32, resultHigh, resultLow, transformContext.CreateConstant(31));
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}
	}
}
