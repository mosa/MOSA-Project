// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.StrengthReduction;

/// <summary>
/// IsolateAndFlipLeastSignificantBit32
/// </summary>
[Transform("IR.Optimizations.Auto.StrengthReduction")]
public sealed class IsolateAndFlipLeastSignificantBit32 : BaseTransform
{
	public IsolateAndFlipLeastSignificantBit32() : base(IRInstruction.Add32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsResolvedConstant)
			return false;

		if (context.Operand2.ConstantUnsigned64 != 1)
			return false;

		if (context.Operand1.Definitions.Count != 1)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IRInstruction.ShiftRight32)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsResolvedConstant)
			return false;

		if (context.Operand1.Definitions[0].Operand2.ConstantUnsigned64 != 31)
			return false;

		if (context.Operand1.Definitions[0].Operand1.Definitions.Count != 1)
			return false;

		if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.ShiftLeft32)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2.IsResolvedConstant)
			return false;

		if (context.Operand1.Definitions[0].Operand1.Definitions[0].Operand2.ConstantUnsigned64 != 31)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1;

		var v1 = transform.VirtualRegisters.Allocate32();

		var e1 = transform.CreateConstant(To32(1));

		context.SetInstruction(IRInstruction.Not32, v1, t1);
		context.AppendInstruction(IRInstruction.And32, result, v1, e1);
	}
}

/// <summary>
/// IsolateAndFlipLeastSignificantBit32_v1
/// </summary>
[Transform("IR.Optimizations.Auto.StrengthReduction")]
public sealed class IsolateAndFlipLeastSignificantBit32_v1 : BaseTransform
{
	public IsolateAndFlipLeastSignificantBit32_v1() : base(IRInstruction.Add32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand1.IsResolvedConstant)
			return false;

		if (context.Operand1.ConstantUnsigned64 != 1)
			return false;

		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (context.Operand2.Definitions.Count != 1)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IRInstruction.ShiftRight32)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.Definitions[0].Operand2.IsResolvedConstant)
			return false;

		if (context.Operand2.Definitions[0].Operand2.ConstantUnsigned64 != 31)
			return false;

		if (context.Operand2.Definitions[0].Operand1.Definitions.Count != 1)
			return false;

		if (context.Operand2.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.ShiftLeft32)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.Definitions[0].Operand2.IsResolvedConstant)
			return false;

		if (context.Operand2.Definitions[0].Operand1.Definitions[0].Operand2.ConstantUnsigned64 != 31)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var t1 = context.Operand2.Definitions[0].Operand1.Definitions[0].Operand1;

		var v1 = transform.VirtualRegisters.Allocate32();

		var e1 = transform.CreateConstant(To32(1));

		context.SetInstruction(IRInstruction.Not32, v1, t1);
		context.AppendInstruction(IRInstruction.And32, result, v1, e1);
	}
}
