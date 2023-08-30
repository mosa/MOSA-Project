// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Call;

/// <summary>
/// SetReturnR8
/// </summary>
public sealed class SetReturnR8 : BaseTransform
{
	public SetReturnR8() : base(IRInstruction.SetReturnR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.SetInstruction(IRInstruction.MoveR8, Operand.CreateCPURegisterR8(transform.Architecture.ReturnFloatingPointRegister), context.Operand1);
	}
}
