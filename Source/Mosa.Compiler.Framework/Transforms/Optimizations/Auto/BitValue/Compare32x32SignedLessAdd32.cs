// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.BitValue;

/// <summary>
/// Compare32x32SignedLessAdd32
/// </summary>
[Transform("IR.Optimizations.Auto.BitValue")]
public sealed class Compare32x32SignedLessAdd32 : BaseTransform
{
	public Compare32x32SignedLessAdd32() : base(IR.Compare32x32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (context.ConditionCode != ConditionCode.Less)
			return false;

		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IR.Add32)
			return false;

		if (!IsResolvedConstant(context.Operand1.Definitions[0].Operand2))
			return false;

		if (!IsResolvedConstant(context.Operand2))
			return false;

		if (IsAddOverflow32(BitValueMax32(context.Operand1.Definitions[0].Operand1), BitValueMax32(context.Operand1.Definitions[0].Operand2)))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand2;
		var t3 = context.Operand2;

		var v1 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(IR.Sub32, v1, t3, t2);
		context.AppendInstruction(IR.Compare32x32, ConditionCode.Less, result, t1, v1);
	}
}

/// <summary>
/// Compare32x32SignedLessAdd32_v1
/// </summary>
[Transform("IR.Optimizations.Auto.BitValue")]
public sealed class Compare32x32SignedLessAdd32_v1 : BaseTransform
{
	public Compare32x32SignedLessAdd32_v1() : base(IR.Compare32x32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (context.ConditionCode != ConditionCode.Greater)
			return false;

		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IR.Add32)
			return false;

		if (!IsResolvedConstant(context.Operand2.Definitions[0].Operand2))
			return false;

		if (!IsResolvedConstant(context.Operand1))
			return false;

		if (IsAddOverflow32(BitValueMax32(context.Operand2.Definitions[0].Operand1), BitValueMax32(context.Operand2.Definitions[0].Operand2)))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;
		var t2 = context.Operand2.Definitions[0].Operand1;
		var t3 = context.Operand2.Definitions[0].Operand2;

		var v1 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(IR.Sub32, v1, t1, t3);
		context.AppendInstruction(IR.Compare32x32, ConditionCode.Less, result, t2, v1);
	}
}
