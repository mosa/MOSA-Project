// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// StoreObject
/// </summary>
public sealed class StoreObject : BaseIRTransform
{
	public StoreObject() : base(IR.StoreObject, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		TransformStore(transform, context, ARM32.Str32, context.Operand1, context.Operand2, context.Operand3);
	}
}
