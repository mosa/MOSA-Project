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

				if (handler.ExceptionHandlerType == ExceptionHandlerType.Finally)
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
				var context = new Context(block.Last);

				while (context.IsEmpty || context.IsBlockEndInstruction || context.Instruction == IRInstruction.Flow)
				{
					context.GotoPrevious();
				}

				if (context.Instruction is LeaveInstruction) // CIL.LeaveInstruction
				{
					var leaveBlock = context.BranchTargets[0];

					// Find enclosing try or finally handler
					var exceptionContext = FindImmediateExceptionContext(context.Label);

					bool InTryContext = exceptionContext.IsLabelWithinTry(context.Label);

					if (!InTryContext)
					{
						// Within try exception context
						context.ReplaceInstructionOnly(IRInstruction.ExceptionEnd);
						continue;
					}

					// Within try context
					context.SetInstruction(IRInstruction.TryEnd);

					if (exceptionContext.ExceptionHandlerType == ExceptionHandlerType.Finally)
					{
						//var finallyBlock = BasicBlocks.GetByLabel(exceptionContext.HandlerStart);

						context.AppendInstruction(IRInstruction.CallFinally, leaveBlock);
					}
					else
					{
						// Leave within exception goes directly to the leave target
						context.AppendInstruction(IRInstruction.Jmp, leaveBlock);
					}
				}
				else if (context.Instruction is EndFinallyInstruction) // CIL.Endfinally
				{
					context.SetInstruction(IRInstruction.FinallyEnd);
					context.AppendInstruction(IRInstruction.Leave);
				}
			}
		}
	}
}
