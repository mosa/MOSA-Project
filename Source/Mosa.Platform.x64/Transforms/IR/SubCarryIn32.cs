// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Transforms.IR;

/// <summary>
/// SubCarryIn32
/// </summary>
[Transform("x64.IR")]
public sealed class SubCarryIn32 : BaseIRTransform
{
	public SubCarryIn32() : base(IRInstruction.SubCarryIn32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;
		var operand3 = context.Operand3;

		var v1 = transform.AllocateVirtualRegister32();

		context.SetInstruction(X64.Bt32, v1, operand3, transform.Constant64_0);
		context.AppendInstruction(X64.Sbb32, result, operand1, operand2);
	}
}
