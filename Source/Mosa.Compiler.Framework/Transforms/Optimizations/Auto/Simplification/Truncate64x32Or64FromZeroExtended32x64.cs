// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.Simplification;

/// <summary>
/// Truncate64x32Or64FromZeroExtended32x64
/// </summary>
[Transform("IR.Optimizations.Auto.Simplification")]
public sealed class Truncate64x32Or64FromZeroExtended32x64 : BaseTransform
{
	public Truncate64x32Or64FromZeroExtended32x64() : base(Framework.IR.Truncate64x32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != Framework.IR.Or64)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != Framework.IR.ZeroExtend32x64)
			return false;

		if (!context.Operand1.Definitions[0].Operand2.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Operand2.Definitions[0].Instruction != Framework.IR.ZeroExtend32x64)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1;
		var t2 = context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1;

		context.SetInstruction(Framework.IR.Or32, result, t1, t2);
	}
}
