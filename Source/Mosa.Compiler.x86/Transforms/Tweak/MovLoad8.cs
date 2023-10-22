// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.Tweak;

/// <summary>
/// MovLoad8
/// </summary>
[Transform("x86.Tweak")]
public sealed class MovLoad8 : BaseTransform
{
	public MovLoad8() : base(X86.MovLoad8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Result.IsCPURegister)
			return false;

		return context.Result.Register == CPURegister.ESI || context.Result.Register == CPURegister.EDI;
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(context.Result.IsCPURegister);

		var result = context.Result;

		var source = context.Operand1;
		var offset = context.Operand2;

		context.SetInstruction(X86.MovLoad32, result, source, offset);
		context.AppendInstruction(X86.And32, result, result, Operand.CreateConstant32(0x000000FF));
	}
}
