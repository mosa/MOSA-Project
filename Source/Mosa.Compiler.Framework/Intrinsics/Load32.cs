﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;

namespace Mosa.Compiler.Framework.Intrinsics;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Runtime.Intrinsic::Load32")]
	private static void Load32(Context context, MethodCompiler methodCompiler)
	{
		var instruction = methodCompiler.Is32BitPlatform ? IRInstruction.Load32 : IRInstruction.LoadZeroExtend32x64;

		if (context.OperandCount == 1)
		{
			context.SetInstruction(instruction, context.Result, context.Operand1, Operand.Constant64_0);
		}
		else if (context.OperandCount == 2)
		{
			context.SetInstruction(instruction, context.Result, context.Operand1, context.Operand2);
		}
		else
		{
			throw new CompilerException();
		}

		LoadStore.OrderOperands(context, methodCompiler);
	}
}
