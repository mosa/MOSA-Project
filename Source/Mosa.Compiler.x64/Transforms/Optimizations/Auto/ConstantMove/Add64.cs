// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.Optimizations.Auto.ConstantMove;

public sealed class Add64 : BaseTransform
{
	public Add64() : base(X64.Add64, TransformType.Auto | TransformType.Optimization, 70)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != X64.Mov64)
			return false;

		if (!IsConstant(context.Operand2.Definitions[0].Operand1))
			return false;

		if (AreAnyStatusFlagsUsed(context, transform.Window))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;
		var t2 = context.Operand2.Definitions[0].Operand1;

		context.SetInstruction(X64.Add64, result, t1, t2);
	}
}
