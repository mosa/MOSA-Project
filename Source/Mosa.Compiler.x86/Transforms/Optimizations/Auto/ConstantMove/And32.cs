// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.Optimizations.Auto.ConstantMove;

/// <summary>
/// And32
/// </summary>
[Transform("x86.Optimizations.Auto.ConstantMove")]
public sealed class And32 : BaseTransform
{
	public And32() : base(X86.And32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != X86.Mov32)
			return false;

		if (!IsConstant(context.Operand2.Definitions[0].Operand1))
			return false;

		if (AreStatusFlagUsed(context))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;
		var t2 = context.Operand2.Definitions[0].Operand1;

		context.SetInstruction(X86.And32, result, t1, t2);
	}
}
