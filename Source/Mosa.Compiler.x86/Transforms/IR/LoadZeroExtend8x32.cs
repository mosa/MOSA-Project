// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.IR;

/// <summary>
/// LoadZeroExtend8x32
/// </summary>
[Transform("x86.IR")]
public sealed class LoadZeroExtend8x32 : BaseIRTransform
{
	public LoadZeroExtend8x32() : base(IRInstruction.LoadZeroExtend8x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.OrderLoadStoreOperands(context);

		context.SetInstruction(X86.MovzxLoad8, context.Result, context.Operand1, context.Operand2);
	}
}
