// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// StoreParamR8
/// </summary>
[Transform("ARM32.BaseIR")]
public sealed class StoreParamR8 : BaseIRTransform
{
	public StoreParamR8() : base(IR.StoreParamR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		TransformStore(transform, context, ARM32.Stf, transform.StackFrame, context.Operand1, context.Operand2);
	}
}
