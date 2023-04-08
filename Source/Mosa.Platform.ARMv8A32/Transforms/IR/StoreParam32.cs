// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// StoreParam32
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class StoreParam32 : BaseIRTransform
{
	public StoreParam32() : base(IRInstruction.StoreParam32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		TransformStore(transform, context, ARMv8A32.Str32, transform.StackFrame, context.Operand1, context.Operand2);
	}
}
