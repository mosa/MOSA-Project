﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.LowerTo32
{
	public sealed class Move64 : BaseTransform
	{
		public Move64() : base(IRInstruction.Move64, TransformType.Manual | TransformType.Optimization)
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

		context.SetInstruction(IRInstruction.GetLow32, resultLow, operand1);
		context.AppendInstruction(IRInstruction.GetHigh32, resultHigh, operand1);
		context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
	}
}
