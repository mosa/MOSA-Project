// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

public sealed class Phi64 : BaseLower32Transform
{
	public Phi64() : base(IRInstruction.Phi64, TransformType.Manual | TransformType.Optimization, true)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!transform.IsLowerTo32)
			return false;

		if (context.OperandCount != 2)
			return false;

		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IRInstruction.To64)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IRInstruction.To64)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsDefinedOnce)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsDefinedOnce)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsDefinedOnce)
			return false;

		if (!context.Operand2.Definitions[0].Operand2.IsDefinedOnce)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		var blockA = context.PhiBlocks[0];
		var blockB = context.PhiBlocks[1];

		var operand1A = operand1.Definitions[0].Operand1;
		var operand1B = operand1.Definitions[0].Operand2;

		var operand2A = operand2.Definitions[0].Operand1;
		var operand2B = operand2.Definitions[0].Operand2;

		var v1 = transform.VirtualRegisters.Allocate32();
		var v2 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(IRInstruction.Phi32, v1, operand1A, operand2A);
		context.PhiBlocks = new List<BasicBlock> { blockA, blockB };

		context.AppendInstruction(IRInstruction.Phi32, v2, operand1B, operand2B);
		context.PhiBlocks = new List<BasicBlock> { blockA, blockB };

		while (context.Node.NextNonEmpty.Instruction.IsPhi)
		{
			context.GotoNext();
		}

		context.AppendInstruction(IRInstruction.To64, result, v1, v2);
	}
}
