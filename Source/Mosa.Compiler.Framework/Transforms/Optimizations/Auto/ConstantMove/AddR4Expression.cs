// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantMove;

/// <summary>
/// AddR4Expression
/// </summary>
[Transform("IR.Optimizations.Auto.ConstantMove")]
public sealed class AddR4Expression : BaseTransform
{
	public AddR4Expression() : base(IR.AddR4, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IR.AddR4)
			return false;

		if (IsResolvedConstant(context.Operand1))
			return false;

		if (!IsResolvedConstant(context.Operand2.Definitions[0].Operand2))
			return false;

		if (IsResolvedConstant(context.Operand2.Definitions[0].Operand1))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;
		var t2 = context.Operand2.Definitions[0].Operand1;
		var t3 = context.Operand2.Definitions[0].Operand2;

		var v1 = transform.VirtualRegisters.AllocateR4();

		context.SetInstruction(IR.AddR4, v1, t1, t2);
		context.AppendInstruction(IR.AddR4, result, v1, t3);
	}
}

/// <summary>
/// AddR4Expression_v1
/// </summary>
[Transform("IR.Optimizations.Auto.ConstantMove")]
public sealed class AddR4Expression_v1 : BaseTransform
{
	public AddR4Expression_v1() : base(IR.AddR4, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IR.AddR4)
			return false;

		if (IsResolvedConstant(context.Operand2))
			return false;

		if (!IsResolvedConstant(context.Operand1.Definitions[0].Operand2))
			return false;

		if (IsResolvedConstant(context.Operand1.Definitions[0].Operand1))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand2;
		var t3 = context.Operand2;

		var v1 = transform.VirtualRegisters.AllocateR4();

		context.SetInstruction(IR.AddR4, v1, t3, t1);
		context.AppendInstruction(IR.AddR4, result, v1, t2);
	}
}

/// <summary>
/// AddR4Expression_v2
/// </summary>
[Transform("IR.Optimizations.Auto.ConstantMove")]
public sealed class AddR4Expression_v2 : BaseTransform
{
	public AddR4Expression_v2() : base(IR.AddR4, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IR.AddR4)
			return false;

		if (IsResolvedConstant(context.Operand1))
			return false;

		if (!IsResolvedConstant(context.Operand2.Definitions[0].Operand1))
			return false;

		if (IsResolvedConstant(context.Operand2.Definitions[0].Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;
		var t2 = context.Operand2.Definitions[0].Operand1;
		var t3 = context.Operand2.Definitions[0].Operand2;

		var v1 = transform.VirtualRegisters.AllocateR4();

		context.SetInstruction(IR.AddR4, v1, t1, t3);
		context.AppendInstruction(IR.AddR4, result, v1, t2);
	}
}

/// <summary>
/// AddR4Expression_v3
/// </summary>
[Transform("IR.Optimizations.Auto.ConstantMove")]
public sealed class AddR4Expression_v3 : BaseTransform
{
	public AddR4Expression_v3() : base(IR.AddR4, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IR.AddR4)
			return false;

		if (IsResolvedConstant(context.Operand2))
			return false;

		if (!IsResolvedConstant(context.Operand1.Definitions[0].Operand1))
			return false;

		if (IsResolvedConstant(context.Operand1.Definitions[0].Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand2;
		var t3 = context.Operand2;

		var v1 = transform.VirtualRegisters.AllocateR4();

		context.SetInstruction(IR.AddR4, v1, t3, t2);
		context.AppendInstruction(IR.AddR4, result, v1, t1);
	}
}
