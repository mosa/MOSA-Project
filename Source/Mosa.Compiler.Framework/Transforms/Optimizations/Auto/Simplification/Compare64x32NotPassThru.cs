// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.Simplification;

[Transform("IR.Optimizations.Auto.Simplification")]
public sealed class Compare64x32NotPassThru : BaseTransform
{
	public Compare64x32NotPassThru() : base(IR.Compare64x32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (context.ConditionCode != ConditionCode.Equal)
			return false;

		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsResolvedConstant)
			return false;

		if (context.Operand2.ConstantUnsigned64 != 0)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IR.And32)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsResolvedConstant)
			return false;

		if (context.Operand1.Definitions[0].Operand2.ConstantUnsigned64 != 1)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;

		var v1 = transform.VirtualRegisters.Allocate32();

		var c1 = Operand.CreateConstant(1);

		context.SetInstruction(IR.Not32, v1, t1);
		context.AppendInstruction(IR.And32, result, v1, c1);
	}
}

[Transform("IR.Optimizations.Auto.Simplification")]
public sealed class Compare64x32NotPassThru_v1 : BaseTransform
{
	public Compare64x32NotPassThru_v1() : base(IR.Compare64x32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (context.ConditionCode != ConditionCode.Equal)
			return false;

		if (!context.Operand1.IsResolvedConstant)
			return false;

		if (context.Operand1.ConstantUnsigned64 != 0)
			return false;

		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IR.And32)
			return false;

		if (!context.Operand2.Definitions[0].Operand2.IsResolvedConstant)
			return false;

		if (context.Operand2.Definitions[0].Operand2.ConstantUnsigned64 != 1)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand2.Definitions[0].Operand1;

		var v1 = transform.VirtualRegisters.Allocate32();

		var c1 = Operand.CreateConstant(1);

		context.SetInstruction(IR.Not32, v1, t1);
		context.AppendInstruction(IR.And32, result, v1, c1);
	}
}

[Transform("IR.Optimizations.Auto.Simplification")]
public sealed class Compare64x32NotPassThru_v2 : BaseTransform
{
	public Compare64x32NotPassThru_v2() : base(IR.Compare64x32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (context.ConditionCode != ConditionCode.Equal)
			return false;

		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsResolvedConstant)
			return false;

		if (context.Operand2.ConstantUnsigned64 != 0)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IR.And32)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsResolvedConstant)
			return false;

		if (context.Operand1.Definitions[0].Operand1.ConstantUnsigned64 != 1)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand2;

		var v1 = transform.VirtualRegisters.Allocate32();

		var c1 = Operand.CreateConstant(1);

		context.SetInstruction(IR.Not32, v1, t1);
		context.AppendInstruction(IR.And32, result, v1, c1);
	}
}

[Transform("IR.Optimizations.Auto.Simplification")]
public sealed class Compare64x32NotPassThru_v3 : BaseTransform
{
	public Compare64x32NotPassThru_v3() : base(IR.Compare64x32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (context.ConditionCode != ConditionCode.Equal)
			return false;

		if (!context.Operand1.IsResolvedConstant)
			return false;

		if (context.Operand1.ConstantUnsigned64 != 0)
			return false;

		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IR.And32)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsResolvedConstant)
			return false;

		if (context.Operand2.Definitions[0].Operand1.ConstantUnsigned64 != 1)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand2.Definitions[0].Operand2;

		var v1 = transform.VirtualRegisters.Allocate32();

		var c1 = Operand.CreateConstant(1);

		context.SetInstruction(IR.Not32, v1, t1);
		context.AppendInstruction(IR.And32, result, v1, c1);
	}
}
