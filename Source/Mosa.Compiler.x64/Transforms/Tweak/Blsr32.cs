// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.Tweak;

/// <summary>
/// Blsr32
/// </summary>
[Transform]
public sealed class Blsr32 : BaseTransform
{
	public Blsr32() : base(X64.Blsr32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.MoveOperand1ToVirtualRegister(context, X64.Mov32);
	}
}
