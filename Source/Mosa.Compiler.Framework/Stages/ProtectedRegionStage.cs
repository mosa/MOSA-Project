/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using Mosa.Compiler.Framework.Analysis;
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
			if (!HasProtectedRegions)
				return;

			InsertBlockProtectInstructions();
			UpdateBlockProtectInstructions();

			MethodCompiler.SetProtectedRegions(ProtectedRegion.CreateProtectedRegions(BasicBlocks, MethodCompiler.Method.ExceptionHandlers));
		}

		private void InsertBlockProtectInstructions()
		{
			foreach (var handler in MethodCompiler.Method.ExceptionHandlers)
			{
				var tryBlock = BasicBlocks.GetByLabel(handler.TryStart);

				var tryHandler = BasicBlocks.GetByLabel(handler.HandlerStart);

				var context = new Context(InstructionSet, tryBlock);

				while (context.IsEmpty || context.Instruction == IRInstruction.TryStart)
				{
					context.GotoNext();
				}

				context.AppendInstruction(IRInstruction.TryStart, tryHandler);

				context = new Context(InstructionSet, tryHandler);

				if (handler.HandlerType == ExceptionHandlerType.Exception)
				{
					var exceptionObject = MethodCompiler.CreateVirtualRegister(handler.Type);

					context.AppendInstruction(IRInstruction.ExceptionStart, exceptionObject);
				}
				else if (handler.HandlerType == ExceptionHandlerType.Finally)
				{
					context.AppendInstruction(IRInstruction.FinallyStart);
				}
			}
		}

		private void UpdateBlockProtectInstructions()
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

					var entry = FindImmediateExceptionHandler(context);

					if (entry != null)
					{
						if (entry.IsLabelWithinTry(context.Label))
							createLink = true;
					}

					if (createLink)
					{
						var tryEndNext = context.BranchTargets[0];
						var tryEndNextBlock = BasicBlocks.GetByLabel(tryEndNext);

						if (entry.HandlerType == ExceptionHandlerType.Finally)
						{
							var finallyBlock = BasicBlocks.GetByLabel(entry.HandlerStart);

							context.SetInstruction(IRInstruction.CallFinally, finallyBlock);
							context.AppendInstruction(IRInstruction.TryEnd);
						}
						else
						{
							context.SetInstruction(IRInstruction.TryEnd);
						}
						context.AppendInstruction(IRInstruction.Jmp, tryEndNextBlock);
					}
					else
					{
						context.ReplaceInstructionOnly(IRInstruction.ExceptionEnd);
					}
				}
			}
		}

		//private List<ProtectRegionBlockInfo> ProtectedRegionBlocks = new List<ProtectRegionBlockInfo>();
	}
}