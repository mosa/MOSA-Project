// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Call;

/// <summary>
/// SetReturnManagedPointer
/// </summary>
public sealed class SetReturnManagedPointer : BaseTransform
{
	public SetReturnManagedPointer() : base(IRInstruction.SetReturnManagedPointer, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.SetInstruction(IRInstruction.MoveManagedPointer, Operand.CreateCPURegisterObject(transform.Architecture.ReturnRegister), context.Operand1);
	}
}
