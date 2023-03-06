// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// AddCarryIn32
/// </summary>
public sealed class AddCarryIn32 : BaseIRTransform
{
	public AddCarryIn32() : base(IRInstruction.AddCarryIn32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;
		var operand3 = context.Operand3;

		operand1 = MoveConstantToRegister(transform, context, operand1);
		operand2 = MoveConstantToRegisterOrImmediate(transform, context, operand2);
		operand3 = MoveConstantToRegisterOrImmediate(transform, context, operand3);

		var v1 = transform.AllocateVirtualRegister32();

		context.SetInstruction(ARMv8A32.Add, v1, operand1, operand2);
		context.AppendInstruction(ARMv8A32.Add, result, v1, operand3);
	}
}
