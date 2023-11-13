// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// StoreParam16
/// </summary>
[Transform("ARM32.BaseIR")]
public sealed class StoreParam16 : BaseIRTransform
{
	public StoreParam16() : base(Framework.IR.StoreParam16, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		TransformStore(transform, context, ARM32.Str16, transform.StackFrame, context.Operand1, context.Operand2);
	}
}
