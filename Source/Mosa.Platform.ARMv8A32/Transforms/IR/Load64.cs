// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// Load64
/// </summary>
public sealed class Load64 : BaseIRTransform
{
	public Load64() : base(IRInstruction.Load64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.SplitLongOperand(context.Result, out var resultLow, out var resultHigh);

		var address = context.Operand1;
		var offset = context.Operand2;

		address = MoveConstantToRegister(transform, context, address);
		offset = MoveConstantToRegisterOrImmediate(transform, context, offset);

		var v1 = transform.AllocateVirtualRegister32();
		var v2 = transform.AllocateVirtualRegister32();

		context.SetInstruction(ARMv8A32.Add, v1, address, offset);
		context.AppendInstruction(ARMv8A32.Ldr32, resultLow, v1, transform.Constant32_0);
		context.AppendInstruction(ARMv8A32.Add, v2, v1, transform.Constant32_4);
		context.AppendInstruction(ARMv8A32.Ldr32, resultHigh, v2, transform.Constant32_0);
	}
}
