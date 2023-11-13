// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.IR;

/// <summary>
/// StoreParam8
/// </summary>
[Transform("x86.IR")]
public sealed class StoreParam8 : BaseIRTransform
{
	public StoreParam8() : base(Framework.IR.StoreParam8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(X86.MovStore8, null, transform.StackFrame, context.Operand1, context.Operand2);
	}
}
