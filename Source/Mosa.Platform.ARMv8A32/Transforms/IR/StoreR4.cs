// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// StoreR4
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class StoreR4 : BaseIRTransform
{
	public StoreR4() : base(IRInstruction.StoreR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		TransformStore(transform, context, ARMv8A32.Stf, context.Operand1, context.Operand2, context.Operand3);
	}
}
