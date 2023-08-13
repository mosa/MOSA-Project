// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARM32.Transforms.IR;

/// <summary>
/// Add32
/// </summary>
[Transform("ARM32.IR")]
public sealed class AddManagedPointer : BaseIRTransform
{
	public AddManagedPointer() : base(IRInstruction.AddManagedPointer, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.ReplaceInstruction(ARM32.Add);
	}
}
