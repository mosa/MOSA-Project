// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// RemSigned64
/// </summary>
[Transform("x64.BaseIR")]
public sealed class RemSigned64 : BaseIRTransform
{
	public RemSigned64() : base(IR.RemSigned64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		var v1 = transform.VirtualRegisters.Allocate32();
		var v2 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(X64.Mov64, v1, Operand.Constant64_0);
		context.AppendInstruction2(X64.Div64, result, v2, v1, operand1, operand2);
	}
}
