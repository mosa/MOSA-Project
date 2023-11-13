// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// DivSigned32
/// </summary>
[Transform("x64.BaseIR")]
public sealed class DivSigned32 : BaseIRTransform
{
	public DivSigned32() : base(Framework.IR.DivSigned32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;
		var result = context.Result;

		var v1 = transform.VirtualRegisters.Allocate64();
		var v2 = transform.VirtualRegisters.Allocate64();
		var v3 = transform.VirtualRegisters.Allocate64();

		context.SetInstruction(X64.Mov32, v2, operand1);
		context.AppendInstruction(X64.Cdq32, v3, v2);
		context.AppendInstruction2(X64.IDiv32, v1, result, v3, v2, operand2);
	}
}
