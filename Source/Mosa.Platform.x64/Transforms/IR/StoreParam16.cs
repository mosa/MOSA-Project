// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Transforms.IR;

/// <summary>
/// StoreParam16
/// </summary>
[Transform("x64.IR")]
public sealed class StoreParam16 : BaseIRTransform
{
	public StoreParam16() : base(IRInstruction.StoreParam16, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.SetInstruction(X64.MovStore16, null, transform.StackFrame, context.Operand1, context.Operand2);
	}
}
