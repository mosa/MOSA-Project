// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding;

public sealed class Xor64x2 : BaseTransform
{
	public Xor64x2() : base(IR.Xor64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 90;

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IR.Xor64)
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

		var e1 = Operand.CreateConstant(Xor64(To64(t2), To64(t3)));

		context.SetInstruction(IR.Xor64, result, t1, e1);
	}
}

public sealed class Xor64x2_v1 : BaseTransform
{
	public Xor64x2_v1() : base(IR.Xor64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 90;

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IR.Xor64)
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

		var e1 = Operand.CreateConstant(Xor64(To64(t3), To64(t1)));

		context.SetInstruction(IR.Xor64, result, t2, e1);
	}
}

public sealed class Xor64x2_v2 : BaseTransform
{
	public Xor64x2_v2() : base(IR.Xor64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 90;

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IR.Xor64)
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

		var e1 = Operand.CreateConstant(Xor64(To64(t1), To64(t3)));

		context.SetInstruction(IR.Xor64, result, t2, e1);
	}
}

public sealed class Xor64x2_v3 : BaseTransform
{
	public Xor64x2_v3() : base(IR.Xor64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 90;

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IR.Xor64)
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

		var e1 = Operand.CreateConstant(Xor64(To64(t2), To64(t1)));

		context.SetInstruction(IR.Xor64, result, t3, e1);
	}
}
