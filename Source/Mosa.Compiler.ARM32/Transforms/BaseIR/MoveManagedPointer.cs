// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// MoveManagedPointer
/// </summary>
[Transform("ARM32.BaseIR")]
public sealed class MoveManagedPointer : BaseIRTransform
{
	public MoveManagedPointer() : base(IR.MoveManagedPointer, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(ARM32.Mov);
	}
}
