// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding;

/// <summary>
/// MulSignedShiftLeft32
/// </summary>
[Transform("IR.Optimizations.Auto.ConstantFolding")]
public sealed class MulSignedShiftLeft32 : BaseTransform
{
	public MulSignedShiftLeft32() : base(IRInstruction.MulSigned32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (context.Operand1.Definitions.Count != 1)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IRInstruction.ShiftLeft32)
			return false;

		if (IsResolvedConstant(context.Operand1.Definitions[0].Operand1))
			return false;

		if (IsResolvedConstant(context.Operand2))
			return false;

		if (!IsResolvedConstant(context.Operand1.Definitions[0].Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand2;
		var t3 = context.Operand2;

		var v1 = transform.AllocateVirtualRegister32();

		context.SetInstruction(IRInstruction.MulSigned32, v1, t1, t3);
		context.AppendInstruction(IRInstruction.ShiftLeft32, result, v1, t2);
	}
}

/// <summary>
/// MulSignedShiftLeft32_v1
/// </summary>
[Transform("IR.Optimizations.Auto.ConstantFolding")]
public sealed class MulSignedShiftLeft32_v1 : BaseTransform
{
	public MulSignedShiftLeft32_v1() : base(IRInstruction.MulSigned32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (context.Operand2.Definitions.Count != 1)
			return false;

		if (context.Operand2.Definitions[0].Instruction != IRInstruction.ShiftLeft32)
			return false;

		if (IsResolvedConstant(context.Operand2.Definitions[0].Operand1))
			return false;

		if (IsResolvedConstant(context.Operand1))
			return false;

		if (!IsResolvedConstant(context.Operand2.Definitions[0].Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;
		var t2 = context.Operand2.Definitions[0].Operand1;
		var t3 = context.Operand2.Definitions[0].Operand2;

		var v1 = transform.AllocateVirtualRegister32();

		context.SetInstruction(IRInstruction.MulSigned32, v1, t2, t1);
		context.AppendInstruction(IRInstruction.ShiftLeft32, result, v1, t3);
	}
}
