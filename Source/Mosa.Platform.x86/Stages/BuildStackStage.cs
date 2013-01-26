/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// Completes the stack handling after register allocation
	/// </summary>
	public sealed class BuildStackStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{
		private int stackSize;

		#region IMethodCompilerStage

		/// <summary>
		/// Setup stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			if (methodCompiler.Compiler.PlugSystem.GetPlugMethod(methodCompiler.Method) != null)
				return;

			IStackLayoutProvider stackLayoutProvider = methodCompiler.Pipeline.FindFirst<IStackLayoutProvider>();

			stackSize = (stackLayoutProvider == null) ? 0 : stackLayoutProvider.LocalsSize;

			Debug.Assert((stackSize % 4) == 0, @"Stack size of method can't be divided by 4!!");

			UpdatePrologue();
			UpdateEpilogue();
			UpdateReturns();
		}

		#endregion // IMethodCompilerStage

		/// <summary>
		/// Updates the prologue.
		/// </summary>
		private void UpdatePrologue()
		{
			// Update prologue Block
			var prologueBlock = this.basicBlocks.PrologueBlock;

			Context prologueContext = new Context(instructionSet, prologueBlock);

			prologueContext.GotoNext();

			Debug.Assert(prologueContext.Instruction is Prologue);

			AddPrologueInstructions(prologueContext);
		}

		/// <summary>
		/// Updates the epilogue.
		/// </summary>
		private void UpdateEpilogue()
		{
			// Update epilogue Block
			var epilogueBlock = this.basicBlocks.EpilogueBlock;

			if (epilogueBlock != null)
			{
				Context epilogueContext = new Context(instructionSet, epilogueBlock);

				epilogueContext.GotoNext();

				Debug.Assert(epilogueContext.Instruction is Epilogue);

				AddEpilogueInstructions(epilogueContext);
			}
		}

		/// <summary>
		/// Updates the returns.
		/// </summary>
		private void UpdateReturns()
		{
			// Update Return(s)
			foreach (var block in basicBlocks)
			{
				// optimization - the return instruction is always the last instruction of the block
				Context ctx = new Context(instructionSet, block, block.EndIndex);

				ctx.GotoPrevious();

				while (ctx.IsEmpty)
				{
					ctx.GotoPrevious();
				}

				if (ctx.Instruction is Return)
				{
					if (ctx.Operand1 != null)
					{
						callingConvention.MoveReturnValue(ctx, ctx.Operand1);
						ctx.AppendInstruction(X86.Jmp);
						ctx.SetBranch(Int32.MaxValue);
					}
					else
					{
						ctx.SetInstruction(X86.Jmp);
						ctx.SetBranch(Int32.MaxValue);
					}
				}
			}
		}

		/// <summary>
		/// Adds the prologue instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		private void AddPrologueInstructions(Context context)
		{
			Operand eax = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.EAX);
			Operand ebx = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.EBX);
			Operand ecx = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.ECX);
			Operand ebp = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.EBP);
			Operand esp = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.ESP);
			Operand edi = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.EDI);
			Operand edx = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.EDX);

			/*
			 * If you want to stop at the header of an emitted function, just set breakFlag
			 * to true in the following line. It will issue a breakpoint instruction. Note
			 * that if you debug using visual studio you must enable unmanaged code
			 * debugging, otherwise the function will never return and the breakpoint will
			 * never appear.
			 */
			bool breakFlag = false; // TODO: Turn this into a compiler option

			if (breakFlag)
			{
				// int 3
				context.SetInstruction(X86.Break);
				context.AppendInstruction(X86.Nop);

				// Uncomment this line to enable breakpoints within Bochs
				//context.AppendInstruction(CPUx86.Instruction.BochsDebug);
			}

			// push ebp
			context.SetInstruction(X86.Push, null, ebp);

			// mov ebp, esp
			context.AppendInstruction(X86.Mov, ebp, esp);

			// sub esp, localsSize
			context.AppendInstruction(X86.Sub, esp, esp, Operand.CreateConstant((int)-stackSize));

			// push ebx
			context.AppendInstruction(X86.Push, null, ebx);

			// Initialize all locals to zero
			context.AppendInstruction(X86.Push, null, edi);
			context.AppendInstruction(X86.Push, null, ecx);
			context.AppendInstruction(X86.Mov, edi, esp);
			context.AppendInstruction(X86.Add, edi, edi, Operand.CreateConstant((int)(3 * 4))); // 4 bytes per push above
			context.AppendInstruction(X86.Mov, ecx, Operand.CreateConstant(BuiltInSigType.Int32, -(int)(stackSize >> 2)));
			context.AppendInstruction(X86.Mov, eax, Operand.CreateConstant((uint)0));
			context.AppendInstruction(X86.Rep);
			context.AppendInstruction(X86.Stos);

			// Save EDX for int32 return values (or do not save EDX for non-int64 return values)
			if (methodCompiler.Method.ReturnType.Type != CilElementType.I8 &&
				methodCompiler.Method.ReturnType.Type != CilElementType.U8)
			{
				// push edx
				context.AppendInstruction(X86.Push, null, edx);
			}
		}

		/// <summary>
		/// Adds the epilogue instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		private void AddEpilogueInstructions(Context context)
		{
			Operand ebx = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.EBX);
			Operand edx = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.EDX);
			Operand ebp = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.EBP);
			Operand esp = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.ESP);
			Operand ecx = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.ECX);
			Operand edi = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.EDI);

			// Load EDX for int32 return values
			if (methodCompiler.Method.ReturnType.Type != CilElementType.I8 &&
				methodCompiler.Method.ReturnType.Type != CilElementType.U8)
			{
				// pop edx
				context.SetInstruction(X86.Pop, edx);
				context.AppendInstruction(X86.Nop);
			}

			// pop ecx
			context.SetInstruction(X86.Pop, ecx);

			// pop edi
			context.AppendInstruction(X86.Pop, edi);

			// pop ebx
			context.AppendInstruction(X86.Pop, ebx);

			// add esp, -localsSize
			context.AppendInstruction(X86.Add, esp, esp, Operand.CreateConstant(BuiltInSigType.IntPtr, -stackSize));

			// pop ebp
			context.AppendInstruction(X86.Pop, ebp);

			// ret
			context.AppendInstruction(X86.Ret);
		}
	}
}