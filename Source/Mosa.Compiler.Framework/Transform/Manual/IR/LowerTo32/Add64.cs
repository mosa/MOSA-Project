﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.IR.LowerTo32
{
	public sealed class Add64 : BaseTransformation
	{
		public Add64() : base(IRInstruction.Add64)
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
			var operand2 = context.Operand2;

			var op0Low = transformContext.AllocateVirtualRegister32();
			var op0High = transformContext.AllocateVirtualRegister32();
			var op1Low = transformContext.AllocateVirtualRegister32();
			var op1High = transformContext.AllocateVirtualRegister32();
			var resultLow = transformContext.AllocateVirtualRegister32();
			var resultHigh = transformContext.AllocateVirtualRegister32();
			var resultCarry = transformContext.AllocateVirtualRegister32();

			context.SetInstruction(IRInstruction.GetLow64, op0Low, operand1);
			context.AppendInstruction(IRInstruction.GetHigh64, op0High, operand1);
			context.AppendInstruction(IRInstruction.GetLow64, op1Low, operand2);
			context.AppendInstruction(IRInstruction.GetHigh64, op1High, operand2);

			context.AppendInstruction2(IRInstruction.AddCarryOut32, resultLow, resultCarry, op0Low, op1Low);
			context.AppendInstruction(IRInstruction.AddCarryIn32, resultHigh, op0High, op1High, resultCarry);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}
	}
}
