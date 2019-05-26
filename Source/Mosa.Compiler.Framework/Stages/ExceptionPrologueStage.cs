// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
		protected MosaType objectType;

		protected override void Initialize()
		{
			objectType = TypeSystem.GetTypeByName("System", "Object");
		}

		protected override void Run()
		{
			InsertExceptionStartInstructions();
			InsertFlowOrJumpInstructions();
		}

		private void InsertExceptionStartInstructions()
		{
			foreach (var clause in MethodCompiler.Method.ExceptionHandlers)
			{
				if (clause.ExceptionHandlerType == ExceptionHandlerType.Exception)
				{
					var handler = BasicBlocks.GetByLabel(clause.HandlerStart);

					var exceptionObject = AllocateVirtualRegister(clause.Type);

					var context = new Context(handler);

					context.AppendInstruction(IRInstruction.ExceptionStart, exceptionObject);
				}

				if (clause.ExceptionHandlerType == ExceptionHandlerType.Filter)
				{
					{
						var handler = BasicBlocks.GetByLabel(clause.HandlerStart);

						var exceptionObject = AllocateVirtualRegister(objectType);

						var context = new Context(handler);

						context.AppendInstruction(IRInstruction.ExceptionStart, exceptionObject);
					}

					{
						var handler = BasicBlocks.GetByLabel(clause.FilterStart.Value);

						var exceptionObject = AllocateVirtualRegister(objectType);

						var context = new Context(handler);

						context.AppendInstruction(IRInstruction.FilterStart, exceptionObject);
					}
				}
			}
		}

		private void InsertFlowOrJumpInstructions()
		{
			foreach (var block in BasicBlocks)
			{
				for (var node = block.BeforeLast; !node.IsBlockStartInstruction; node = node.Previous)
				{
					if (node.IsEmptyOrNop)
						continue;

					if (!(node.Instruction is CIL.LeaveInstruction))
						continue;   // FUTURE: Could this be a break instruction instead?

					var target = node.BranchTargets[0];

					if (IsSourceAndTargetWithinSameTryOrException(node))
					{
						// Leave instruction can be converted into a simple jump instruction
						node.SetInstruction(IRInstruction.Jmp, target);
						BasicBlocks.RemoveHeaderBlock(target);
						continue;
					}

					var entry = FindImmediateExceptionContext(node.Label);

					if (!entry.IsLabelWithinTry(node.Label))
						break;

					var flowNode = new InstructionNode(IRInstruction.Flow, target);

					node.Insert(flowNode);

					if (target.IsHeadBlock)
					{
						BasicBlocks.RemoveHeaderBlock(target);
					}

					break;
				}
			}
		}
	}
}
