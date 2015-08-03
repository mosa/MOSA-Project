// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
			if (BasicBlocks.PrologueBlock == null)
				return;

			for (var node = BasicBlocks.PrologueBlock.First; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.Instruction == IRInstruction.Prologue)
				{
					AddPrologueInstructions(new Context(node));
					return;
				}
			}
		}

		/// <summary>
		/// Updates the epilogue.
		/// </summary>
		private void UpdateEpilogue()
		{
			if (BasicBlocks.EpilogueBlock == null)
				return;

			for (var node = BasicBlocks.EpilogueBlock.First; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.Instruction == IRInstruction.Epilogue)
				{
					AddEpilogueInstructions(new Context(node));
					return;
				}
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
				context.AppendInstruction(X86.Sub, esp, esp, Operand.CreateConstant(TypeSystem, -MethodCompiler.StackLayout.StackSize));
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

			context.Empty();

			if (MethodCompiler.StackLayout.StackSize != 0)
			{
				context.AppendInstruction(X86.Add, esp, esp, Operand.CreateConstant(TypeSystem, -MethodCompiler.StackLayout.StackSize));
			}

			context.AppendInstruction(X86.Pop, ebp);
			context.AppendInstruction(X86.Ret);
		}
	}
}