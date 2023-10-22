// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// MulUnsigned32
/// </summary>
[Transform("ARM32.IR")]
public sealed class MulUnsigned32 : BaseIRTransform
{
	public MulUnsigned32() : base(IRInstruction.MulUnsigned32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Framework.Transform.MoveConstantRight(context);

		Translate(transform, context, ARM32.Mul, false);
	}
}
