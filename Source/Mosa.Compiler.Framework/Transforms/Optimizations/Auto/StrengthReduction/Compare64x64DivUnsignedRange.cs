// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.StrengthReduction;

[Transform()]
public sealed class Compare64x64DivUnsignedRange : BaseTransform
{
	public Compare64x64DivUnsignedRange() : base(IR.Compare64x64, TransformType.Auto | TransformType.Optimization)
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

		if (context.Operand1.Definitions[0].Instruction != IR.DivUnsigned64)
			return false;

		if (!IsResolvedConstant(context.Operand2))
			return false;

		if (!IsResolvedConstant(context.Operand1.Definitions[0].Operand2))
			return false;

		if (IsZero(context.Operand1.Definitions[0].Operand2))
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

		var e1 = Operand.CreateConstant(MulUnsigned64(To64(t2), To64(t3)));

		context.SetInstruction(IR.Sub64, v1, t1, e1);
		context.AppendInstruction(IR.Compare64x64, ConditionCode.UnsignedLess, result, v1, t2);
	}
}

[Transform()]
public sealed class Compare64x64DivUnsignedRange_v1 : BaseTransform
{
	public Compare64x64DivUnsignedRange_v1() : base(IR.Compare64x64, TransformType.Auto | TransformType.Optimization)
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

		if (context.Operand2.Definitions[0].Instruction != IR.DivUnsigned64)
			return false;

		if (!IsResolvedConstant(context.Operand1))
			return false;

		if (!IsResolvedConstant(context.Operand2.Definitions[0].Operand2))
			return false;

		if (IsZero(context.Operand2.Definitions[0].Operand2))
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

		var e1 = Operand.CreateConstant(MulUnsigned64(To64(t3), To64(t1)));

		context.SetInstruction(IR.Sub64, v1, t2, e1);
		context.AppendInstruction(IR.Compare64x64, ConditionCode.UnsignedLess, result, v1, t3);
	}
}
