// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.Tweak;

/// <summary>
/// Movzx8To32
/// </summary>
[Transform("x86.Tweak")]
public sealed class Movzx8To32 : BaseTransform
{
	public Movzx8To32() : base(X86.Movzx8To32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand1.IsCPURegister)
			return false;

		return !(context.Operand1.Register != CPURegister.ESI && context.Operand1.Register != CPURegister.EDI);
	}

	public override void Transform(Context context, TransformContext transform)
	{
		Debug.Assert(context.Result.IsCPURegister);

		var result = context.Result;
		var source = context.Operand1;

		// Movzx8To32 can not use with ESI or EDI registers as source registers
		if (source.Register == result.Register)
		{
			context.SetInstruction(X86.And32, result, result, Operand.CreateConstant32(0xFF));
		}
		else
		{
			context.SetInstruction(X86.Mov32, result, source);
			context.AppendInstruction(X86.And32, result, result, Operand.CreateConstant32(0xFF));
		}
	}
}
