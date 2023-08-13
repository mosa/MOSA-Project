// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARM32.Transforms.RuntimeCall;

/// <summary>
/// RemUnsigned64
/// </summary>
[Transform("ARM32.RuntimeCall")]
public sealed class RemUnsigned64 : BaseTransform
{
	public RemUnsigned64() : base(IRInstruction.RemUnsigned64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.ReplaceWithCall(context, "Mosa.Runtime.Math.Division", "umod64");
	}
}
