// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// StoreParam32
/// </summary>
[Transform("ARM32.IR")]
public sealed class StoreParam32 : BaseIRTransform
{
	public StoreParam32() : base(IRInstruction.StoreParam32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		TransformStore(transform, context, ARM32.Str32, transform.StackFrame, context.Operand1, context.Operand2);
	}
}
