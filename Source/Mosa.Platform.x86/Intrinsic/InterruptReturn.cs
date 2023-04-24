// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::InterruptReturn")]
	private static void InterruptReturn(Context context, MethodCompiler methodCompiler)
	{
		Operand v0 = context.Operand1;

		Operand esp = Operand.CreateCPURegister32(CPURegister.ESP);

		context.SetInstruction(X86.Mov32, esp, v0);
		context.AppendInstruction(X86.Popad);
		context.AppendInstruction(X86.Add32, esp, esp, Operand.Constant32_8);
		context.AppendInstruction(X86.Sti);
		context.AppendInstruction(X86.IRetd);

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
