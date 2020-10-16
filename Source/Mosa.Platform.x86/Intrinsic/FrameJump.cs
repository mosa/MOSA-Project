// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Platform.Intel;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::FrameJump")]
		private static void FrameJump(Context context, MethodCompiler methodCompiler)
		{
			var v0 = context.Operand1;
			var v1 = context.Operand2;
			var v2 = context.Operand3;
			var v3 = context.GetOperand(3);

			var esp = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, GeneralPurposeRegister.ESP);
			var ebp = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EBP);

			var eax = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);
			var ebx = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EBX);
			var ecx = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, GeneralPurposeRegister.ECX);

			// Move all virtual registers into physical registers - necessary since stack frame pointer will change
			context.SetInstruction(X86.Mov32, eax, v0);
			context.AppendInstruction(X86.Mov32, ebx, v1);
			context.AppendInstruction(X86.Mov32, ecx, v2);
			context.AppendInstruction(X86.Mov32, methodCompiler.Compiler.ExceptionRegister, v3);

			// Update the frame and stack registers
			context.AppendInstruction(X86.Mov32, ebp, ecx);
			context.AppendInstruction(X86.Mov32, esp, ebx);
			context.AppendInstruction(X86.JmpExternal, null, eax);

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
