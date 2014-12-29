/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.Framework.IR;
using System;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///
	/// </summary>
	public sealed class StackSetupStage : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			// No stack setup if this is a linker generated method
			if (MethodCompiler.Method.DeclaringType.IsLinkerGenerated)
				return;

			if (MethodCompiler.Compiler.PlugSystem.GetPlugMethod(MethodCompiler.Method) != null)
				return;

			// Create a prologue instruction
			var prologueCtx = new Context(InstructionSet, BasicBlocks.PrologueBlock);
			prologueCtx.AppendInstruction(IRInstruction.Prologue);
			prologueCtx.Label = -1;

			if (BasicBlocks.EpilogueBlock != null)
			{
				// Create an epilogue instruction
				var epilogueCtx = new Context(InstructionSet, BasicBlocks.EpilogueBlock);
				epilogueCtx.AppendInstruction(IRInstruction.Epilogue);
				epilogueCtx.Label = Int32.MaxValue;
			}
		}
	}
}
