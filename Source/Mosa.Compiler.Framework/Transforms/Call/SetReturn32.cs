// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Call;

/// <summary>
/// SetReturn32
/// </summary>
public sealed class SetReturn32 : BaseTransform
{
	public SetReturn32() : base(IR.SetReturn32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(IR.Move32, transform.PhysicalRegisters.Allocate32(transform.Architecture.ReturnRegister), context.Operand1);
	}
}
