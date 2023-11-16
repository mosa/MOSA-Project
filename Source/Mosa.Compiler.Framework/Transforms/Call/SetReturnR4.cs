// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Call;

/// <summary>
/// SetReturnR4
/// </summary>
public sealed class SetReturnR4 : BaseTransform
{
	public SetReturnR4() : base(IR.SetReturnR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(IR.MoveR4, transform.PhysicalRegisters.AllocateR4(transform.Architecture.ReturnFloatingPointRegister), context.Operand1);
	}
}
