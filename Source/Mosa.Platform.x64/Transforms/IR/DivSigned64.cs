// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// DivSigned64
/// </summary>
[Transform("x64.IR")]
public sealed class DivSigned64 : BaseIRTransform
{
	public DivSigned64() : base(IRInstruction.DivSigned64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		var v1 = transform.VirtualRegisters.Allocate64();
		var v2 = transform.VirtualRegisters.Allocate64();
		var v3 = transform.VirtualRegisters.Allocate64();

		context.SetInstruction(X64.Mov64, v2, operand1);
		context.AppendInstruction(X64.Cdq64, v3, v2);
		context.AppendInstruction2(X64.IDiv64, v1, result, v3, v2, operand2);
	}
}
