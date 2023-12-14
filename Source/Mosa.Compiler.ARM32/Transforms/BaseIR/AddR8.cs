// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// AddR8
/// </summary>
public sealed class AddR8 : BaseIRTransform
{
	public AddR8() : base(IR.AddR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Framework.Transform.MoveConstantRight(context);

		Translate(transform, context, ARM32.VAdd, false);
	}
}
