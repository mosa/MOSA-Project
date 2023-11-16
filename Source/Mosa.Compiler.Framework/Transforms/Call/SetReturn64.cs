// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Call;

/// <summary>
/// SetReturn64
/// </summary>
public sealed class SetReturn64 : BaseTransform
{
	public SetReturn64() : base(IR.SetReturn64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var operand = context.Operand1;

		if (transform.Is32BitPlatform)
		{
			context.SetInstruction(IR.GetLow32, transform.PhysicalRegisters.Allocate32(transform.Architecture.ReturnRegister), operand);
			context.AppendInstruction(IR.GetHigh32, transform.PhysicalRegisters.Allocate32(transform.Architecture.ReturnHighRegister), operand);
		}
		else
		{
			context.SetInstruction(IR.Move64, transform.PhysicalRegisters.Allocate64(transform.Architecture.ReturnRegister), context.Operand1);
		}
	}
}
