// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.Framework.Call;

/// <summary>
/// SetReturnObject
/// </summary>
public sealed class SetReturnObject : BaseTransform
{
	public SetReturnObject() : base(IRInstruction.SetReturnObject, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.SetInstruction(IRInstruction.MoveObject, Operand.CreateCPURegisterObject(transform.Architecture.ReturnRegister), context.Operand1);
	}
}
