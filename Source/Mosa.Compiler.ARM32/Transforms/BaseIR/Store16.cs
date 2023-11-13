// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// Store16
/// </summary>
[Transform("ARM32.BaseIR")]
public sealed class Store16 : BaseIRTransform
{
	public Store16() : base(IR.Store16, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		TransformStore(transform, context, ARM32.Str16, context.Operand1, context.Operand2, context.Operand3);
	}
}
