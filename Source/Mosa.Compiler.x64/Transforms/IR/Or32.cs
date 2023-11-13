// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// Or32
/// </summary>
[Transform("x64.IR")]
public sealed class Or32 : BaseIRTransform
{
	public Or32() : base(Framework.IR.Or32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.Or32);
	}
}
