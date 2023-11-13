// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// MulHs64
/// </summary>
[Transform("x64.BaseIR")]
public sealed class MulHs64 : BaseIRTransform
{
	public MulHs64() : base(IR.MulHu64, TransformType.Manual | TransformType.Transform, true)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		var v1 = transform.VirtualRegisters.Allocate64();

		context.SetInstruction2(X64.IMul32, result, v1, operand1, operand2);
	}
}
