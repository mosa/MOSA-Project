// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Platform.Intel;

namespace Mosa.Platform.x64.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	internal static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::FrameJump")]
		private static void FrameJump(Context context, MethodCompiler methodCompiler)
		{
			var v0 = context.Operand1;
			var v1 = context.Operand2;
			var v2 = context.Operand3;
			var v3 = context.GetOperand(3);

			var esp = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I8, CPURegister.RSP);
			var ebp = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I8, CPURegister.RBP);

			var eax = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I8, CPURegister.R1);
			var ebx = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I8, CPURegister.R3);
			var ecx = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I8, CPURegister.R1);

			// Move all virtual registers into physical registers - necessary since stack frame pointer will change
			context.SetInstruction(X64.Mov64, eax, v0);
			context.AppendInstruction(X64.Mov64, ebx, v1);
			context.AppendInstruction(X64.Mov64, ecx, v2);
			context.AppendInstruction(X64.Mov64, methodCompiler.Compiler.ExceptionRegister, v3);

			// Update the frame and stack registers
			context.AppendInstruction(X64.Mov64, ebp, ecx);
			context.AppendInstruction(X64.Mov64, esp, ebx);
			context.AppendInstruction(X64.JmpExternal, null, eax);

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
