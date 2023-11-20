// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.Tweak;

/// <summary>
/// Movzx16To32
/// </summary>
public sealed class Movzx16To32 : BaseTransform
{
	public Movzx16To32() : base(X86.Movzx16To32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsPhysicalRegister)
			return false;

		return !(context.Operand1.Register != CPURegister.ESI && context.Operand1.Register != CPURegister.EDI);
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(context.Result.IsPhysicalRegister);

		var result = context.Result;
		var source = context.Operand1;

		// Movzx8To32 can not use with ESI or EDI registers as source registers
		if (source.Register == result.Register)
		{
			context.SetInstruction(X86.And32, result, result, Operand.CreateConstant32(0xFFFF));
		}
		else
		{
			context.SetInstruction(X86.Mov32, result, source);
			context.AppendInstruction(X86.And32, result, result, Operand.CreateConstant32(0xFFFF));
		}
	}
}
