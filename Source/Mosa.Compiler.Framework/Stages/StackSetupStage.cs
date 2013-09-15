/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Linker;
using System;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///
	/// </summary>
	public sealed class StackSetupStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{
		/// <summary>
		/// Runs the specified method compiler.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			// No stack setup if this is a linker generated method
			if (methodCompiler.Method is LinkerGeneratedMethod)
				return;

			if (methodCompiler.Compiler.PlugSystem.GetPlugMethod(methodCompiler.Method) != null)
				return;

			// Create a prologue instruction
			Context prologueCtx = new Context(instructionSet, basicBlocks.PrologueBlock);
			prologueCtx.AppendInstruction(IRInstruction.Prologue);
			prologueCtx.Label = -1;

			if (basicBlocks.EpilogueBlock != null)
			{
				// Create an epilogue instruction
				Context epilogueCtx = new Context(instructionSet, basicBlocks.EpilogueBlock);
				epilogueCtx.AppendInstruction(IRInstruction.Epilogue);
				epilogueCtx.Label = Int32.MaxValue;
			}
		}
	}
}