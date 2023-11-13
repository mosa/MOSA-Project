// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Call;

/// <summary>
/// SetReturnManagedPointer
/// </summary>
public sealed class SetReturnManagedPointer : BaseTransform
{
	public SetReturnManagedPointer() : base(Framework.IR.SetReturnManagedPointer, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(Framework.IR.MoveManagedPointer, transform.PhysicalRegisters.AllocateObject(transform.Architecture.ReturnRegister), context.Operand1);
	}
}
