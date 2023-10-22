// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// And64
/// </summary>
[Transform("x64.IR")]
public sealed class And64 : BaseIRTransform
{
	public And64() : base(IRInstruction.And64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.And64);
	}
}
