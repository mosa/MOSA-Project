// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
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
			// The Run() method is faster
		}

		protected override void Run()
		{
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
				if (node.Instruction != IRInstruction.Prologue)
					continue;

				if (MethodCompiler.IsStackFrameRequired)
				{
					AddPrologueInstructions(new Context(node));
				}
				else
				{
					node.Empty();
				}

				return;
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
				if (node.Instruction != IRInstruction.Epilogue)
					continue;

				if (MethodCompiler.IsStackFrameRequired)
				{
					AddEpilogueInstructions(new Context(node));
				}
				else
				{
					node.Empty();
				}

				return;
			}
		}
	}
}
