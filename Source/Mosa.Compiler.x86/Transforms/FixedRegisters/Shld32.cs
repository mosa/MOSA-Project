// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.FixedRegisters;

/// <summary>
/// Shld32
/// </summary>
[Transform("x86.FixedRegisters")]
public sealed class Shld32 : BaseTransform
{
	public Shld32() : base(X86.Shld32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (context.Operand3.IsConstant)
			return false;

		if (context.Operand3.Register == CPURegister.ECX)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;
		var operand3 = context.Operand3;
		var result = context.Result;

		var ecx = transform.PhysicalRegisters.Allocate32(CPURegister.ECX);

		context.SetInstruction(X86.Mov32, ecx, operand3);
		context.AppendInstruction(X86.Shld32, result, operand1, operand2, ecx);
	}
}
