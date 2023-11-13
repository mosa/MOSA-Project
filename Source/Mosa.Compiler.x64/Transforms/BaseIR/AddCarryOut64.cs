// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// AddCarryOut32
/// </summary>
[Transform("x64.BaseIR")]
public sealed class AddCarryOut64 : BaseIRTransform
{
	public AddCarryOut64() : base(IR.AddCarryOut64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var result2 = context.Result2;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		var v1 = transform.VirtualRegisters.Allocate64();

		context.SetInstruction(X64.Add64, result, operand1, operand2);
		context.AppendInstruction(X64.Setcc, ConditionCode.Carry, v1);
		context.AppendInstruction(X64.Movzx8To64, result2, v1);
	}
}
