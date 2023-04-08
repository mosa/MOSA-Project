// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// Store8
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class Store8 : BaseIRTransform
{
	public Store8() : base(IRInstruction.Store8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		TransformStore(transform, context, ARMv8A32.Str8, context.Operand1, context.Operand2, context.Operand3);
	}
}
