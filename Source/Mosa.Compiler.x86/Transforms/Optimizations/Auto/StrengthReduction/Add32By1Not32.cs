// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.Optimizations.Auto.StrengthReduction;

[Transform("x86.Optimizations.Auto.StrengthReduction")]
public sealed class Add32By1Not32 : BaseTransform
{
	public Add32By1Not32() : base(X86.Add32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsConstantOne)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != X86.Not32)
			return false;

		if (!IsVirtualRegister(context.Operand1.Definitions[0].Operand1))
			return false;

		if (IsCarryFlagUsed(context))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;

		context.SetInstruction(X86.Neg32, result, t1);
	}
}

[Transform("x86.Optimizations.Auto.StrengthReduction")]
public sealed class Add32By1Not32_v1 : BaseTransform
{
	public Add32By1Not32_v1() : base(X86.Add32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsConstantOne)
			return false;

		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != X86.Not32)
			return false;

		if (!IsVirtualRegister(context.Operand2.Definitions[0].Operand1))
			return false;

		if (IsCarryFlagUsed(context))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand2.Definitions[0].Operand1;

		context.SetInstruction(X86.Neg32, result, t1);
	}
}
