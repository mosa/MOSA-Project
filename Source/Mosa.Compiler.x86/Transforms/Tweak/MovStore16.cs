// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.Tweak;

/// <summary>
/// MovStore16
/// </summary>
public sealed class MovStore16 : BaseTransform
{
	public MovStore16() : base(X86.MovStore16, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return context.Operand3.IsPhysicalRegister && (context.Operand3.Register == CPURegister.ESI || context.Operand3.Register == CPURegister.EDI);
	}

	public override void Transform(Context context, Transform transform)
	{
		var dest = context.Operand1;
		var offset = context.Operand2;
		var value = context.Operand3;

		Operand temporaryRegister;

		if (dest.Register != CPURegister.EAX && offset.Register != CPURegister.EAX)
		{
			temporaryRegister = transform.PhysicalRegisters.Allocate32(CPURegister.EAX);
		}
		else if (dest.Register != CPURegister.EBX && offset.Register != CPURegister.EBX)
		{
			temporaryRegister = transform.PhysicalRegisters.Allocate32(CPURegister.EBX);
		}
		else if (dest.Register != CPURegister.ECX && offset.Register != CPURegister.ECX)
		{
			temporaryRegister = transform.PhysicalRegisters.Allocate32(CPURegister.ECX);
		}
		else
		{
			temporaryRegister = transform.PhysicalRegisters.Allocate32(CPURegister.EDX);
		}

		context.SetInstruction2(X86.XChg32, temporaryRegister, value, value, temporaryRegister);
		context.AppendInstruction(X86.MovStore16, null, dest, offset, temporaryRegister);
		context.AppendInstruction2(X86.XChg32, value, temporaryRegister, temporaryRegister, value);
	}
}
