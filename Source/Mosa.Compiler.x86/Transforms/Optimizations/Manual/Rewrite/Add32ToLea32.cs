// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.Optimizations.Manual.Rewrite;
// This transformation can reduce restrictions placed on the register allocator.
// The LEA does not change any of the status flags, however, the add instruction does modify some flags (carry, zero, etc.)
// Therefore, this transformation can only occur if the status flags are unused later.
// A search is required to determine if a status flag is used.
// However, if the search is not conclusive, the transformation is not made.

public sealed class Add32ToLea32 : BaseTransform
{
	public Add32ToLea32() : base(X86.Add32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (context.Operand2.IsPhysicalRegister)
			return false;

		if (context.Operand1.Register == CPURegister.ESP)
			return false;

		if (context.Operand2.IsResolvedConstant && context.Operand2.ConstantUnsigned64 == 1 && context.Operand1 == context.Result)
			return false;

		if (AreAnyStatusFlagsUsed(context))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		if (operand2.IsConstant)
		{
			context.SetInstruction(X86.Lea32, result, operand1, Operand.Constant32_0, Operand.Constant32_1, operand2);
		}
		else
		{
			context.SetInstruction(X86.Lea32, result, operand1, operand2, Operand.Constant32_1, Operand.Constant32_0);
		}
	}
}
