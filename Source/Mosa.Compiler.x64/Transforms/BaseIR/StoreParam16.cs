// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// StoreParam16
/// </summary>
[Transform("x64.BaseIR")]
public sealed class StoreParam16 : BaseIRTransform
{
	public StoreParam16() : base(IR.StoreParam16, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(X64.MovStore16, null, transform.StackFrame, context.Operand1, context.Operand2);
	}
}
