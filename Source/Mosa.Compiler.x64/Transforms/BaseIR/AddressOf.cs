// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// AddressOf
/// </summary>
public sealed class AddressOf : BaseIRTransform
{
	public AddressOf() : base(IR.AddressOf, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(context.Operand1.IsOnStack || context.Operand1.IsStaticField);

		if (context.Operand1.IsStaticField)
		{
			context.SetInstruction(X64.Mov64, context.Result, context.Operand1);
		}
		else if (context.Operand1.IsLocalStack)
		{
			context.SetInstruction(X64.Lea64, context.Result, transform.StackFrame, Operand.Constant64_0, Operand.Constant64_1, context.Operand1);
		}
		else
		{
			var offset = Operand.CreateConstant32(context.Operand1.Offset);

			context.SetInstruction(X64.Lea64, context.Result, transform.StackFrame, Operand.Constant64_0, Operand.Constant64_1, offset);
		}
	}
}
