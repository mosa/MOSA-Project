// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.FixedRegisters;

/// <summary>
/// In32
/// </summary>
public sealed class In32 : BaseTransform
{
	public In32() : base(X64.In32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return !(context.Result.IsPhysicalRegister
				 && context.Operand1.IsPhysicalRegister
				 && context.Result.Register == CPURegister.RAX
				 && (context.Operand1.Register == CPURegister.RDX || context.Operand1.IsConstant));
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		var rax = transform.PhysicalRegisters.Allocate64(CPURegister.RAX);
		var rdx = transform.PhysicalRegisters.Allocate32(CPURegister.RDX);

		context.SetInstruction(X64.Mov64, rdx, operand1);
		context.AppendInstruction(X64.In32, rax, rdx);
		context.AppendInstruction(X64.Mov64, result, rax);
	}
}
