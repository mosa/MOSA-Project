// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.Useless;

/// <summary>
/// ZeroExtend16x64ZeroExtend8x64
/// </summary>
[Transform("IR.Optimizations.Auto.Useless")]
public sealed class ZeroExtend16x64ZeroExtend8x64 : BaseTransform
{
	public ZeroExtend16x64ZeroExtend8x64() : base(Framework.IR.ZeroExtend16x64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 85;

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != Framework.IR.ZeroExtend8x64)
			return false;

		if (IsConstant(context.Operand1.Definitions[0].Operand1))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;

		context.SetInstruction(Framework.IR.ZeroExtend8x64, result, t1);
	}
}
