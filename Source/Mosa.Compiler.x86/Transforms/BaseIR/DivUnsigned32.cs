// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// DivUnsigned32
/// </summary>
[Transform]
public sealed class DivUnsigned32 : BaseIRTransform
{
	public DivUnsigned32() : base(IR.DivUnsigned32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		var v1 = transform.VirtualRegisters.Allocate32();
		var v2 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(X86.Mov32, v1, Operand.Constant32_0);
		context.AppendInstruction2(X86.Div32, result, v2, operand1, v1, operand2);
	}
}
