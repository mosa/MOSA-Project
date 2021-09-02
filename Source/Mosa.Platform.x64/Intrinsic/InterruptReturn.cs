﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Platform.Intel;

namespace Mosa.Platform.x64.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::InterruptReturn")]
		private static void InterruptReturn(Context context, MethodCompiler methodCompiler)
		{
			Operand v0 = context.Operand1;

			Operand esp = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I8, GeneralPurposeRegister.ESP);

			context.SetInstruction(X64.Mov64, esp, v0);
			context.AppendInstruction(X64.Popad);
			context.AppendInstruction(X64.Add64, esp, esp, methodCompiler.CreateConstant(8));
			context.AppendInstruction(X64.Sti);
			context.AppendInstruction(X64.IRetd);

			// future - common code (refactor opportunity)
			context.GotoNext();

			// Remove all remaining instructions in block and clear next block list
			while (!context.IsBlockEndInstruction)
			{
				if (!context.IsEmpty)
				{
					context.Empty();
				}
				context.GotoNext();
			}

			var nextBlocks = context.Block.NextBlocks;

			foreach (var next in nextBlocks)
			{
				next.PreviousBlocks.Remove(context.Block);
			}

			nextBlocks.Clear();
		}
	}
}
