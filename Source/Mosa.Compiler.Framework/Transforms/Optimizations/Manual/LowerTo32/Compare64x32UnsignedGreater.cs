﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.LowerTo32
{
	// NOT WORKING!!!
	public sealed class Compare64x32UnsignedGreater : BaseTransformation
	{
		public Compare64x32UnsignedGreater() : base(IRInstruction.Compare64x32, TransformationType.Manual | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (context.ConditionCode != ConditionCode.UnsignedGreater)
				return false;

			return transform.LowerTo32;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var op0Low = transform.AllocateVirtualRegister32();
			var op0High = transform.AllocateVirtualRegister32();
			var op1Low = transform.AllocateVirtualRegister32();
			var op1High = transform.AllocateVirtualRegister32();

			var v1 = transform.AllocateVirtualRegister32();
			var v2 = transform.AllocateVirtualRegister32();
			var v3 = transform.AllocateVirtualRegister32();
			var v4 = transform.AllocateVirtualRegister32();
			var v5 = transform.AllocateVirtualRegister32();

			context.SetInstruction(IRInstruction.GetLow32, op0Low, operand1);
			context.AppendInstruction(IRInstruction.GetHigh32, op0High, operand1);
			context.AppendInstruction(IRInstruction.GetLow32, op1Low, operand2);
			context.AppendInstruction(IRInstruction.GetHigh32, op1High, operand2);

			context.AppendInstruction(IRInstruction.Compare32x32, ConditionCode.UnsignedGreater, v1, op0High, op1High);
			context.AppendInstruction(IRInstruction.Compare32x32, ConditionCode.Equal, v2, op0High, op1High);
			context.AppendInstruction(IRInstruction.Compare32x32, ConditionCode.UnsignedGreater, v3, op0Low, op1Low);

			context.AppendInstruction(IRInstruction.And32, v4, v2, v3);
			context.AppendInstruction(IRInstruction.Or32, v5, v1, v4);

			//context.AppendInstruction(IRInstruction.And32, result, v5, transform.CreateConstant((uint)1));
			context.AppendInstruction(IRInstruction.IfThenElse32, result, v5, transform.CreateConstant((uint)1), transform.ConstantZero32);
		}
	}
}
