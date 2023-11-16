// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// StoreParam8
/// </summary>
[Transform]
public sealed class StoreParam8 : BaseIRTransform
{
	public StoreParam8() : base(IR.StoreParam8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(X64.MovStore8, null, transform.StackFrame, context.Operand1, context.Operand2);
	}
}
