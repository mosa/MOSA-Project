// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// StoreParam8
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class StoreParam8 : BaseIRTransform
{
	public StoreParam8() : base(IRInstruction.StoreParam8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		TransformStore(transform, context, ARMv8A32.Str8, transform.StackFrame, context.Operand1, context.Operand2);
	}
}
