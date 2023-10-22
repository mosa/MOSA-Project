// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// Store8
/// </summary>
[Transform("ARM32.IR")]
public sealed class Store8 : BaseIRTransform
{
	public Store8() : base(IRInstruction.Store8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		TransformStore(transform, context, ARM32.Str8, context.Operand1, context.Operand2, context.Operand3);
	}
}
