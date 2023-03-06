// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// DivUnsigned32
/// </summary>
public sealed class DivUnsigned32 : BaseIRTransform
{
	public DivUnsigned32() : base(IRInstruction.DivUnsigned32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;
		var result = context.Result;

		var v1 = transform.AllocateVirtualRegister32();
		var v2 = transform.AllocateVirtualRegister32();

		context.SetInstruction(X86.Mov32, v1, transform.Constant32_0);
		context.AppendInstruction2(X86.Div32, v1, v2, v1, operand1, operand2);
		context.AppendInstruction(X86.Mov32, result, v2);
	}
}
