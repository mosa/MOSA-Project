// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.Simplification;

[Transform()]
public sealed class Compare64x64PassThru2 : BaseTransform
{
	public Compare64x64PassThru2() : base(IR.Compare64x64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (context.ConditionCode != ConditionCode.Equal)
			return false;

		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsConstantOne)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IR.And64)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsConstantOne)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;

		var e1 = Operand.Constant64_1;

		context.SetInstruction(IR.And64, result, t1, e1);
	}
}

[Transform()]
public sealed class Compare64x64PassThru2_v1 : BaseTransform
{
	public Compare64x64PassThru2_v1() : base(IR.Compare64x64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (context.ConditionCode != ConditionCode.Equal)
			return false;

		if (!context.Operand1.IsConstantOne)
			return false;

		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IR.And64)
			return false;

		if (!context.Operand2.Definitions[0].Operand2.IsConstantOne)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand2.Definitions[0].Operand1;

		var e1 = Operand.Constant64_1;

		context.SetInstruction(IR.And64, result, t1, e1);
	}
}

[Transform()]
public sealed class Compare64x64PassThru2_v2 : BaseTransform
{
	public Compare64x64PassThru2_v2() : base(IR.Compare64x64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (context.ConditionCode != ConditionCode.Equal)
			return false;

		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsConstantOne)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IR.And64)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsConstantOne)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand2;

		var e1 = Operand.Constant64_1;

		context.SetInstruction(IR.And64, result, t1, e1);
	}
}

[Transform()]
public sealed class Compare64x64PassThru2_v3 : BaseTransform
{
	public Compare64x64PassThru2_v3() : base(IR.Compare64x64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (context.ConditionCode != ConditionCode.Equal)
			return false;

		if (!context.Operand1.IsConstantOne)
			return false;

		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IR.And64)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsConstantOne)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand2.Definitions[0].Operand2;

		var e1 = Operand.Constant64_1;

		context.SetInstruction(IR.And64, result, t1, e1);
	}
}
