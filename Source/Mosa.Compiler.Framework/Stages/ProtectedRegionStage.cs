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
	/// This stage inserts IR instructions related to protected regions.
	/// </summary>
	public class ProtectedRegionStage : BaseMethodCompilerStage
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
			foreach (var entry in MethodCompiler.Method.ExceptionBlocks)
			{
				var tryBlock = BasicBlocks.GetByLabel(entry.TryStart);

				var tryHandler = BasicBlocks.GetByLabel(entry.HandlerStart);

				var context = new Context(InstructionSet, tryBlock);

				while (context.IsEmpty || context.Instruction == IRInstruction.TryStart)
				{
					context.GotoNext();
				}

				context.AppendInstruction(IRInstruction.TryStart, tryHandler);

				context = new Context(InstructionSet, tryHandler);

				if (entry.HandlerType == ExceptionHandlerType.Exception)
				{
					var exceptionObject = MethodCompiler.CreateVirtualRegister(entry.Type);

					context.AppendInstruction(IRInstruction.ExceptionStart, exceptionObject);
				}
				else if (entry.HandlerType == ExceptionHandlerType.Finally)
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
						var tryEndNext = context.BranchTargets[0];
						var tryEndNextBlock = BasicBlocks.GetByLabel(tryEndNext);

						var finallyBlock = BasicBlocks.GetByLabel(entry.HandlerStart);

						context.SetInstruction(IRInstruction.CallFinally, finallyBlock);
						context.AppendInstruction(IRInstruction.TryEnd);
						context.AppendInstruction(IRInstruction.Jmp, tryEndNextBlock);
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