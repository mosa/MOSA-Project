// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.Simplification;

/// <summary>
/// Move64Coalescing
/// </summary>
[Transform("IR.Optimizations.Auto.Simplification")]
public sealed class Move64Coalescing : BaseTransform
{
	public Move64Coalescing() : base(IR.Move64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 25;

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IR.Move64)
			return false;

		if (IsCPURegister(context.Operand1.Definitions[0].Operand1))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;

		context.SetInstruction(IR.Move64, result, t1);
	}
}
