// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.Simplification;

[Transform()]
public sealed class Compare64x32SwapToZero : BaseTransform
{
	public Compare64x32SwapToZero() : base(IR.Compare64x32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (context.ConditionCode != ConditionCode.NotEqual)
			return false;

		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsConstantOne)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IR.And32)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsConstantOne)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;

		var v1 = transform.VirtualRegisters.Allocate32();

		var c1 = Operand.CreateConstant(0);
		var c2 = Operand.CreateConstant(1);

		context.SetInstruction(IR.And32, v1, t1, c2);
		context.AppendInstruction(IR.Compare64x32, ConditionCode.Equal, result, v1, c1);
	}
}

[Transform()]
public sealed class Compare64x32SwapToZero_v1 : BaseTransform
{
	public Compare64x32SwapToZero_v1() : base(IR.Compare64x32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (context.ConditionCode != ConditionCode.NotEqual)
			return false;

		if (!context.Operand1.IsConstantOne)
			return false;

		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IR.And32)
			return false;

		if (!context.Operand2.Definitions[0].Operand2.IsConstantOne)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand2.Definitions[0].Operand1;

		var v1 = transform.VirtualRegisters.Allocate32();

		var c1 = Operand.CreateConstant(0);
		var c2 = Operand.CreateConstant(1);

		context.SetInstruction(IR.And32, v1, t1, c2);
		context.AppendInstruction(IR.Compare64x32, ConditionCode.Equal, result, v1, c1);
	}
}

[Transform()]
public sealed class Compare64x32SwapToZero_v2 : BaseTransform
{
	public Compare64x32SwapToZero_v2() : base(IR.Compare64x32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (context.ConditionCode != ConditionCode.NotEqual)
			return false;

		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsConstantOne)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IR.And32)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsConstantOne)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand2;

		var v1 = transform.VirtualRegisters.Allocate32();

		var c1 = Operand.CreateConstant(0);
		var c2 = Operand.CreateConstant(1);

		context.SetInstruction(IR.And32, v1, t1, c2);
		context.AppendInstruction(IR.Compare64x32, ConditionCode.Equal, result, v1, c1);
	}
}

[Transform()]
public sealed class Compare64x32SwapToZero_v3 : BaseTransform
{
	public Compare64x32SwapToZero_v3() : base(IR.Compare64x32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (context.ConditionCode != ConditionCode.NotEqual)
			return false;

		if (!context.Operand1.IsConstantOne)
			return false;

		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IR.And32)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsConstantOne)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand2.Definitions[0].Operand2;

		var v1 = transform.VirtualRegisters.Allocate32();

		var c1 = Operand.CreateConstant(0);
		var c2 = Operand.CreateConstant(1);

		context.SetInstruction(IR.And32, v1, t1, c2);
		context.AppendInstruction(IR.Compare64x32, ConditionCode.Equal, result, v1, c1);
	}
}
