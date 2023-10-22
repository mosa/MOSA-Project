// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.FixedRegisters;

/// <summary>
/// IMul32o1
/// </summary>
[Transform("x86.FixedRegisters")]
public sealed class IMul32o1 : BaseTransform
{
	public IMul32o1() : base(X86.IMul32o1, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (context.Result.IsCPURegister
			&& context.Result2.IsCPURegister
			&& context.Operand1.IsCPURegister
			&& !context.Operand2.IsConstant
			&& context.Result.Register == CPURegister.EDX
			&& context.Result2.Register == CPURegister.EAX
			&& context.Operand1.Register == CPURegister.EAX)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;
		var result = context.Result;
		var result2 = context.Result2;

		var eax = Operand.CreateCPURegister32(CPURegister.EAX);
		var edx = Operand.CreateCPURegister32(CPURegister.EDX);

		context.SetInstruction(X86.Mov32, eax, operand1);

		if (operand2.IsConstant)
		{
			var v1 = transform.VirtualRegisters.Allocate32();
			context.AppendInstruction(X86.Mov32, v1, operand2);
			operand2 = v1;
		}

		Debug.Assert(operand2.IsCPURegister || operand2.IsVirtualRegister);

		context.AppendInstruction2(X86.IMul32o1, edx, eax, eax, operand2);
		context.AppendInstruction(X86.Mov32, result, edx);
		context.AppendInstruction(X86.Mov32, result2, eax);
	}
}
