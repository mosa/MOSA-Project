// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// Add32
/// </summary>
[Transform("x86.BaseIR")]
public sealed class AddManagedPointer : BaseIRTransform
{
	public AddManagedPointer() : base(IR.AddManagedPointer, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X86.Add32);
	}
}
