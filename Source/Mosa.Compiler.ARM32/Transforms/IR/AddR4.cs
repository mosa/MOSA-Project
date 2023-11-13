// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// AddR4
/// </summary>
[Transform("ARM32.IR")]
public sealed class AddR4 : BaseIRTransform
{
	public AddR4() : base(Framework.IR.AddR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Framework.Transform.MoveConstantRight(context);

		Translate(transform, context, ARM32.Adf, true);
	}
}
