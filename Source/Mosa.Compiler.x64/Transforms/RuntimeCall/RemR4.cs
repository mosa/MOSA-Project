// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.RuntimeCall;

/// <summary>
/// RemR4
/// </summary>
[Transform]
public sealed class RemR4 : BaseTransform
{
	public RemR4() : base(IR.RemR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(context.Result.IsR4);
		Debug.Assert(context.Operand1.IsR4);

		transform.ReplaceWithCall(context, "Mosa.Runtime.Math.x64.Division", "RemR4");
	}
}
