// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.FixedRegisters;

/// <summary>
/// Out32
/// </summary>
[Transform("x64.FixedRegisters")]
public sealed class Out32 : BaseTransform
{
	public Out32() : base(X64.Out32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return !(context.Operand1.IsPhysicalRegister
				 && context.Operand2.IsPhysicalRegister
				 && (context.Operand1.Register == CPURegister.RDX || context.Operand1.IsConstant)
				 && context.Operand2.Register == CPURegister.RAX);
	}

	public override void Transform(Context context, Transform transform)
	{
		// TRANSFORM: OUT <= rdx, rax && OUT <= imm8, rax

		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		var rax = transform.PhysicalRegisters.Allocate32(CPURegister.RAX);
		var rdx = transform.PhysicalRegisters.Allocate32(CPURegister.RDX);

		context.SetInstruction(X64.Mov64, rdx, operand1);
		context.AppendInstruction(X64.Mov64, rax, operand2);
		context.AppendInstruction(X64.Out32, null, rdx, rax);
	}
}
