// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.RuntimeCall;

/// <summary>
/// RemR8
/// </summary>
[Transform("x64.RuntimeCall")]
public sealed class RemR8 : BaseTransform
{
	public RemR8() : base(IR.RemR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(context.Result.IsR8);
		Debug.Assert(context.Operand1.IsR8);

		transform.ReplaceWithCall(context, "Mosa.Runtime.Math.x64.Division", "RemR8");
	}
}
