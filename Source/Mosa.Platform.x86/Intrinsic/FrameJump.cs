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
		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic:FrameJump")]
		private static void FrameJump(Context context, MethodCompiler methodCompiler)
		{
			Operand v0 = context.Operand1;
			Operand v1 = context.Operand2;
			Operand v2 = context.Operand3;
			Operand v3 = context.GetOperand(3);

			Operand esp = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, GeneralPurposeRegister.ESP);
			Operand ebp = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EBP);

			Operand eax = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);
			Operand ebx = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EBX);
			Operand ecx = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, GeneralPurposeRegister.ECX);

			Operand exceptionRegister = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.Object, methodCompiler.Architecture.ExceptionRegister);

			// Move all virtual registers into physical registers - necessary since stack frame pointer will change
			context.SetInstruction(X86.Mov32, eax, v0);
			context.AppendInstruction(X86.Mov32, ebx, v1);
			context.AppendInstruction(X86.Mov32, ecx, v2);
			context.AppendInstruction(X86.Mov32, exceptionRegister, v3);

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
					context.SetInstruction(X86.Nop);
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
