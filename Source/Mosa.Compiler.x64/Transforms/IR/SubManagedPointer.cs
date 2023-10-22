// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// Sub32
/// </summary>
[Transform("x64.IR")]
public sealed class SubManagedPointer : BaseIRTransform
{
	public SubManagedPointer() : base(IRInstruction.SubManagedPointer, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.Sub32);
	}
}
