/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.CIL;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// This stage inserts IR instructions related to protected regions.
	/// </summary>
	public class ProtectedRegionStage : BaseMethodCompilerStage
	{
		private KeyedList<MosaExceptionHandler, BasicBlock> returns = new KeyedList<MosaExceptionHandler, BasicBlock>();

		private MosaType exceptionType;

		protected override void Run()
		{
			if (!HasProtectedRegions)
				return;

			exceptionType = TypeSystem.GetTypeByName("System", "Exception");

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

				if (handler.HandlerType == ExceptionHandlerType.Finally)
				{
					var exceptionObject = MethodCompiler.CreateVirtualRegister(exceptionType);
					var finallyOperand = MethodCompiler.CreateVirtualRegister(TypeSystem.BuiltIn.I4);

					context.AppendInstruction2(IRInstruction.FinallyStart, exceptionObject, finallyOperand);
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
					context.SetInstruction(IRInstruction.FinallyEnd);

					var entry = FindFinallyHandler(context);
					var list = returns.Get(entry);

					if (list == null)
						return;

					context.AppendInstruction(IRInstruction.FinallyReturn);
					context.AllocateBranchTargets((uint)list.Count);

					int targetNumber = 0;
					foreach (var returnBlock in list)
					{
						context.BranchTargets[targetNumber] = returnBlock.Label;
						targetNumber++;
					}
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
						var tryFinally = context.BranchTargets[0];
						var tryFinallyBlock = BasicBlocks.GetByLabel(tryFinally);

						returns.Add(entry, tryFinallyBlock);

						context.SetInstruction(IRInstruction.TryEnd);

						if (entry.HandlerType == ExceptionHandlerType.Finally)
						{
							var finallyBlock = BasicBlocks.GetByLabel(entry.HandlerStart);

							context.AppendInstruction(IRInstruction.CallFinally, finallyBlock, tryFinallyBlock);
						}
						else
						{
							context.AppendInstruction(IRInstruction.Jmp, tryFinallyBlock);
						}

						// Fix flow control
						Debug.Assert(context.BasicBlock.NextBlocks.Count <= 1);

						//if (context.BasicBlock.NextBlocks.Count == 1)
						//{
						//	var nextBlock = context.BasicBlock.NextBlocks[0];

						//	if (!BasicBlocks.HeadBlocks.Contains(nextBlock))
						//	{
						//		BasicBlocks.AddHeaderBlock(nextBlock);
						//	}

						//	context.BasicBlock.NextBlocks.Clear();
						//	nextBlock.PreviousBlocks.Remove(context.BasicBlock);
						//}
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