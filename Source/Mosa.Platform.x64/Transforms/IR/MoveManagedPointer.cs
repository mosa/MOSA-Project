// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Transforms.IR;

/// <summary>
/// MoveManagedPointer
/// </summary>
[Transform("x64.IR")]
public sealed class MoveManagedPointer : BaseIRTransform
{
	public MoveManagedPointer() : base(IRInstruction.MoveManagedPointer, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.ReplaceInstruction(X64.Mov32);
	}
}
