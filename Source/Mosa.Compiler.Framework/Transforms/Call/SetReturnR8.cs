// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Call;

/// <summary>
/// SetReturnR8
/// </summary>
public sealed class SetReturnR8 : BaseTransform
{
	public SetReturnR8() : base(Framework.IR.SetReturnR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(Framework.IR.MoveR8, transform.PhysicalRegisters.AllocateR8(transform.Architecture.ReturnFloatingPointRegister), context.Operand1);
	}
}
