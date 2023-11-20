// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.FixedRegisters;

/// <summary>
/// RdMSR
/// </summary>
public sealed class RdMSR : BaseTransform
{
	public RdMSR() : base(X64.RdMSR, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return !(context.Result.IsPhysicalRegister
				 && context.Result2.IsPhysicalRegister
				 && context.Operand1.IsPhysicalRegister
				 && context.Result.Register == CPURegister.RAX
				 && context.Result2.Register == CPURegister.RDX
				 && context.Operand1.Register == CPURegister.RCX);
	}

	public override void Transform(Context context, Transform transform)
	{
		var operand1 = context.Operand1;
		var result = context.Result;
		var result2 = context.Result2;

		var rax = transform.PhysicalRegisters.Allocate64(CPURegister.RAX);
		var rdx = transform.PhysicalRegisters.Allocate64(CPURegister.RDX);
		var rcx = transform.PhysicalRegisters.Allocate64(CPURegister.RCX);

		context.SetInstruction(X64.Mov64, rcx, operand1);
		context.AppendInstruction2(X64.RdMSR, rax, rdx, rcx);
		context.AppendInstruction(X64.Mov64, result, rax);
		context.AppendInstruction(X64.Mov64, result2, rdx);
	}
}
