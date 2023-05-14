// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.Framework.Call;

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
