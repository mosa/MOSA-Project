// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.Optimizations.Auto.Consolidation;

public sealed class IMul32Mov32ByOne : BaseTransform
{
	public IMul32Mov32ByOne() : base(X64.IMul32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != X64.Mov32)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsConstantOne)
			return false;

		if (AreAnyStatusFlagsUsed(context, 10))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;

		context.SetInstruction(X64.Mov32, result, t1);
	}
}
