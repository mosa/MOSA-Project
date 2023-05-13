// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Transforms.IR;

/// <summary>
/// Add32
/// </summary>
[Transform("x64.IR")]
public sealed class AddManagedPointer : BaseIRTransform
{
	public AddManagedPointer() : base(IRInstruction.AddManagedPointer, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.ReplaceInstruction(X64.Add32);
	}
}
