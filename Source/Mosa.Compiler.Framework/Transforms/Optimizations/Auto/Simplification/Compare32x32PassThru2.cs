// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.Simplification;

public sealed class Compare32x32PassThru2 : BaseTransform
{
	public Compare32x32PassThru2() : base(IR.Compare32x32, TransformType.Auto | TransformType.Optimization, 95)
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

		var e1 = Operand.Constant32_1;

		context.SetInstruction(IR.And32, result, t1, e1);
	}
}

public sealed class Compare32x32PassThru2_v1 : BaseTransform
{
	public Compare32x32PassThru2_v1() : base(IR.Compare32x32, TransformType.Auto | TransformType.Optimization, 95)
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

		var e1 = Operand.Constant32_1;

		context.SetInstruction(IR.And32, result, t1, e1);
	}
}

public sealed class Compare32x32PassThru2_v2 : BaseTransform
{
	public Compare32x32PassThru2_v2() : base(IR.Compare32x32, TransformType.Auto | TransformType.Optimization, 95)
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

		var e1 = Operand.Constant32_1;

		context.SetInstruction(IR.And32, result, t1, e1);
	}
}

public sealed class Compare32x32PassThru2_v3 : BaseTransform
{
	public Compare32x32PassThru2_v3() : base(IR.Compare32x32, TransformType.Auto | TransformType.Optimization, 95)
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

		var e1 = Operand.Constant32_1;

		context.SetInstruction(IR.And32, result, t1, e1);
	}
}
