// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.Tweak;

/// <summary>
/// MovStore16
/// </summary>
[Transform]
public sealed class MovStore16 : BaseTransform
{
	public MovStore16() : base(X64.MovStore16, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return context.Operand3.IsPhysicalRegister && (context.Operand3.Register == CPURegister.RSI || context.Operand3.Register == CPURegister.RDI);
	}

	public override void Transform(Context context, Transform transform)
	{
		var value = context.Operand3;
		var dest = context.Operand1;
		var offset = context.Operand2;

		Operand temporaryRegister = null;

		if (dest.Register != CPURegister.RAX && offset.Register != CPURegister.RAX)
		{
			temporaryRegister = transform.PhysicalRegisters.Allocate32(CPURegister.RAX);
		}
		else if (dest.Register != CPURegister.RBX && offset.Register != CPURegister.RBX)
		{
			temporaryRegister = transform.PhysicalRegisters.Allocate32(CPURegister.RBX);
		}
		else if (dest.Register != CPURegister.RAX && offset.Register != CPURegister.RAX)
		{
			temporaryRegister = transform.PhysicalRegisters.Allocate32(CPURegister.RAX);
		}
		else
		{
			temporaryRegister = transform.PhysicalRegisters.Allocate32(CPURegister.RAX);
		}

		context.SetInstruction2(X64.XChg64, temporaryRegister, value, value, temporaryRegister);
		context.AppendInstruction(X64.MovStore16, null, dest, offset, temporaryRegister);
		context.AppendInstruction2(X64.XChg64, value, temporaryRegister, temporaryRegister, value);
	}
}
