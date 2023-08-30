// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// StoreParamR8
/// </summary>
[Transform("ARM32.IR")]
public sealed class StoreParamR8 : BaseIRTransform
{
	public StoreParamR8() : base(IRInstruction.StoreParamR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		TransformStore(transform, context, ARM32.Stf, transform.StackFrame, context.Operand1, context.Operand2);
	}
}
