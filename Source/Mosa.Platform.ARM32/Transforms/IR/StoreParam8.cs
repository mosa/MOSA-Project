// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARM32.Transforms.IR;

/// <summary>
/// StoreParam8
/// </summary>
[Transform("ARM32.IR")]
public sealed class StoreParam8 : BaseIRTransform
{
	public StoreParam8() : base(IRInstruction.StoreParam8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		TransformStore(transform, context, ARM32.Str8, transform.StackFrame, context.Operand1, context.Operand2);
	}
}
