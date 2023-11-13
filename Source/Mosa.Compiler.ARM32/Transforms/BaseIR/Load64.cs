// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// Load64
/// </summary>
[Transform("ARM32.BaseIR")]
public sealed class Load64 : BaseIRTransform
{
	public Load64() : base(IR.Load64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);

		var address = context.Operand1;
		var offset = context.Operand2;

		address = MoveConstantToRegister(transform, context, address);
		offset = MoveConstantToRegisterOrImmediate(transform, context, offset);

		var v1 = transform.VirtualRegisters.Allocate32();
		var v2 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(ARM32.Add, v1, address, offset);
		context.AppendInstruction(ARM32.Ldr32, resultLow, v1, Operand.Constant32_0);
		context.AppendInstruction(ARM32.Add, v2, v1, Operand.Constant32_4);
		context.AppendInstruction(ARM32.Ldr32, resultHigh, v2, Operand.Constant32_0);
	}
}
