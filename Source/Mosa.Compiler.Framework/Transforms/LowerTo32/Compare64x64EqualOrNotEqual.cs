// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

public sealed class Compare64x64EqualOrNotEqual : BaseLowerTo32Transform
{
	public Compare64x64EqualOrNotEqual() : base(IRInstruction.Compare64x64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!base.Match(context, transform))
			return false;

		if (context.ConditionCode != ConditionCode.Equal && context.ConditionCode != ConditionCode.NotEqual)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;
		var condition = context.ConditionCode;

		var op0Low = transform.VirtualRegisters.Allocate32();
		var op0High = transform.VirtualRegisters.Allocate32();
		var op1Low = transform.VirtualRegisters.Allocate32();
		var op1High = transform.VirtualRegisters.Allocate32();

		var v1 = transform.VirtualRegisters.Allocate32();
		var v2 = transform.VirtualRegisters.Allocate32();
		var v3 = transform.VirtualRegisters.Allocate32();

		transform.SplitOperand(result, out Operand resultLow, out Operand resultHigh);

		context.SetInstruction(IRInstruction.GetLow32, op0Low, operand1);
		context.AppendInstruction(IRInstruction.GetHigh32, op0High, operand1);
		context.AppendInstruction(IRInstruction.GetLow32, op1Low, operand2);
		context.AppendInstruction(IRInstruction.GetHigh32, op1High, operand2);

		context.AppendInstruction(IRInstruction.Xor32, v1, op0Low, op1Low);
		context.AppendInstruction(IRInstruction.Xor32, v2, op0High, op1High);
		context.AppendInstruction(IRInstruction.Or32, v3, v1, v2);
		context.AppendInstruction(IRInstruction.Compare32x32, condition, resultLow, v3, Operand.Constant32_0);
		context.AppendInstruction(IRInstruction.Move32, condition, resultHigh, Operand.Constant32_0);
	}
}
