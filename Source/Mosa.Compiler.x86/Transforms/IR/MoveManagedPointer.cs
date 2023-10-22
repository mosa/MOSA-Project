// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.IR;

/// <summary>
/// MoveManagedPointer
/// </summary>
[Transform("x86.IR")]
public sealed class MoveManagedPointer : BaseIRTransform
{
	public MoveManagedPointer() : base(IRInstruction.MoveManagedPointer, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X86.Mov32);
	}
}
