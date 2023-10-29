// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Call;

/// <summary>
/// SetReturnObject
/// </summary>
public sealed class SetReturnObject : BaseTransform
{
	public SetReturnObject() : base(IRInstruction.SetReturnObject, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(IRInstruction.MoveObject, transform.PhysicalRegisters.AllocateObject(transform.Architecture.ReturnRegister), context.Operand1);
	}
}
