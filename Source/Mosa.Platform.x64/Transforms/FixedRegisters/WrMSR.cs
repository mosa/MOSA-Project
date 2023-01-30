// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.FixedRegisters;

/// <summary>
/// WrMSR
/// </summary>
public sealed class WrMSR : BaseTransform
{
	public WrMSR() : base(X64.WrMSR, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return !(context.Result.IsCPURegister
				 && context.Result2.IsCPURegister
				 && context.Operand1.IsCPURegister
				 && context.Result.Register == CPURegister.RAX
				 && context.Operand1.Register == CPURegister.RAX
				 && context.Operand2.Register == CPURegister.RDX);
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;
		var result = context.Result;

		var rax = Operand.CreateCPURegister(transform.I8, CPURegister.RAX);
		var rdx = Operand.CreateCPURegister(transform.I8, CPURegister.RDX);
		var rcx = Operand.CreateCPURegister(transform.I8, CPURegister.RCX);

		context.SetInstruction(X64.Mov64, rax, operand1);
		context.AppendInstruction(X64.Mov64, rdx, operand2);
		context.AppendInstruction(X64.WrMSR, rcx, rax, rdx);
		context.AppendInstruction(X64.Mov64, result, rcx);
	}
}
