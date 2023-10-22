// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Call;

/// <summary>
/// SetReturn64
/// </summary>
public sealed class SetReturn64 : BaseTransform
{
	public SetReturn64() : base(IRInstruction.SetReturn64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var operand = context.Operand1;

		if (transform.Is32BitPlatform)
		{
			context.SetInstruction(IRInstruction.GetLow32, Operand.CreateCPURegister32(transform.Architecture.ReturnRegister), operand);
			context.AppendInstruction(IRInstruction.GetHigh32, Operand.CreateCPURegister32(transform.Architecture.ReturnHighRegister), operand);
		}
		else
		{
			context.SetInstruction(IRInstruction.Move64, Operand.CreateCPURegister64(transform.Architecture.ReturnRegister), context.Operand1);
		}
	}
}
