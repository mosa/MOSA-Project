// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// Store64
/// </summary>
[Transform("x86.BaseIR")]
public sealed class Store64 : BaseIRTransform
{
	public Store64() : base(IR.Store64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.SplitOperand(context.Operand2, out var op2L, out _);
		transform.SplitOperand(context.Operand3, out var op3L, out var op3H);

		var address = context.Operand1;
		var offset = context.Operand2;

		context.SetInstruction(X86.MovStore32, null, address, op2L, op3L);

		if (offset.IsResolvedConstant)
		{
			context.AppendInstruction(X86.MovStore32, null, address, Operand.CreateConstant32(offset.Offset + transform.NativePointerSize), op3H);
		}
		else
		{
			var offset4 = transform.VirtualRegisters.Allocate32();

			context.AppendInstruction(X86.Add32, offset4, op2L, Operand.Constant32_4);
			context.AppendInstruction(X86.MovStore32, null, address, offset4, op3H);
		}
	}
}
