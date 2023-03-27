// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.Optimizations.Manual.Special;

/// <summary>
/// Bt32Movzx8To32Setcc
/// </summary>
[Transform("x86.Optimizations.Manual.Special")]
public sealed class Bt32Movzx8To32Setcc : BaseTransform
{
	public Bt32Movzx8To32Setcc() : base(X86.Bt32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsResolvedConstant)
			return false;

		if (context.Operand2.ConstantUnsigned64 != 0)
			return false;

		if (context.Operand1.Definitions.Count != 1)
			return false;

		if (context.Operand1.Definitions[0].Instruction != X86.Movzx8To32)
			return false;

		if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
			return false;

		if (context.Operand1.Definitions[0].Operand1.Definitions.Count != 1)
			return false;

		if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != X86.Setcc)
			return false;

		var next = context.Node.PreviousNonEmpty;

		if (next == null || next.Instruction != X86.Movzx8To32)
			return false;

		next = next.PreviousNonEmpty;

		if (next == null || next.Instruction != X86.Setcc)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var next = context.Node.PreviousNonEmpty;

		next.SetNop();

		next = next.PreviousNonEmpty;

		next.SetNop();

		context.SetNop();
	}
}
