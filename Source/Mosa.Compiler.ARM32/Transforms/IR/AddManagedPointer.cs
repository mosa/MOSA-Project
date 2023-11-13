// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// Add32
/// </summary>
[Transform("ARM32.IR")]
public sealed class AddManagedPointer : BaseIRTransform
{
	public AddManagedPointer() : base(Framework.IR.AddManagedPointer, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(ARM32.Add);
	}
}
