// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.Optimizations.Auto.Standard;

/// <summary>
/// Mov32Coalescing
/// </summary>
[Transform("x64.Optimizations.Auto.Standard")]
public sealed class Mov32Coalescing : BaseTransform
{
	public Mov32Coalescing() : base(X64.Mov32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != X64.Mov32)
			return false;

		if (!IsVirtualRegister(context.Operand1.Definitions[0].Operand1))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var t1 = context.Operand1.Definitions[0].Operand1;

		context.SetInstruction(X64.Mov32, result, t1);
	}
}
