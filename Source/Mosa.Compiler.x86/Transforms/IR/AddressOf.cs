// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.IR;

/// <summary>
/// AddressOf
/// </summary>
[Transform("x86.IR")]
public sealed class AddressOf : BaseIRTransform
{
	public AddressOf() : base(IRInstruction.AddressOf, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(context.Operand1.IsOnStack || context.Operand1.IsStaticField);

		if (context.Operand1.IsStaticField)
		{
			context.SetInstruction(X86.Mov32, context.Result, context.Operand1);
		}
		else if (context.Operand1.IsLocalStack)
		{
			context.SetInstruction(X86.Lea32, context.Result, transform.StackFrame, context.Operand1);
		}
		else
		{
			var offset = Operand.CreateConstant32(context.Operand1.Offset);

			context.SetInstruction(X86.Lea32, context.Result, transform.StackFrame, offset);
		}
	}
}
