// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// CompareManagedPointer
/// </summary>
public sealed class CompareManagedPointer : BaseIRTransform
{
	public CompareManagedPointer() : base(IR.CompareManagedPointer, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var condition = context.ConditionCode;
		var result = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		var v1 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(X86.Cmp32, null, operand1, operand2);
		context.AppendInstruction(X86.Setcc, condition, v1);
		context.AppendInstruction(X86.Movzx8To32, result, v1);
	}
}
