﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.IR.LowerTo32
{
	public sealed class LoadParamZeroExtend16x64 : BaseTransformation
	{
		public LoadParamZeroExtend16x64() : base(IRInstruction.LoadParamZeroExtend16x64)
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

			transformContext.SplitLongOperand(operand1, out Operand op0Low, out Operand _);

			var resultLow = transformContext.AllocateVirtualRegister32();
			context.SetInstruction(IRInstruction.LoadParamZeroExtend16x32, resultLow, op0Low);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, transformContext.ConstantZero32);
		}
	}
}
