// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// StoreR8
/// </summary>
public sealed class StoreR8 : BaseIRTransform
{
	public StoreR8() : base(IRInstruction.StoreR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		TransformStore(transform, context, ARMv8A32.Stf, context.Operand1, context.Operand2, context.Operand3);
	}
}
