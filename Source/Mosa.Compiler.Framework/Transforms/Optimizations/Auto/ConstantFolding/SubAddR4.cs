// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding;

/// <summary>
/// SubAddR4
/// </summary>
public sealed class SubAddR4 : BaseTransform
{
	public SubAddR4() : base(IRInstruction.SubR4, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 90;

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (context.Operand1.Definitions.Count != 1)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IRInstruction.AddR4)
			return false;

		if (!IsResolvedConstant(context.Operand1.Definitions[0].Operand2))
			return false;

		if (!IsResolvedConstant(context.Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand2;
		var t3 = context.Operand2;

		var e1 = transform.CreateConstant(SubR4(ToR4(t2), ToR4(t3)));

		context.SetInstruction(IRInstruction.AddR4, result, t1, e1);
	}
}

/// <summary>
/// SubAddR4_v1
/// </summary>
public sealed class SubAddR4_v1 : BaseTransform
{
	public SubAddR4_v1() : base(IRInstruction.SubR4, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 90;

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (context.Operand1.Definitions.Count != 1)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IRInstruction.AddR4)
			return false;

		if (!IsResolvedConstant(context.Operand1.Definitions[0].Operand1))
			return false;

		if (!IsResolvedConstant(context.Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand2;
		var t3 = context.Operand2;

		var e1 = transform.CreateConstant(SubR4(ToR4(t1), ToR4(t3)));

		context.SetInstruction(IRInstruction.AddR4, result, t2, e1);
	}
}
