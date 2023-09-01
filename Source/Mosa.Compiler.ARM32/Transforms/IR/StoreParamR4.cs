// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// StoreParamR4
/// </summary>
[Transform("ARM32.IR")]
public sealed class StoreParamR4 : BaseIRTransform
{
	public StoreParamR4() : base(IRInstruction.StoreParamR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		TransformStore(transform, context, ARM32.Stf, transform.StackFrame, context.Operand1, context.Operand2);
	}
}
