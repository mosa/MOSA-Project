// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.Tweak;

/// <summary>
/// MovLoad16
/// </summary>
[Transform]
public sealed class MovLoad16 : BaseTransform
{
	public MovLoad16() : base(X64.MovLoad16, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return context.Result.Register == CPURegister.RSI || context.Result.Register == CPURegister.RDI;
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(context.Result.IsPhysicalRegister);

		var source = context.Operand1;
		var offset = context.Operand2;
		var result = context.Result;

		context.SetInstruction(X64.MovLoad32, result, source, offset);
		context.AppendInstruction(X64.And32, result, result, Operand.CreateConstant32(0x0000FFFF));
	}
}
