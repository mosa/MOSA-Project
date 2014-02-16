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
	public sealed class BuildStackStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{
		#region IMethodCompilerStage

		/// <summary>
		/// Setup stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			if (methodCompiler.Compiler.PlugSystem.GetPlugMethod(methodCompiler.Method) != null)
				return;

			Debug.Assert((methodCompiler.StackLayout.StackSize % 4) == 0, @"Stack size of interrupt can't be divided by 4!!");

			UpdatePrologue();
			UpdateEpilogue();
		}

		#endregion IMethodCompilerStage

		public bool SaveRegisters { get; set; }

		public bool InsertBreaks { get; set; }

		public BuildStackStage()
		{
			SaveRegisters = false;
			InsertBreaks = false;
		}

		/// <summary>
		/// Updates the prologue.
		/// </summary>
		private void UpdatePrologue()
		{
			// Update prologue Block
			var prologueBlock = this.basicBlocks.PrologueBlock;

			if (prologueBlock != null)
			{
				Context prologueContext = new Context(instructionSet, prologueBlock);

				prologueContext.GotoNext();

				Debug.Assert(prologueContext.Instruction is Prologue);

				AddPrologueInstructions(prologueContext);
			}
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

				while (epilogueContext.IsEmpty)
				{
					epilogueContext.GotoNext();
				}

				Debug.Assert(epilogueContext.Instruction is Epilogue);

				AddEpilogueInstructions(epilogueContext);
			}
		}

		/// <summary>
		/// Adds the prologue instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		private void AddPrologueInstructions(Context context)
		{
			Operand ebp = Operand.CreateCPURegister(typeSystem.BuiltIn.Int32, GeneralPurposeRegister.EBP);
			Operand esp = Operand.CreateCPURegister(typeSystem.BuiltIn.Int32, GeneralPurposeRegister.ESP);

			//Operand eax = Operand.CreateCPURegister(typeSystem.BuiltIn.Int32, GeneralPurposeRegister.EAX);
			Operand edx = Operand.CreateCPURegister(typeSystem.BuiltIn.Int32, GeneralPurposeRegister.EDX);
			Operand edi = Operand.CreateCPURegister(typeSystem.BuiltIn.Int32, GeneralPurposeRegister.EDI);
			Operand ecx = Operand.CreateCPURegister(typeSystem.BuiltIn.Int32, GeneralPurposeRegister.ECX);
			Operand ebx = Operand.CreateCPURegister(typeSystem.BuiltIn.Int32, GeneralPurposeRegister.EBX);

			context.SetInstruction(X86.Push, null, ebp);
			context.AppendInstruction(X86.Mov, ebp, esp);

			if (methodCompiler.StackLayout.StackSize != 0)
			{
				context.AppendInstruction(X86.Sub, esp, esp, Operand.CreateConstantSignedInt(typeSystem, -methodCompiler.StackLayout.StackSize));
			}

			if (InsertBreaks)// && !methodCompiler.Method.FullName.Equals(".cctor"))
			{
				//Note that if you debug using visual studio you must enable unmanaged code
				//debugging, otherwise the function will never return and the breakpoint will
				//never appear.

				// int 3
				context.AppendInstruction(X86.Break);

				// Uncomment this line to enable breakpoints within Bochs
				//context.AppendInstruction(CPUx86.Instruction.BochsDebug);
			}

			if (SaveRegisters)
			{
				// Save EDX for int32 return values (or do not save EDX for non-int64 return values)
				if (!methodCompiler.Method.ReturnType.IsSignedLong && !methodCompiler.Method.ReturnType.IsUnsignedLong)
				{
					context.AppendInstruction(X86.Push, null, edx);
				}
				context.AppendInstruction(X86.Push, null, edi);
				context.AppendInstruction(X86.Push, null, ecx);
				context.AppendInstruction(X86.Push, null, ebx);
			}
		}

		/// <summary>
		/// Adds the epilogue instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		private void AddEpilogueInstructions(Context context)
		{
			Operand ebp = Operand.CreateCPURegister(typeSystem.BuiltIn.Int32, GeneralPurposeRegister.EBP);
			Operand esp = Operand.CreateCPURegister(typeSystem.BuiltIn.Int32, GeneralPurposeRegister.ESP);

			Operand edx = Operand.CreateCPURegister(typeSystem.BuiltIn.Int32, GeneralPurposeRegister.EDX);
			Operand edi = Operand.CreateCPURegister(typeSystem.BuiltIn.Int32, GeneralPurposeRegister.EDI);
			Operand ecx = Operand.CreateCPURegister(typeSystem.BuiltIn.Int32, GeneralPurposeRegister.ECX);
			Operand ebx = Operand.CreateCPURegister(typeSystem.BuiltIn.Int32, GeneralPurposeRegister.EBX);

			context.SetInstruction(X86.Nop);

			if (SaveRegisters)
			{
				context.AppendInstruction(X86.Pop, ebx);
				context.AppendInstruction(X86.Pop, ecx);
				context.AppendInstruction(X86.Pop, edi);

				// Save EDX for int32 return values (or do not save EDX for non-int64 return values)
				if (!methodCompiler.Method.ReturnType.IsSignedLong && !methodCompiler.Method.ReturnType.IsUnsignedLong)
				{
					context.AppendInstruction(X86.Pop, edx);
				}
			}

			if (methodCompiler.StackLayout.StackSize != 0)
			{
				context.AppendInstruction(X86.Add, esp, esp, Operand.CreateConstantSignedInt(typeSystem, -methodCompiler.StackLayout.StackSize));
			}

			context.AppendInstruction(X86.Pop, ebp);
			context.AppendInstruction(X86.Ret);
		}
	}
}