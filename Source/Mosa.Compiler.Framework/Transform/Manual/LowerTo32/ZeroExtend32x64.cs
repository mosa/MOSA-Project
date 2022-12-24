﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.LowerTo32
{
	public sealed class ZeroExtend32x64 : BaseTransformation
	{
		public ZeroExtend32x64() : base(IRInstruction.ZeroExtend32x64)
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

			context.SetInstruction(IRInstruction.GetLow32, resultLow, operand1);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, transformContext.ConstantZero32);
		}
	}
}
