// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// AddressOf
/// </summary>
[Transform("ARM32.IR")]
public sealed class AddressOf : BaseIRTransform
{
	public AddressOf() : base(IRInstruction.AddressOf, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(context.Operand1.IsOnStack || context.Operand1.IsStaticField);

		var result = context.Result;
		var operand1 = context.Operand1;

		operand1 = MoveConstantToRegisterOrImmediate(transform, context, operand1);

		if (operand1.IsStaticField)
		{
			context.SetInstruction(ARM32.Mov, result, operand1);
		}
		else if (operand1.IsLocalStack)
		{
			context.SetInstruction(ARM32.Add, result, transform.StackFrame, operand1);
		}
		else if (context.Operand1.IsUnresolvedConstant)
		{
			var offset = MoveConstantToRegister(transform, context, operand1);

			context.SetInstruction(ARM32.Add, result, transform.StackFrame, offset);
		}
		else
		{
			var offset = Operand.CreateConstant32(context.Operand1.Offset);

			offset = MoveConstantToRegisterOrImmediate(transform, context, offset);

			context.SetInstruction(ARM32.Add, result, transform.StackFrame, offset);
		}
	}
}
