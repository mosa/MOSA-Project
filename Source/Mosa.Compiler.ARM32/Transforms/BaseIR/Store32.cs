// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// Store32
/// </summary>
public sealed class Store32 : BaseIRTransform
{
	public Store32() : base(IR.Store32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		TransformStore(transform, context, ARM32.Str32, context.Operand1, context.Operand2, context.Operand3);
	}
}
