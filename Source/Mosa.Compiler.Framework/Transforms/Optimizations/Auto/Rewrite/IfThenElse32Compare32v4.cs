// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.Rewrite;

public sealed class IfThenElse32Compare32v4 : BaseTransform
{
	public IfThenElse32Compare32v4() : base(IR.IfThenElse32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IR.Compare32x32)
			return false;

		if (context.Operand1.Definitions[0].ConditionCode != ConditionCode.NotEqual)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsConstantZero)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand2;
		var t2 = context.Operand2;
		var t3 = context.Operand3;

		context.SetInstruction(IR.IfThenElse32, result, t1, t2, t3);
	}
}

public sealed class IfThenElse32Compare32v4_v1 : BaseTransform
{
	public IfThenElse32Compare32v4_v1() : base(IR.IfThenElse32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IR.Compare32x32)
			return false;

		if (context.Operand1.Definitions[0].ConditionCode != ConditionCode.NotEqual)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsConstantZero)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;
		var t2 = context.Operand2;
		var t3 = context.Operand3;

		context.SetInstruction(IR.IfThenElse32, result, t1, t2, t3);
	}
}
