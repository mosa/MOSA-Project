// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// StoreParamObject
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class StoreParamObject : BaseIRTransform
{
	public StoreParamObject() : base(IRInstruction.StoreParamObject, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.SetInstruction(ARMv8A32.Mov, null, transform.StackFrame, context.Operand1, context.Operand2);
	}
}
