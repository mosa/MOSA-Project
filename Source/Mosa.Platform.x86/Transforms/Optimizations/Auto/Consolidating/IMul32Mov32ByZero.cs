// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.Optimizations.Auto.Consolidating;

/// <summary>
/// IMul32Mov32ByZero
/// </summary>
[Transform("x86.Optimizations.Auto.Consolidating")]
public sealed class IMul32Mov32ByZero : BaseTransform
{
	public IMul32Mov32ByZero() : base(X86.IMul32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (context.Operand2.Definitions.Count != 1)
			return false;

		if (context.Operand2.Definitions[0].Instruction != X86.Mov32)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsResolvedConstant)
			return false;

		if (context.Operand2.Definitions[0].Operand1.ConstantUnsigned64 != 0)
			return false;

		if (AreStatusFlagUsed(context))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var c1 = Operand.CreateConstant(0);

		context.SetInstruction(X86.Mov32, result, c1);
	}
}
