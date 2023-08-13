// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARM32.Transforms.IR;

/// <summary>
/// Add32
/// </summary>
[Transform("ARM32.IR")]
public sealed class Add32 : BaseIRTransform
{
	public Add32() : base(IRInstruction.Add32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		TransformContext.MoveConstantRight(context);

		Translate(transform, context, ARM32.Add, true);
	}
}
