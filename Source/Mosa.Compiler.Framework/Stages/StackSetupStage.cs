// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Stack Setup Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseMethodCompilerStage" />
	public sealed class StackSetupStage : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			if (!MethodCompiler.IsStackFrameRequired)
				return;

			// Create a prologue instruction
			var prologue = new Context(BasicBlocks.PrologueBlock);
			prologue.AppendInstruction(IRInstruction.Prologue);
			prologue.Label = -1;

			if (BasicBlocks.EpilogueBlock != null)
			{
				// Create an epilogue instruction
				var epilogue = new Context(BasicBlocks.EpilogueBlock);
				epilogue.AppendInstruction(IRInstruction.Epilogue);
				epilogue.Label = BasicBlock.EpilogueLabel;
			}
		}
	}
}
