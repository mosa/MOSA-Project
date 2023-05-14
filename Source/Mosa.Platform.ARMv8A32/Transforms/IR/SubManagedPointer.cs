// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// Sub32
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class SubManagedPointer : BaseIRTransform
{
	public SubManagedPointer() : base(IRInstruction.SubManagedPointer, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.ReplaceInstruction(ARMv8A32.Sub);
	}
}
