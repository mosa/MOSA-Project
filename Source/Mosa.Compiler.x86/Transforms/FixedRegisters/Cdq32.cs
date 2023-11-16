// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.FixedRegisters;

/// <summary>
/// Cdq32
/// </summary>
[Transform]
public sealed class Cdq32 : BaseTransform
{
	public Cdq32() : base(X86.Cdq32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (context.Result.IsPhysicalRegister
			&& context.Operand1.IsPhysicalRegister
			&& context.Result.Register == CPURegister.EDX
			&& context.Operand1.Register == CPURegister.EAX)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var operand1 = context.Operand1;
		var result = context.Result;

		var eax = transform.PhysicalRegisters.Allocate32(CPURegister.EAX);
		var edx = transform.PhysicalRegisters.Allocate32(CPURegister.EDX);

		context.SetInstruction(X86.Mov32, eax, operand1);
		context.AppendInstruction(X86.Cdq32, edx, eax);
		context.AppendInstruction(X86.Mov32, result, edx);
	}
}
