// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.Optimizations.Auto.Specific;

public sealed class And64Add64ToBlsr64 : BaseTransform
{
	public And64Add64ToBlsr64() : base(X64.And64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != X64.Add64)
			return false;

		if (!context.Operand2.Definitions[0].Operand2.IsResolvedConstant)
			return false;

		if (context.Operand2.Definitions[0].Operand2.ConstantUnsigned64 != 18446744073709551615)
			return false;

		if (!AreSame(context.Operand1, context.Operand2.Definitions[0].Operand1))
			return false;

		if (!IsVirtualRegister(context.Operand1))
			return false;

		if (IsCarryFlagUsed(context))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;

		context.SetInstruction(X64.Blsr64, result, t1);
	}
}

public sealed class And64Add64ToBlsr64_v1 : BaseTransform
{
	public And64Add64ToBlsr64_v1() : base(X64.And64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != X64.Add64)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsResolvedConstant)
			return false;

		if (context.Operand1.Definitions[0].Operand2.ConstantUnsigned64 != 18446744073709551615)
			return false;

		if (!AreSame(context.Operand1.Definitions[0].Operand1, context.Operand2))
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

		context.SetInstruction(X64.Blsr64, result, t1);
	}
}

public sealed class And64Add64ToBlsr64_v2 : BaseTransform
{
	public And64Add64ToBlsr64_v2() : base(X64.And64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != X64.Add64)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsResolvedConstant)
			return false;

		if (context.Operand2.Definitions[0].Operand1.ConstantUnsigned64 != 18446744073709551615)
			return false;

		if (!AreSame(context.Operand1, context.Operand2.Definitions[0].Operand2))
			return false;

		if (!IsVirtualRegister(context.Operand1))
			return false;

		if (IsCarryFlagUsed(context))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;

		context.SetInstruction(X64.Blsr64, result, t1);
	}
}

public sealed class And64Add64ToBlsr64_v3 : BaseTransform
{
	public And64Add64ToBlsr64_v3() : base(X64.And64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != X64.Add64)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsResolvedConstant)
			return false;

		if (context.Operand1.Definitions[0].Operand1.ConstantUnsigned64 != 18446744073709551615)
			return false;

		if (!AreSame(context.Operand1.Definitions[0].Operand2, context.Operand2))
			return false;

		if (!IsVirtualRegister(context.Operand1.Definitions[0].Operand2))
			return false;

		if (IsCarryFlagUsed(context))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand2;

		context.SetInstruction(X64.Blsr64, result, t1);
	}
}
