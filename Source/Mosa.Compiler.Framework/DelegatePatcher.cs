/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System.Collections.Generic;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.TypeSystem;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// 
	/// </summary>
	public static class DelegatePatcher
	{

		/// <summary>
		/// Patches the delegate.
		/// </summary>
		/// <param name="method">The method.</param>
		/// <param name="context">The context.</param>
		/// <returns></returns>
		public static bool PatchDelegate(RuntimeMethod method, InstructionSet instructionSet, BasicBlocks basicBlocks)
		{
			if (!method.DeclaringType.IsDelegate)
				return false;

			switch (method.Name)
			{
				case ".ctor": PatchConstructor(instructionSet, basicBlocks); return false;
				case "Invoke": PatchInvoke(instructionSet, basicBlocks); return false;
				case "BeginInvoke": PatchBeginInvoke(instructionSet, basicBlocks); return true;
				case "EndInvoke": PatchEndInvoke(instructionSet, basicBlocks); return true;
				default: return false;
			}
		}

		private static void PatchConstructor(InstructionSet instructionSet, BasicBlocks basicBlocks)
		{ }

		private static void PatchInvoke(InstructionSet instructionSet, BasicBlocks basicBlocks)
		{ }

		private static void PatchBeginInvoke(InstructionSet instructionSet, BasicBlocks basicBlocks)
		{
			Context context = CreateMethodStructure(instructionSet, basicBlocks);
			context.AppendInstruction(IRInstruction.Return, Operand.GetNull());
		}

		private static void PatchEndInvoke(InstructionSet instructionSet, BasicBlocks basicBlocks)
		{
			Context context = CreateMethodStructure(instructionSet, basicBlocks);
			context.AppendInstruction(IRInstruction.Jmp, basicBlocks.EpilogueBlock);
		}

		private static Context CreateMethodStructure(InstructionSet instructionSet, BasicBlocks basicBlocks)
		{
			CreatePrologueAndEpilogueBlocks(instructionSet, basicBlocks);

			Context context = new Context(instructionSet);
			context.AppendInstruction(null);
			context.Label = 0;

			var newblock = basicBlocks.CreateBlock(0, context.Index);
			basicBlocks.LinkBlocks(basicBlocks.PrologueBlock, newblock);
			basicBlocks.LinkBlocks(newblock, basicBlocks.EpilogueBlock);

			return context;
		}

		private static void CreatePrologueAndEpilogueBlocks(InstructionSet instructionSet, BasicBlocks basicBlocks)
		{
			// Create the prologue block
			Context context = new Context(instructionSet);
			// Add a jump instruction to the first block from the prologue
			context.AppendInstruction(IRInstruction.Jmp);
			context.SetBranch(0);
			context.Label = BasicBlock.PrologueLabel;
			var prologue = basicBlocks.CreateBlock(BasicBlock.PrologueLabel, context.Index);
			basicBlocks.AddHeaderBlock(prologue);

			// Create the epilogue block
			context = new Context(instructionSet);
			// Add null instruction, necessary to generate a block index
			context.AppendInstruction(null);
			context.Label = BasicBlock.EpilogueLabel;
			var epilogue = basicBlocks.CreateBlock(BasicBlock.EpilogueLabel, context.Index);
		}

	}
}
