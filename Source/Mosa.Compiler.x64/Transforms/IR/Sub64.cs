// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// Sub64
/// </summary>
[Transform("x64.IR")]
public sealed class Sub64 : BaseIRTransform
{
	public Sub64() : base(Framework.IR.Sub64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.Sub64);
	}
}
