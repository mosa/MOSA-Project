// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// Sub32
/// </summary>
[Transform("x64.IR")]
public sealed class Sub32 : BaseIRTransform
{
	public Sub32() : base(IRInstruction.Sub32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.ReplaceInstruction(X64.Sub32);
	}
}
