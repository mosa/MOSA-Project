// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.Tweak;

/// <summary>
/// Blsr64
/// </summary>
[Transform]
public sealed class Blsr64 : BaseTransform
{
	public Blsr64() : base(X64.Blsr64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.MoveOperand1ToVirtualRegister(context, X64.Mov64);
	}
}
