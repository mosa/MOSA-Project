// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding;

[Transform("IR.Optimizations.Auto.ConstantFolding")]
public sealed class MulR8x2 : BaseTransform
{
	public MulR8x2() : base(IR.MulR8, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 90;

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IR.MulR8)
			return false;

		if (!IsResolvedConstant(context.Operand1.Definitions[0].Operand2))
			return false;

		if (!IsResolvedConstant(context.Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand2;
		var t3 = context.Operand2;

		var e1 = Operand.CreateConstant(MulR8(ToR8(t2), ToR8(t3)));

		context.SetInstruction(IR.MulR8, result, t1, e1);
	}
}

[Transform("IR.Optimizations.Auto.ConstantFolding")]
public sealed class MulR8x2_v1 : BaseTransform
{
	public MulR8x2_v1() : base(IR.MulR8, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 90;

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IR.MulR8)
			return false;

		if (!IsResolvedConstant(context.Operand2.Definitions[0].Operand2))
			return false;

		if (!IsResolvedConstant(context.Operand1))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;
		var t2 = context.Operand2.Definitions[0].Operand1;
		var t3 = context.Operand2.Definitions[0].Operand2;

		var e1 = Operand.CreateConstant(MulR8(ToR8(t3), ToR8(t1)));

		context.SetInstruction(IR.MulR8, result, t2, e1);
	}
}

[Transform("IR.Optimizations.Auto.ConstantFolding")]
public sealed class MulR8x2_v2 : BaseTransform
{
	public MulR8x2_v2() : base(IR.MulR8, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 90;

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IR.MulR8)
			return false;

		if (!IsResolvedConstant(context.Operand1.Definitions[0].Operand1))
			return false;

		if (!IsResolvedConstant(context.Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand2;
		var t3 = context.Operand2;

		var e1 = Operand.CreateConstant(MulR8(ToR8(t1), ToR8(t3)));

		context.SetInstruction(IR.MulR8, result, t2, e1);
	}
}

[Transform("IR.Optimizations.Auto.ConstantFolding")]
public sealed class MulR8x2_v3 : BaseTransform
{
	public MulR8x2_v3() : base(IR.MulR8, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 90;

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IR.MulR8)
			return false;

		if (!IsResolvedConstant(context.Operand2.Definitions[0].Operand1))
			return false;

		if (!IsResolvedConstant(context.Operand1))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;
		var t2 = context.Operand2.Definitions[0].Operand1;
		var t3 = context.Operand2.Definitions[0].Operand2;

		var e1 = Operand.CreateConstant(MulR8(ToR8(t2), ToR8(t1)));

		context.SetInstruction(IR.MulR8, result, t3, e1);
	}
}
