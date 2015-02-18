/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using Mosa.Compiler.Framework.IR;

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// This stage inserts the IR ExceptionStart instructions at the beginning of each exception block and
	/// IR Flow instructions when necessary after leave instructions
	/// </summary>
	public class ExceptionPrologueStage : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			InsertExceptionStartInstructions();
			InsertFlowOrJumpInstructions();
		}

		private void InsertExceptionStartInstructions()
		{
			foreach (var clause in MethodCompiler.Method.ExceptionHandlers)
			{
				if (clause.HandlerType == ExceptionHandlerType.Exception)
				{
					var tryHandler = BasicBlocks.GetByLabel(clause.HandlerStart);

					var context = new Context(tryHandler);

					var exceptionObject = MethodCompiler.CreateVirtualRegister(clause.Type);

					context.AppendInstruction(IRInstruction.ExceptionStart, exceptionObject);
				}
			}
		}

		private void InsertFlowOrJumpInstructions()
		{
			foreach (var block in BasicBlocks)
			{
				for (var node = block.Last.Previous; !node.IsBlockStartInstruction; node = node.Previous)
				{
					if (node.IsEmpty)
						continue;

					if (!(node.Instruction is CIL.LeaveInstruction))
						continue;

					var target = node.BranchTargets[0];

					if (IsLeaveAndTargetWithinTry(node))
					{
						node.SetInstruction(IRInstruction.Jmp, target);

						BasicBlocks.RemoveHeaderBlock(target);

						continue;
					}

					var entry = FindImmediateExceptionHandler(node);

					if (entry == null)
						break;

					if (!entry.IsLabelWithinTry(node.Label))
						break;

					var flowNode = new InstructionNode(IRInstruction.Flow, target);

					node.Insert(flowNode);

					if (BasicBlocks.IsHeaderBlock(target))
					{
						BasicBlocks.RemoveHeaderBlock(target);
					}

					break;
				}
			}
		}
	}
}
