// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.FixedRegisters;

/// <summary>
/// Mul32
/// </summary>
[Transform]
public sealed class Mul32 : BaseTransform
{
	public Mul32() : base(X64.Mul32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (context.Result.IsPhysicalRegister
			&& context.Result2.IsPhysicalRegister
			&& context.Operand1.IsPhysicalRegister
			&& !context.Operand2.IsConstant
			&& context.Result.Register == CPURegister.RDX
			&& context.Result2.Register == CPURegister.RAX
			&& context.Operand1.Register == CPURegister.RAX)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;
		var result = context.Result;
		var result2 = context.Result2;

		var rax = transform.PhysicalRegisters.Allocate32(CPURegister.RAX);
		var rdx = transform.PhysicalRegisters.Allocate32(CPURegister.RDX);

		context.SetInstruction(X64.Mov64, rax, operand1);

		if (operand2.IsConstant)
		{
			var v1 = transform.VirtualRegisters.Allocate32();
			context.AppendInstruction(X64.Mov64, v1, operand2);
			operand2 = v1;
		}

		Debug.Assert(operand2.IsPhysicalRegister || operand2.IsVirtualRegister);

		context.AppendInstruction2(X64.Mul32, rdx, rax, rax, operand2);
		context.AppendInstruction(X64.Mov64, result, rdx);
		context.AppendInstruction(X64.Mov64, result2, rax);
	}
}
