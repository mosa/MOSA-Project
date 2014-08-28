/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using Mosa.Compiler.Framework.CIL;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// This stage inserts IR instructions related to protected blocks.
	/// </summary>
	public class ProtectedBlockStage : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			if (!HasExceptionOrFinally)
				return;

			MethodCompiler.CreateExceptionReturnOperands();

			InsertBlockProtectInstructions();
			UpdateBlockProtectInstructions();
		}

		private void InsertBlockProtectInstructions()
		{
			foreach (var clause in MethodCompiler.Method.ExceptionBlocks)
			{
				var tryBlock = BasicBlocks.GetByLabel(clause.TryStart);

				var tryFilter = BasicBlocks.GetByLabel(clause.HandlerStart);

				var context = new Context(InstructionSet, tryBlock);

				while (context.IsEmpty || context.Instruction == IRInstruction.TryStart)
				{
					context.GotoNext();
				}

				context.AppendInstruction(IRInstruction.TryStart, tryFilter);

				// find handler block
				var handlerBlock = BasicBlocks.GetByLabel(clause.HandlerStart);

				context = new Context(InstructionSet, handlerBlock);

				if (clause.HandlerType == ExceptionHandlerType.Exception)
				{
					var exceptionObject = MethodCompiler.CreateVirtualRegister(clause.Type);

					context.AppendInstruction(IRInstruction.ExceptionStart, exceptionObject);
				}
				else if (clause.HandlerType == ExceptionHandlerType.Finally)
				{
					context.AppendInstruction(IRInstruction.FinallyStart);
				}
			}
		}

		public void UpdateBlockProtectInstructions()
		{
			foreach (var block in BasicBlocks)
			{
				var context = new Context(InstructionSet, block, block.EndIndex);

				while (context.IsEmpty || context.IsBlockEndInstruction)
				{
					context.GotoPrevious();
				}

				if (context.Instruction is EndFinallyInstruction)
				{
					context.ReplaceInstructionOnly(IRInstruction.FinallyEnd);
				}
				else if (context.Instruction is LeaveInstruction)
				{
					// Find enclosing finally clause
					bool createLink = false;

					var entry = FindImmediateExceptionEntry(context);

					if (entry != null)
					{
						if (entry.IsLabelWithinTry(context.Label))
							createLink = true;
					}

					if (createLink)
					{
						var finallyBlock = BasicBlocks.GetByLabel(context.BranchTargets[0]);

						context.ReplaceInstructionOnly(IRInstruction.TryEnd);
						context.AppendInstruction(IRInstruction.Jmp, finallyBlock);
						//LinkBlocks(context, finallyBlock);
					}
					else
					{
						context.ReplaceInstructionOnly(IRInstruction.ExceptionEnd);
					}
				}
			}
		}
	}
}