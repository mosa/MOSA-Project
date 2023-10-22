// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// Or64
/// </summary>
[Transform("x64.IR")]
public sealed class Or64 : BaseIRTransform
{
	public Or64() : base(IRInstruction.Or64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.Or64);
	}
}
