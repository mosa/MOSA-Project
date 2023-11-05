// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

public sealed class MulUnsigned64 : BaseLower32Transform
{
	public MulUnsigned64() : base(IRInstruction.MulUnsigned64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		// same code as with MulUnsigned64!

		var result = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		var op0Low = transform.VirtualRegisters.Allocate32();
		var op0High = transform.VirtualRegisters.Allocate32();
		var op1Low = transform.VirtualRegisters.Allocate32();
		var op1High = transform.VirtualRegisters.Allocate32();

		var v1 = transform.VirtualRegisters.Allocate32();
		var v2 = transform.VirtualRegisters.Allocate32();
		var v3 = transform.VirtualRegisters.Allocate32();
		var v4 = transform.VirtualRegisters.Allocate32();
		var v5 = transform.VirtualRegisters.Allocate32();
		var v6 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(IRInstruction.GetLow32, op0Low, operand1);
		context.AppendInstruction(IRInstruction.GetHigh32, op0High, operand1);
		context.AppendInstruction(IRInstruction.GetLow32, op1Low, operand2);
		context.AppendInstruction(IRInstruction.GetHigh32, op1High, operand2);

		context.AppendInstruction(IRInstruction.MulSigned32, v1, op1High, op0Low);
		context.AppendInstruction(IRInstruction.MulSigned32, v2, op1Low, op0High);
		context.AppendInstruction(IRInstruction.Add32, v4, v2, v1);
		context.AppendInstruction(IRInstruction.MulHu32, v3, op1Low, op0Low);
		context.AppendInstruction(IRInstruction.MulUnsigned32, v6, op1Low, op0Low);
		context.AppendInstruction(IRInstruction.Add32, v5, v3, v4);

		context.AppendInstruction(IRInstruction.To64, result, v6, v5);
	}
}
