// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// ConvertI32ToR8
/// </summary>
[Transform("x64.BaseIR")]
public sealed class ConvertI32ToR8 : BaseIRTransform
{
	public ConvertI32ToR8() : base(Framework.IR.ConvertI32ToR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(context.Result.IsR8);
		context.ReplaceInstruction(X64.Cvtsi2sd32);
	}
}
