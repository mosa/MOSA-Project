// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.CIL;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// This stage inserts IR instructions related to protected regions.
	/// </summary>
	public class CILProtectedRegionStage : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			if (!MethodCompiler.IsCILStream)
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
					var exceptionObject = AllocateVirtualRegister(TypeSystem.BuiltIn.Object);
					var finallyOperand = Is32BitPlatform ? AllocateVirtualRegisterI32() : AllocateVirtualRegisterI64();

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

					if (node.Instruction == CILInstruction.Leave || node.Instruction == CILInstruction.Leave_s)
					{
						var leaveBlock = node.BranchTargets[0];

						// Traverse to the header block
						var headerBlock = TraverseBackToNativeBlock(node.Block);

						// Find enclosing try or finally handler
						var exceptionHandler = FindImmediateExceptionHandler(headerBlock.Label);
						bool InTry = exceptionHandler.IsLabelWithinTry(headerBlock.Label);

						var instruction = InTry ? (BaseInstruction)IRInstruction.TryEnd : IRInstruction.ExceptionEnd;

						node.SetInstruction(instruction, leaveBlock);  // added header block
					}
					else if (node.Instruction == CILInstruction.Endfinally)
					{
						node.SetInstruction(IRInstruction.FinallyEnd);
					}
					else if (node.Instruction == CILInstruction.Throw)
					{
						node.SetInstruction(IRInstruction.Throw, node.Result, node.Operand1);
					}
				}
			}
		}
	}
}
