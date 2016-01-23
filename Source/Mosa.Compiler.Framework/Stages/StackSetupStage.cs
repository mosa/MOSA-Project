// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

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

			if (IsPlugged)
				return;

			// Create a prologue instruction
			var prologue = new Context(BasicBlocks.PrologueBlock);
			prologue.AppendInstruction(IRInstruction.Prologue);
			prologue.Label = -1;

			if (BasicBlocks.EpilogueBlock != null)
			{
				// Create an epilogue instruction
				var epilogueCtx = new Context(BasicBlocks.EpilogueBlock);
				epilogueCtx.AppendInstruction(IRInstruction.Epilogue);
				epilogueCtx.Label = BasicBlock.EpilogueLabel;
			}
		}
	}
}
