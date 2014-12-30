/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using System.Diagnostics;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// Completes the stack handling after register allocation
	/// </summary>
	public sealed class BuildStackStage : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			if (MethodCompiler.Compiler.PlugSystem.GetPlugMethod(MethodCompiler.Method) != null)
				return;

			Debug.Assert((MethodCompiler.StackLayout.StackSize % 4) == 0, @"Stack size of interrupt can't be divided by 4!!");

			UpdatePrologue();
			UpdateEpilogue();
		}

		/// <summary>
		/// Updates the prologue.
		/// </summary>
		private void UpdatePrologue()
		{
			// Update prologue Block
			var prologueBlock = BasicBlocks.PrologueBlock;

			if (prologueBlock != null)
			{
				var prologueContext = new Context(InstructionSet, prologueBlock);

				prologueContext.GotoNext();

				Debug.Assert(prologueContext.Instruction == IRInstruction.Prologue);

				AddPrologueInstructions(prologueContext);
			}
		}

		/// <summary>
		/// Updates the epilogue.
		/// </summary>
		private void UpdateEpilogue()
		{
			// Update epilogue Block
			var epilogueBlock = BasicBlocks.EpilogueBlock;

			if (epilogueBlock != null)
			{
				var epilogueContext = new Context(InstructionSet, epilogueBlock);

				epilogueContext.GotoNext();

				while (epilogueContext.IsEmpty)
				{
					epilogueContext.GotoNext();
				}

				Debug.Assert(epilogueContext.Instruction == IRInstruction.Epilogue);

				AddEpilogueInstructions(epilogueContext);
			}
		}

		/// <summary>
		/// Adds the prologue instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		private void AddPrologueInstructions(Context context)
		{
			Operand ebp = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EBP);
			Operand esp = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.ESP);

			context.SetInstruction(X86.Push, null, ebp);
			context.AppendInstruction(X86.Mov, ebp, esp);

			if (MethodCompiler.StackLayout.StackSize != 0)
			{
				context.AppendInstruction(X86.Sub, esp, esp, Operand.CreateConstantSignedInt(TypeSystem, -MethodCompiler.StackLayout.StackSize));
			}
		}

		/// <summary>
		/// Adds the epilogue instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		private void AddEpilogueInstructions(Context context)
		{
			Operand ebp = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EBP);
			Operand esp = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.ESP);

			context.Remove();

			if (MethodCompiler.StackLayout.StackSize != 0)
			{
				context.AppendInstruction(X86.Add, esp, esp, Operand.CreateConstantSignedInt(TypeSystem, -MethodCompiler.StackLayout.StackSize));
			}

			context.AppendInstruction(X86.Pop, ebp);
			context.AppendInstruction(X86.Ret);
		}
	}
}
