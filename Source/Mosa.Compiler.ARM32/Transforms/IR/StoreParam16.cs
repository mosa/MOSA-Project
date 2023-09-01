// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// StoreParam16
/// </summary>
[Transform("ARM32.IR")]
public sealed class StoreParam16 : BaseIRTransform
{
	public StoreParam16() : base(IRInstruction.StoreParam16, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		TransformStore(transform, context, ARM32.Str16, transform.StackFrame, context.Operand1, context.Operand2);
	}
}
