// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.BitValue;

/// <summary>
/// Compare64x64EqualAdd64
/// </summary>
[Transform("IR.Optimizations.Auto.BitValue")]
public sealed class Compare64x64EqualAdd64 : BaseTransform
{
	public Compare64x64EqualAdd64() : base(Framework.IR.Compare64x64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (context.ConditionCode != ConditionCode.Equal)
			return false;

		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != Framework.IR.Add64)
			return false;

		if (!IsResolvedConstant(context.Operand1.Definitions[0].Operand2))
			return false;

		if (!IsResolvedConstant(context.Operand2))
			return false;

		if (IsAddOverflow64(BitValueMax64(context.Operand1.Definitions[0].Operand1), BitValueMax64(context.Operand1.Definitions[0].Operand2)))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand2;
		var t3 = context.Operand2;

		var v1 = transform.VirtualRegisters.Allocate64();

		context.SetInstruction(Framework.IR.Sub64, v1, t3, t2);
		context.AppendInstruction(Framework.IR.Compare64x64, ConditionCode.Equal, result, t1, v1);
	}
}

/// <summary>
/// Compare64x64EqualAdd64_v1
/// </summary>
[Transform("IR.Optimizations.Auto.BitValue")]
public sealed class Compare64x64EqualAdd64_v1 : BaseTransform
{
	public Compare64x64EqualAdd64_v1() : base(Framework.IR.Compare64x64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (context.ConditionCode != ConditionCode.Equal)
			return false;

		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != Framework.IR.Add64)
			return false;

		if (!IsResolvedConstant(context.Operand2.Definitions[0].Operand2))
			return false;

		if (!IsResolvedConstant(context.Operand1))
			return false;

		if (IsAddOverflow64(BitValueMax64(context.Operand2.Definitions[0].Operand1), BitValueMax64(context.Operand2.Definitions[0].Operand2)))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;
		var t2 = context.Operand2.Definitions[0].Operand1;
		var t3 = context.Operand2.Definitions[0].Operand2;

		var v1 = transform.VirtualRegisters.Allocate64();

		context.SetInstruction(Framework.IR.Sub64, v1, t1, t3);
		context.AppendInstruction(Framework.IR.Compare64x64, ConditionCode.Equal, result, t2, v1);
	}
}
