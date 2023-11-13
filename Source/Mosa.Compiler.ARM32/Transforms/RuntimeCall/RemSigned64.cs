// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.RuntimeCall;

/// <summary>
/// RemSigned64
/// </summary>
[Transform("ARM32.RuntimeCall")]
public sealed class RemSigned64 : BaseTransform
{
	public RemSigned64() : base(Framework.IR.RemSigned64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.ReplaceWithCall(context, "Mosa.Runtime.Math.Division", "smod64");
	}
}
