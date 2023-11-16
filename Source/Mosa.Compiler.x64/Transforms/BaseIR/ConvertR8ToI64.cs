// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// ConvertR8ToI64
/// </summary>
[Transform]
public sealed class ConvertR8ToI64 : BaseIRTransform
{
	public ConvertR8ToI64() : base(IR.ConvertR8ToI64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(context.Result.IsInteger && !context.Result.IsFloatingPoint);
		context.ReplaceInstruction(X64.Cvttss2si64);
	}
}
