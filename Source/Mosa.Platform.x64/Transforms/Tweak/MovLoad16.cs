// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.Tweak;

/// <summary>
/// MovLoad16
/// </summary>
public sealed class MovLoad16 : BaseTransform
{
	public MovLoad16() : base(X64.MovLoad16, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return context.Result.Register == CPURegister.RSI || context.Result.Register == CPURegister.RDI;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		Debug.Assert(context.Result.IsCPURegister);

		var source = context.Operand1;
		var offset = context.Operand2;
		var result = context.Result;

		context.SetInstruction(X64.MovLoad32, result, source, offset);
		context.AppendInstruction(X64.And32, result, result, transform.CreateConstant32(0x0000FFFF));
	}
}
