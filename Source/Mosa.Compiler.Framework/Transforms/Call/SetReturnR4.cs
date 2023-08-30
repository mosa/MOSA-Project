// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Call;

/// <summary>
/// SetReturnR4
/// </summary>
public sealed class SetReturnR4 : BaseTransform
{
	public SetReturnR4() : base(IRInstruction.SetReturnR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.SetInstruction(IRInstruction.MoveR4, Operand.CreateCPURegisterR4(transform.Architecture.ReturnFloatingPointRegister), context.Operand1);
	}
}
