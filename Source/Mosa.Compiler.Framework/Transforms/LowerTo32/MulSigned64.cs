// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

public sealed class MulSigned64 : BaseLower32Transform
{
	public MulSigned64() : base(IRInstruction.MulSigned64, TransformType.Manual | TransformType.Optimization, true)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
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

		//mov eax, DWORD PTR[esp + 8]     // eax =op0Low
		//mov edx, DWORD PTR[esp + 16]    // edx = op0High
		//mov ecx, DWORD PTR[esp + 12]    // ecx = op1Low
		//mov ebx, DWORD PTR[esp + 20]    // ebx = op1High

		// imul ebx (op1High), eax (op0Low)
		context.AppendInstruction(IRInstruction.MulSigned32, v1, op1High, op0Low);

		// imul ecx (op1Low), edx (op0High)
		context.AppendInstruction(IRInstruction.MulSigned32, v2, op1Low, op0High);

		// mul edx (op0High), eax (op0Low)
		context.AppendInstruction(IRInstruction.MulHu32, v3, op0High, op0Low);
		// v3 = edx

		context.AppendInstruction(IRInstruction.MulUnsigned32, v6, op0High, op0Low);
		// v6 = eax

		// add ecx (v2), ebx (v1)
		context.AppendInstruction(IRInstruction.Add32, v4, v2, v1);

		// add edx (v3), ecx (v4)
		context.AppendInstruction(IRInstruction.Add32, v5, v3, v4);

		context.AppendInstruction(IRInstruction.To64, result, v6, v5);
	}
}
