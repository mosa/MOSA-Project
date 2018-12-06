// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Platform;
using System.Diagnostics;

namespace Mosa.Platform.Intel.Stages
{
	/// <summary>
	/// Completes the stack handling after register allocation
	/// </summary>
	/// <seealso cref="BasePlatformTransformationStage" />
	public abstract class BuildStackStage : BasePlatformTransformationStage
	{
		#region Abstract Methods

		/// <summary>
		/// Adds the prologue instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		protected abstract void AddPrologueInstructions(Context context);

		/// <summary>
		/// Adds the epilogue instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		protected abstract void AddEpilogueInstructions(Context context);

		#endregion Abstract Methods

		protected override void PopulateVisitationDictionary()
		{
			// Nothing to do
		}

		protected override void Run()
		{
			if (IsMethodPlugged)
				return;

			Debug.Assert((MethodCompiler.StackSize % 4) == 0, "Stack size of interrupt can't be divided by 4!!");

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

			for (var node = BasicBlocks.PrologueBlock.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
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

			for (var node = BasicBlocks.EpilogueBlock.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.Instruction == IRInstruction.Epilogue)
				{
					AddEpilogueInstructions(new Context(node));
					return;
				}
			}
		}
	}
}
