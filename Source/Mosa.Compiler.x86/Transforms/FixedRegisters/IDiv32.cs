// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.FixedRegisters;

/// <summary>
/// IDiv32
/// </summary>
[Transform]
public sealed class IDiv32 : BaseTransform
{
	public IDiv32() : base(X86.IDiv32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (context.Result.IsPhysicalRegister
			&& context.Result2.IsPhysicalRegister
			&& context.Operand1.IsPhysicalRegister
			&& context.Operand2.IsPhysicalRegister
			&& context.Result.Register == CPURegister.EAX
			&& context.Result2.Register == CPURegister.EDX
			&& context.Operand1.Register == CPURegister.EAX
			&& context.Operand2.Register == CPURegister.EDX)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;
		var operand3 = context.Operand3;
		var result = context.Result;
		var result2 = context.Result2;

		var eax = transform.PhysicalRegisters.Allocate32(CPURegister.EAX);
		var edx = transform.PhysicalRegisters.Allocate32(CPURegister.EDX);

		context.SetInstruction(X86.Mov32, edx, operand2);
		context.AppendInstruction(X86.Mov32, eax, operand1);

		if (operand3.IsConstant)
		{
			var v1 = transform.VirtualRegisters.Allocate32();
			context.AppendInstruction(X86.Mov32, v1, operand3);
			context.AppendInstruction2(X86.IDiv32, eax, edx, eax, edx, v1);
		}
		else
		{
			context.AppendInstruction2(X86.IDiv32, eax, edx, eax, edx, operand3);
		}

		context.AppendInstruction(X86.Mov32, result, eax);
		context.AppendInstruction(X86.Mov32, result2, edx);
	}
}
