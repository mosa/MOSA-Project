// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.StrengthReduction;

/// <summary>
/// UselessOr64ShiftRight64
/// </summary>
[Transform("IR.Optimizations.Auto.StrengthReduction")]
public sealed class UselessOr64ShiftRight64 : BaseTransform
{
	public UselessOr64ShiftRight64() : base(IRInstruction.ShiftRight64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IRInstruction.Or64)
			return false;

		if (!IsConstant(context.Operand1.Definitions[0].Operand2))
			return false;

		if (!IsConstant(context.Operand2))
			return false;

		if (IsZero(context.Operand2))
			return false;

		if (!IsLessOrEqual(GetHighestSetBitPosition(To64(context.Operand1.Definitions[0].Operand2)), To64(context.Operand2)))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;
		var t2 = context.Operand2;

		context.SetInstruction(IRInstruction.ShiftRight64, result, t1, t2);
	}
}

/// <summary>
/// UselessOr64ShiftRight64_v1
/// </summary>
[Transform("IR.Optimizations.Auto.StrengthReduction")]
public sealed class UselessOr64ShiftRight64_v1 : BaseTransform
{
	public UselessOr64ShiftRight64_v1() : base(IRInstruction.ShiftRight64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IRInstruction.Or64)
			return false;

		if (!IsConstant(context.Operand1.Definitions[0].Operand1))
			return false;

		if (!IsConstant(context.Operand2))
			return false;

		if (IsZero(context.Operand2))
			return false;

		if (!IsLessOrEqual(GetHighestSetBitPosition(To64(context.Operand1.Definitions[0].Operand1)), To64(context.Operand2)))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand2;
		var t2 = context.Operand2;

		context.SetInstruction(IRInstruction.ShiftRight64, result, t1, t2);
	}
}
