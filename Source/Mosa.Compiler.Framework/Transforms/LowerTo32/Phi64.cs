// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

public sealed class Phi64 : BaseLowerTo32Transform
{
	public Phi64() : base(IR.Phi64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!base.Match(context, transform))
			return false;

		if (context.OperandCount == 1)
			return false;

		foreach (var operand in context.Operands)
		{
			if (operand.IsConstant)
				continue;

			if (!operand.IsVirtualRegister || !operand.IsDefinedOnce)
				return false;

			var def = operand.Definitions[0];

			if (def.Instruction != IR.To64)
				return false;

			if (!(def.Operand1.IsDefinedOnce || def.Operand1.IsConstant))
				return false;

			if (!(def.Operand2.IsDefinedOnce || def.Operand2.IsConstant))
				return false;
		}

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var ctx = new Context(context.Node);

		// Low
		var low = transform.VirtualRegisters.Allocate32();
		ctx.AppendInstruction(IR.Phi32, low);
		ctx.PhiBlocks = new List<BasicBlock>(context.PhiBlocks.Count);
		ctx.OperandCount = context.OperandCount;

		for (var i = 0; i < context.OperandCount; i++)
		{
			var operand = context.GetOperand(i);

			var value = operand.IsConstant
				? Operand.CreateConstant32(operand.ConstantUnsigned32)
				: operand.Definitions[0].Operand1;

			ctx.SetOperand(i, value);
			ctx.PhiBlocks.Add(context.PhiBlocks[i]);
		}

		// High
		var high = transform.VirtualRegisters.Allocate32();
		ctx.AppendInstruction(IR.Phi32, high);
		ctx.PhiBlocks = new List<BasicBlock>(context.PhiBlocks.Count);
		ctx.OperandCount = context.OperandCount;

		for (var i = 0; i < context.OperandCount; i++)
		{
			var operand = context.GetOperand(i);

			var value = operand.IsConstant
				? Operand.CreateConstant32(operand.ConstantUnsigned64 >> 32)
				: operand.Definitions[0].Operand2;

			ctx.SetOperand(i, value);
			ctx.PhiBlocks.Add(context.PhiBlocks[i]);
		}

		while (ctx.Node.NextNonEmpty.Instruction.IsPhi)
		{
			ctx.GotoNext();
		}

		ctx.AppendInstruction(IR.To64, context.Result, low, high);

		context.SetNop();
	}
}
