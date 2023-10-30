// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.FixedRegisters;

/// <summary>
/// RdMSR
/// </summary>
[Transform("x86.FixedRegisters")]
public sealed class RdMSR : BaseTransform
{
	public RdMSR() : base(X86.RdMSR, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (context.Result.IsPhysicalRegister
			&& context.Result2.IsPhysicalRegister
			&& context.Operand1.IsPhysicalRegister
			&& context.Result.Register == CPURegister.EAX
			&& context.Result2.Register == CPURegister.EDX
			&& context.Operand1.Register == CPURegister.ECX)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var operand1 = context.Operand1;
		var result = context.Result;
		var result2 = context.Result2;

		var eax = transform.PhysicalRegisters.Allocate32(CPURegister.EAX);
		var edx = transform.PhysicalRegisters.Allocate32(CPURegister.EDX);
		var ecx = transform.PhysicalRegisters.Allocate32(CPURegister.ECX);

		context.SetInstruction(X86.Mov32, ecx, operand1);
		context.AppendInstruction2(X86.RdMSR, eax, edx, ecx);
		context.AppendInstruction(X86.Mov32, result, eax);
		context.AppendInstruction(X86.Mov32, result2, edx);
	}
}
