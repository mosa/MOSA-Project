// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// StoreParam8
/// </summary>
public sealed class StoreParam8 : BaseIRTransform
{
	public StoreParam8() : base(IR.StoreParam8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		TransformStore(transform, context, ARM32.Str8, transform.StackFrame, context.Operand1, context.Operand2);
	}
}
