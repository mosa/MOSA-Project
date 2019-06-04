// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Analysis;
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

		protected override void Initialize()
		{
			exceptionType = TypeSystem.GetTypeByName("System", "Exception");
		}

		protected override void Run()
		{
			if (!MethodCompiler.IsCILDecodeRequired)
				return;

			InsertBlockProtectInstructions();
			UpdateBlockProtectInstructions();

			MethodCompiler.ProtectedRegions = ProtectedRegion.CreateProtectedRegions(BasicBlocks, Method.ExceptionHandlers);
		}

		private void InsertBlockProtectInstructions()
		{
			foreach (var handler in Method.ExceptionHandlers)
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
					var exceptionObject = AllocateVirtualRegister(exceptionType);
					var finallyOperand = AllocateVirtualRegister(Is32BitPlatform ? TypeSystem.BuiltIn.I4 : TypeSystem.BuiltIn.I8);

					context.AppendInstruction2(IRInstruction.FinallyStart, exceptionObject, finallyOperand);
				}
			}
		}

		private void UpdateBlockProtectInstructions()
		{
			foreach (var block in BasicBlocks)
			{
				for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					if (node.Instruction is CIL.LeaveInstruction) // CIL.LeaveInstruction
					{
						var leaveBlock = node.BranchTargets[0];

						// Find enclosing try or finally handler
						var exceptionContext = FindImmediateExceptionContext(node.Label);

						bool InTryContext = exceptionContext.IsLabelWithinTry(node.Label);

						var ctx = new Context(node);

						if (!InTryContext)
						{
							// Within exception handler
							ctx.SetInstruction(IRInstruction.ExceptionEnd);
						}
						else
						{
							ctx.SetInstruction(IRInstruction.TryEnd);
						}
						ctx.AppendInstruction(IRInstruction.SetLeaveTarget, leaveBlock);
						ctx.AppendInstruction(IRInstruction.GotoLeaveTarget);
					}
					else if (node.Instruction is CIL.EndFinallyInstruction) // CIL.Endfinally
					{
						var ctx = new Context(node);

						ctx.SetInstruction(IRInstruction.FinallyEnd);
						ctx.AppendInstruction(IRInstruction.GotoLeaveTarget);
					}
					else if (node.Instruction is CIL.ThrowInstruction) // CIL.Throw
					{
						node.SetInstruction(IRInstruction.Throw, node.Result, node.Operand1);
					}
				}
			}
		}
	}
}
