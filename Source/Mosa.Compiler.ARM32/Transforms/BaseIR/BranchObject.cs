// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// BranchObject
/// </summary>
public sealed class BranchObject : BaseIRTransform
{
	public BranchObject() : base(IR.BranchObject, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Framework.Transform.MoveConstantRight(context);

		var target = context.BranchTarget1;
		var condition = context.ConditionCode;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		context.SetInstruction(ARM32.Cmp, null, operand1, operand2);
		context.AppendInstruction(ARM32.B, condition, target);
	}
}
