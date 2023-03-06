// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Transforms.IR;

/// <summary>
/// DivSigned64
/// </summary>
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

		var v1 = transform.AllocateVirtualRegister32();
		var v2 = transform.AllocateVirtualRegister32();
		var v3 = transform.AllocateVirtualRegister32();

		context.SetInstruction2(X64.Cdq64, v1, v2, operand1);
		context.AppendInstruction2(X64.IDiv64, result, v3, v1, v2, operand2);
	}
}
