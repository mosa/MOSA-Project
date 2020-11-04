// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.LowerTo32
{
	public sealed class Compare64x32EqualOrNotEqual : BaseTransformation
	{
		public Compare64x32EqualOrNotEqual() : base(IRInstruction.Compare64x32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (context.ConditionCode != ConditionCode.Equal && context.ConditionCode != ConditionCode.NotEqual)
				return false;

			return transformContext.LowerTo32;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var condition = context.ConditionCode;

			var op0Low = transformContext.AllocateVirtualRegister32();
			var op0High = transformContext.AllocateVirtualRegister32();
			var op1Low = transformContext.AllocateVirtualRegister32();
			var op1High = transformContext.AllocateVirtualRegister32();

			var v1 = transformContext.AllocateVirtualRegister32();
			var v2 = transformContext.AllocateVirtualRegister32();
			var v3 = transformContext.AllocateVirtualRegister32();

			context.SetInstruction(IRInstruction.GetLow32, op0Low, operand1);
			context.AppendInstruction(IRInstruction.GetHigh32, op0High, operand1);
			context.AppendInstruction(IRInstruction.GetLow32, op1Low, operand2);
			context.AppendInstruction(IRInstruction.GetHigh32, op1High, operand2);

			context.AppendInstruction(IRInstruction.Xor32, v1, op0Low, op1Low);
			context.AppendInstruction(IRInstruction.Xor32, v2, op0High, op1High);
			context.AppendInstruction(IRInstruction.Or32, v3, v1, v2);
			context.AppendInstruction(IRInstruction.Compare32x32, condition, result, v3, transformContext.ConstantZero32);
		}
	}
}
