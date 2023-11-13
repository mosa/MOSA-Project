// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.RuntimeCall;

/// <summary>
/// RemUnsigned32
/// </summary>
[Transform("ARM32.RuntimeCall")]
public sealed class RemUnsigned32 : BaseTransform
{
	public RemUnsigned32() : base(IR.RemUnsigned32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.ReplaceWithCall(context, "Mosa.Runtime.Math.Division", "umod32");
	}
}
