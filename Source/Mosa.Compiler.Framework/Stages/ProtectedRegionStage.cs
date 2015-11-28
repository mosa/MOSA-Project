// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
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

				var context = new Context(tryBlock);

				while (context.IsEmpty || context.Instruction == IRInstruction.TryStart)
				{
					context.GotoNext();
				}

				context.AppendInstruction(IRInstruction.TryStart, tryHandler);

				context = new Context(tryHandler);

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
			var returns = new KeyedList<MosaExceptionHandler, BasicBlock>();

			foreach (var block in BasicBlocks)
			{
				var context = new Context(block.Last);

				while (context.IsEmpty || context.IsBlockEndInstruction || context.Instruction == IRInstruction.Flow)
				{
					context.GotoPrevious();
				}

				if (context.Instruction is EndFinallyInstruction) // CIL.Endfinally
				{
					context.SetInstruction(IRInstruction.FinallyEnd);

					var entry = FindFinallyHandler(context.Node);
					var list = returns.Get(entry);

					if (list == null)
					{
						// FIXME
						continue;
					}

					if (list.Count > 1)
					{
						int i = 10;
					}

					context.AppendInstruction(IRInstruction.FinallyReturn);

					foreach (var returnBlock in list)
					{
						context.AddBranchTarget(returnBlock);
					}
				}
				else if (context.Instruction is LeaveInstruction) // CIL.LeaveInstruction
				{
					bool notInExceptionHandler = false;

					// Find enclosing try or finally handler
					var entry = FindImmediateExceptionHandler(context.Node);

					if (entry.IsLabelWithinTry(context.Label))
						notInExceptionHandler = true;

					if (!notInExceptionHandler)
					{
						context.ReplaceInstructionOnly(IRInstruction.ExceptionEnd);
						continue;
					}

					var tryFinallyBlock = context.BranchTargets[0];

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
				}
			}
		}
	}
}
