// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARM32.Transforms.IR;

/// <summary>
/// CallDirect
/// </summary>
[Transform("ARM32.IR")]
public sealed class CallDirect : BaseIRTransform
{
	public CallDirect() : base(IRInstruction.CallDirect, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var operand1 = context.Operand1;

		if (operand1.IsCPURegister || operand1.IsVirtualRegister || operand1.IsResolvedConstant)
		{
			operand1 = MoveConstantToRegister(transform, context, operand1);

			context.SetInstruction(ARM32.Add, transform.LinkRegister, transform.ProgramCounter, Operand.Constant32_4);
			context.AppendInstruction(ARM32.Mov, transform.ProgramCounter, operand1);
		}
		else
		{
			context.ReplaceInstruction(ARM32.Bl);
		}
	}
}
