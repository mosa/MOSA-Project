// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// StoreR8
/// </summary>
public sealed class StoreR8 : BaseIRTransform
{
	public static readonly StoreR8 Instance = new();

	private StoreR8() : base(IR.StoreR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		TransformFloatingPointStore(transform, context, ARM32.VStr, context.Operand1, context.Operand2, context.Operand3);
	}
}
