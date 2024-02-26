// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// Intrinsic Methods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x64.Intrinsic::GetIDTJumpLocation")]
	private static void GetIDTJumpLocation(Context context, Transform transform)
	{
		var operand = context.Operand1;

		if (!operand.IsResolvedConstant)
		{
			// try to find the constant - a bit of a hack
			var ctx = new Context(operand.Definitions[0]);

			if ((ctx.Instruction == IR.Move64 || ctx.Instruction == IR.Move32) && ctx.Operand1.IsConstant)
			{
				operand = ctx.Operand1;
			}
		}

		Debug.Assert(operand.IsResolvedConstant);

		int irq = (int)operand.ConstantSigned64;

		// Find the method
		var method = transform.TypeSystem.DefaultLinkerType.FindMethodByName("InterruptISR" + irq);

		if (method == null)
		{
			throw new CompilerException();
		}

		context.SetInstruction(IR.Move64, context.Result, Operand.CreateLabel(method, transform.Is32BitPlatform));

		transform.MethodScanner.MethodInvoked(method, transform.Method);
	}
}
