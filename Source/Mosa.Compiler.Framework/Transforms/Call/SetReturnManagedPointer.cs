// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Call;

/// <summary>
/// SetReturnManagedPointer
/// </summary>
public sealed class SetReturnManagedPointer : BaseTransform
{
	public SetReturnManagedPointer() : base(IR.SetReturnManagedPointer, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(IR.MoveManagedPointer, transform.PhysicalRegisters.AllocateObject(transform.Architecture.ReturnRegister), context.Operand1);
	}
}
