// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.RuntimeCall;

/// <summary>
/// RemR8
/// </summary>
[Transform("x86.RuntimeCall")]
public sealed class RemR8 : BaseTransform
{
	public RemR8() : base(IRInstruction.RemR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -100;

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		Debug.Assert(context.Result.IsR8);
		Debug.Assert(context.Operand1.IsR8);

		transform.ReplaceWithCall(context, "Mosa.Runtime.Math.x86.Division", "RemR8");
	}
}
