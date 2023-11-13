// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// Add32
/// </summary>
[Transform("ARM32.IR")]
public sealed class Add32 : BaseIRTransform
{
	public Add32() : base(Framework.IR.Add32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Framework.Transform.MoveConstantRight(context);

		Translate(transform, context, ARM32.Add, true);
	}
}
