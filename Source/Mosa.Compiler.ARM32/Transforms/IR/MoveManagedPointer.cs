// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// MoveManagedPointer
/// </summary>
[Transform("ARM32.IR")]
public sealed class MoveManagedPointer : BaseIRTransform
{
	public MoveManagedPointer() : base(IRInstruction.MoveManagedPointer, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(ARM32.Mov);
	}
}
