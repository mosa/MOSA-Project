// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// MoveManagedPointer
/// </summary>
public sealed class MoveManagedPointer : BaseIRTransform
{
	public MoveManagedPointer() : base(IR.MoveManagedPointer, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X86.Mov32);
	}
}
