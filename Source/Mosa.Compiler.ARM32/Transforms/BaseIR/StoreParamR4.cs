// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// StoreParamR4
/// </summary>
[Transform("ARM32.BaseIR")]
public sealed class StoreParamR4 : BaseIRTransform
{
	public StoreParamR4() : base(Framework.IR.StoreParamR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		TransformStore(transform, context, ARM32.Stf, transform.StackFrame, context.Operand1, context.Operand2);
	}
}
