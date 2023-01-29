// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// Store16
/// </summary>
public sealed class Store16 : BaseTransform
{
	public Store16() : base(IRInstruction.Store16, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		ARMv8A32TransformHelper.TransformStore(transform, context, ARMv8A32.Str16, context.Operand1, context.Operand2, context.Operand3);
	}
}
