// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using System.Diagnostics;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic:GetIDTJumpLocation")]
		private static void GetIDTJumpLocation(Context context, MethodCompiler methodCompiler)
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
			var method = methodCompiler.TypeSystem.DefaultLinkerType.FindMethodByName("InterruptISR" + irq.ToString());

			if (method == null)
			{
				throw new CompilerException();
			}

			context.SetInstruction(IRInstruction.Move32, context.Result, Operand.CreateSymbolFromMethod(method, methodCompiler.TypeSystem));

			methodCompiler.MethodScanner.MethodInvoked(method, methodCompiler.Method);
		}
	}
}
