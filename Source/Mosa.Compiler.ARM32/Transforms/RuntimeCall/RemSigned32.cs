// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.RuntimeCall;

/// <summary>
/// RemSigned32
/// </summary>
[Transform("ARM32.RuntimeCall")]
public sealed class RemSigned32 : BaseTransform
{
	public RemSigned32() : base(Framework.IR.RemSigned32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.ReplaceWithCall(context, "Mosa.Runtime.Math.Division", "smod32");
	}
}
