// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// RemSigned32
/// </summary>
[Transform("x86.IR")]
public sealed class RemSigned32 : BaseIRTransform
{
	public RemSigned32() : base(IRInstruction.RemSigned32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		var v1 = transform.AllocateVirtualRegister32();
		var v2 = transform.AllocateVirtualRegister32();

		context.SetInstruction(X86.Cdq32, v1, operand1);
		context.AppendInstruction2(X86.IDiv32, result, v2, v1, operand1, operand2);
	}
}
