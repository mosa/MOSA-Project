// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// AddCarryIn64
/// </summary>
[Transform("x64.BaseIR")]
public sealed class AddCarryIn64 : BaseIRTransform
{
	public AddCarryIn64() : base(Framework.IR.AddCarryIn64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;
		var operand3 = context.Operand3;

		var v1 = transform.VirtualRegisters.Allocate64();

		context.SetInstruction(X64.Bt32, v1, operand3, Operand.Constant64_0);
		context.AppendInstruction(X64.Adc64, result, operand1, operand2);
	}
}
