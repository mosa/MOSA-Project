// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.Optimizations.Manual.StrengthReduction;

[Transform("x86.Optimizations.Manual.StrengthReduction")]
public sealed class Mul32ByZero_v1 : BaseTransform
{
	public Mul32ByZero_v1() : base(X86.Mul32, TransformType.Manual | TransformType.Optimization, true)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsDefinedOnce)
			return false;

		if (context.Operand2.Definitions[0].Instruction != X86.Mov32)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsResolvedConstant)
			return false;

		if (!context.Operand2.Definitions[0].Operand1.IsConstantZero)
			return false;

		if (!(AreStatusFlagsUsed(context.Instruction, context.Node.Next) == TriState.No))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var result2 = context.Result2;

		context.SetInstruction(X86.Mov32, result, Operand.Constant32_0);
		context.AppendInstruction(X86.Mov32, result2, Operand.Constant32_0);
	}
}
