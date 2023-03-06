// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Transforms.IR;

/// <summary>
/// RemSigned64
/// </summary>
public sealed class RemSigned64 : BaseIRTransform
{
	public RemSigned64() : base(IRInstruction.RemSigned64, TransformType.Manual | TransformType.Transform)
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
