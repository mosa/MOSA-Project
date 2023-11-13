// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// LoadZeroExtend8x32
/// </summary>
[Transform("x64.BaseIR")]
public sealed class LoadZeroExtend8x32 : BaseIRTransform
{
	public LoadZeroExtend8x32() : base(IR.LoadZeroExtend8x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.OrderLoadStoreOperands(context);

		context.SetInstruction(X64.MovzxLoad8, context.Result, context.Operand1, context.Operand2);
	}
}
