// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::GetIDTJumpLocation")]
	private static void GetIDTJumpLocation(Context context, TransformContext transformContext)
	{
		var operand = context.Operand1;

		if (!operand.IsResolvedConstant)
		{
			// try to find the constant - a bit of a hack
			var ctx = new Context(operand.Definitions[0]);

			if ((ctx.Instruction == IRInstruction.Move64 || ctx.Instruction == IRInstruction.Move32) && ctx.Operand1.IsConstant)
			{
				operand = ctx.Operand1;
			}
		}

		Debug.Assert(operand.IsResolvedConstant);

		int irq = (int)operand.ConstantSigned64;

		// Find the method
		var method = transformContext.TypeSystem.DefaultLinkerType.FindMethodByName("InterruptISR" + irq);

		if (method == null)
		{
			throw new CompilerException();
		}

		context.SetInstruction(IRInstruction.Move64, context.Result, Operand.CreateLabel(method, transformContext.Is32BitPlatform));

		transformContext.MethodScanner.MethodInvoked(method, transformContext.Method);
	}
}
