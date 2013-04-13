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

		#endregion IMethodCompilerStage

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
			Operand ebp = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.EBP);
			Operand esp = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.ESP);

			bool breakFlag = false; // TODO: Turn this into a compiler option

			/*
			 * If you want to stop at the header of an emitted function, just set breakFlag
			 * to true in the following line. It will issue a breakpoint instruction. Note
			 * that if you debug using visual studio you must enable unmanaged code
			 * debugging, otherwise the function will never return and the breakpoint will
			 * never appear.
			 */

			// push ebp
			context.SetInstruction(X86.Push, null, ebp);

			// mov ebp, esp
			context.AppendInstruction(X86.Mov, ebp, esp);

			// sub esp, localsSize
			context.AppendInstruction(X86.Sub, esp, esp, Operand.CreateConstant((int)-stackSize));

			if (breakFlag)
			{
				// int 3
				context.AppendInstruction(X86.Break);

				// Uncomment this line to enable breakpoints within Bochs
				//context.AppendInstruction(CPUx86.Instruction.BochsDebug);
			}
		}

		/// <summary>
		/// Adds the epilogue instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		private void AddEpilogueInstructions(Context context)
		{
			Operand ebp = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.EBP);
			Operand esp = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.ESP);

			// add esp, -localsSize
			context.SetInstruction(X86.Add, esp, esp, Operand.CreateConstant(BuiltInSigType.IntPtr, -stackSize));

			// pop ebp
			context.AppendInstruction(X86.Pop, ebp);

			// ret
			context.AppendInstruction(X86.Ret);
		}
	}
}