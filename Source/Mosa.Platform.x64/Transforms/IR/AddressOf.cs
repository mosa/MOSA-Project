// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Transforms.IR;

/// <summary>
/// AddressOf
/// </summary>
public sealed class AddressOf : BaseIRTransform
{
	public AddressOf() : base(IRInstruction.AddressOf, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		Debug.Assert(context.Operand1.IsOnStack || context.Operand1.IsStaticField);

		if (context.Operand1.IsStaticField)
		{
			context.SetInstruction(X64.Mov64, context.Result, context.Operand1);
		}
		else if (context.Operand1.IsStackLocal)
		{
			context.SetInstruction(X64.Lea64, context.Result, transform.StackFrame, context.Operand1);
		}
		else
		{
			var offset = transform.CreateConstant32(context.Operand1.Offset);

			context.SetInstruction(X64.Lea64, context.Result, transform.StackFrame, offset);
		}
	}
}
