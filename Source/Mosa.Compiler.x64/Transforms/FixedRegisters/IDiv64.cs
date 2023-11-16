// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.FixedRegisters;

/// <summary>
/// IDiv64
/// </summary>
[Transform]
public sealed class IDiv64 : BaseTransform
{
	public IDiv64() : base(X64.IDiv64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (context.Result.IsPhysicalRegister
			&& context.Result2.IsPhysicalRegister
			&& context.Operand1.IsPhysicalRegister
			&& context.Operand2.IsPhysicalRegister
			&& context.Result.Register == CPURegister.RDX
			&& context.Result2.Register == CPURegister.RAX
			&& context.Operand1.Register == CPURegister.RDX
			&& context.Operand2.Register == CPURegister.RAX)
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

		var rax = transform.PhysicalRegisters.Allocate64(CPURegister.RAX);
		var rdx = transform.PhysicalRegisters.Allocate64(CPURegister.RDX);

		context.SetInstruction(X64.Mov64, rdx, operand1);
		context.AppendInstruction(X64.Mov64, rax, operand2);

		if (operand3.IsPhysicalRegister)
		{
			context.AppendInstruction2(X64.IDiv64, rdx, rax, rdx, rax, operand3);
		}
		else
		{
			var v3 = transform.VirtualRegisters.Allocate64();
			context.AppendInstruction(X64.Mov64, v3, operand3);
			context.AppendInstruction2(X64.IDiv64, rdx, rax, rdx, rax, v3);
		}

		context.AppendInstruction(X64.Mov64, result2, rax);
		context.AppendInstruction(X64.Mov64, result, rdx);
	}
}
