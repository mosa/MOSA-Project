// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding;

/// <summary>
/// MulSigned32x2
/// </summary>
[Transform("IR.Optimizations.Auto.ConstantFolding")]
public sealed class MulSigned32x2 : BaseTransform
{
	public MulSigned32x2() : base(IRInstruction.MulSigned32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 90;

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IRInstruction.MulSigned32)
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

		var e1 = Operand.CreateConstant(MulSigned32(ToSigned32(t2), ToSigned32(t3)));

		context.SetInstruction(IRInstruction.MulSigned32, result, t1, e1);
	}
}

/// <summary>
/// MulSigned32x2_v1
/// </summary>
[Transform("IR.Optimizations.Auto.ConstantFolding")]
public sealed class MulSigned32x2_v1 : BaseTransform
{
	public MulSigned32x2_v1() : base(IRInstruction.MulSigned32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 90;

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IRInstruction.MulSigned32)
			return false;

		if (!IsResolvedConstant(context.Operand2.Definitions[0].Operand2))
			return false;

		if (!IsResolvedConstant(context.Operand1))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;
		var t2 = context.Operand2.Definitions[0].Operand1;
		var t3 = context.Operand2.Definitions[0].Operand2;

		var e1 = Operand.CreateConstant(MulSigned32(ToSigned32(t3), ToSigned32(t1)));

		context.SetInstruction(IRInstruction.MulSigned32, result, t2, e1);
	}
}

/// <summary>
/// MulSigned32x2_v2
/// </summary>
[Transform("IR.Optimizations.Auto.ConstantFolding")]
public sealed class MulSigned32x2_v2 : BaseTransform
{
	public MulSigned32x2_v2() : base(IRInstruction.MulSigned32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 90;

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IRInstruction.MulSigned32)
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

		var e1 = Operand.CreateConstant(MulSigned32(ToSigned32(t1), ToSigned32(t3)));

		context.SetInstruction(IRInstruction.MulSigned32, result, t2, e1);
	}
}

/// <summary>
/// MulSigned32x2_v3
/// </summary>
[Transform("IR.Optimizations.Auto.ConstantFolding")]
public sealed class MulSigned32x2_v3 : BaseTransform
{
	public MulSigned32x2_v3() : base(IRInstruction.MulSigned32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 90;

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IRInstruction.MulSigned32)
			return false;

		if (!IsResolvedConstant(context.Operand2.Definitions[0].Operand1))
			return false;

		if (!IsResolvedConstant(context.Operand1))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;
		var t2 = context.Operand2.Definitions[0].Operand1;
		var t3 = context.Operand2.Definitions[0].Operand2;

		var e1 = Operand.CreateConstant(MulSigned32(ToSigned32(t2), ToSigned32(t1)));

		context.SetInstruction(IRInstruction.MulSigned32, result, t3, e1);
	}
}
