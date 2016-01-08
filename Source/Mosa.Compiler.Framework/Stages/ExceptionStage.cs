// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Common;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;
using System;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// </summary>
	public class ExceptionStage : BaseMethodCompilerStage
	{
		private Dictionary<BasicBlock, Operand> exceptionVirtualRegisters = new Dictionary<BasicBlock, Operand>();

		private Operand exceptionRegister;

		private Operand nullOperand;

		private Operand leaveTargetRegister;

		private MosaType exceptionType;

		private List<Tuple<BasicBlock, BasicBlock>> leaveTargets;

		private delegate void Dispatch(InstructionNode node);

		protected override void Run()
		{
			exceptionType = TypeSystem.GetTypeByName("System", "Exception");

			exceptionRegister = Operand.CreateCPURegister(exceptionType, Architecture.ExceptionRegister);

			leaveTargetRegister = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, Architecture.LeaveTargetRegister);

			nullOperand = Operand.GetNull(TypeSystem);

			// collect leave targets
			leaveTargets = CollectLeaveTargets();

			var dispatches = new Dictionary<BaseInstruction, Dispatch>();

			dispatches.Add(IRInstruction.Throw, ThrowInstruction);
			dispatches.Add(IRInstruction.FinallyStart, FinallyStartInstruction);
			dispatches.Add(IRInstruction.FinallyEnd, FinallyEndInstruction);
			dispatches.Add(IRInstruction.ExceptionStart, ExceptionStartInstruction);
			dispatches.Add(IRInstruction.SetLeaveTarget, SetLeaveTargetInstruction);
			dispatches.Add(IRInstruction.GotoLeaveTarget, GotoLeaveTargetInstruction);
			dispatches.Add(IRInstruction.Flow, Empty);
			dispatches.Add(IRInstruction.TryStart, Empty);
			dispatches.Add(IRInstruction.TryEnd, Empty);
			dispatches.Add(IRInstruction.ExceptionEnd, Empty);

			for (int i = 0; i < BasicBlocks.Count; i++)
			{
				for (var node = BasicBlocks[i].First; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					Dispatch dispatch;

					if (dispatches.TryGetValue(node.Instruction, out dispatch))
					{
						dispatch.Invoke(node);
					}
				}
			}
		}

		private static void Empty(InstructionNode node)
		{
			node.Empty();
		}

		protected override void Finish()
		{
			exceptionVirtualRegisters = null;
		}

		private void SetLeaveTargetInstruction(InstructionNode node)
		{
			var target = node.BranchTargets[0];

			var ctx = new Context(node);

			ctx.SetInstruction(IRInstruction.Move, leaveTargetRegister, Operand.CreateConstant(TypeSystem, target.Label));
		}

		private void ExceptionStartInstruction(InstructionNode node)
		{
			var exceptionVirtualRegister = node.Result;
			var ctx = new Context(node);

			ctx.SetInstruction(IRInstruction.KillAll);
			ctx.AppendInstruction(IRInstruction.Gen, exceptionRegister);
			ctx.AppendInstruction(IRInstruction.Move, exceptionVirtualRegister, exceptionRegister);
		}

		private void FinallyEndInstruction(InstructionNode node)
		{
			var header = FindImmediateExceptionContext(node.Label);
			var headerBlock = BasicBlocks.GetByLabel(header.HandlerStart);

			var exceptionVirtualRegister = exceptionVirtualRegisters[headerBlock];

			var newBlocks = CreateNewBlockContexts(1);
			var ctx = new Context(node);
			var nextBlock = Split(ctx);

			ctx.SetInstruction(IRInstruction.IntegerCompareBranch, ConditionCode.NotEqual, null, exceptionVirtualRegister, nullOperand, newBlocks[0].Block);
			ctx.AppendInstruction(IRInstruction.Jmp, nextBlock.Block);

			var method = PlatformInternalRuntimeType.FindMethodByName("ExceptionHandler");

			newBlocks[0].AppendInstruction(IRInstruction.Move, exceptionRegister, exceptionVirtualRegister);
			newBlocks[0].AppendInstruction(IRInstruction.Call, null, Operand.CreateSymbolFromMethod(TypeSystem, method));
			newBlocks[0].InvokeMethod = method;
		}

		private void FinallyStartInstruction(InstructionNode node)
		{
			// Remove from header blocks
			BasicBlocks.RemoveHeaderBlock(node.Block);

			var exceptionVirtualRegister = node.Result;
			var leaveTargetVirtualRegister = node.Result2;
			var ctx = new Context(node);

			exceptionVirtualRegisters.Add(node.Block, exceptionVirtualRegister);

			ctx.SetInstruction(IRInstruction.KillAll);
			ctx.AppendInstruction(IRInstruction.Gen, exceptionRegister);
			ctx.AppendInstruction(IRInstruction.Gen, leaveTargetRegister);

			ctx.AppendInstruction(IRInstruction.Move, exceptionVirtualRegister, exceptionRegister);
			ctx.AppendInstruction(IRInstruction.Move, leaveTargetVirtualRegister, leaveTargetRegister);
		}

		private void ThrowInstruction(InstructionNode node)
		{
			var method = PlatformInternalRuntimeType.FindMethodByName("ExceptionHandler");
			var ctx = new Context(node);

			ctx.SetInstruction(IRInstruction.Move, exceptionRegister, node.Operand1);

			//ctx.AppendInstruction(IRInstruction.KillAllExcept, null, exceptionRegister);
			ctx.AppendInstruction(IRInstruction.Call, null, Operand.CreateSymbolFromMethod(TypeSystem, method));
			ctx.InvokeMethod = method;
		}

		private void GotoLeaveTargetInstruction(InstructionNode node)
		{
			var ctx = new Context(node);

			// clear exception register
			// FIXME: This will need to be preserved for filtered exceptions; will need a flag to know this - maybe an upper bit of leaveTargetRegister
			ctx.SetInstruction(IRInstruction.Move, exceptionRegister, nullOperand);

			var label = node.Label;
			var exceptionContext = FindImmediateExceptionContext(label);

			// 1) currently within a try block with a finally handler --- call it.
			if (exceptionContext.ExceptionHandlerType == ExceptionHandlerType.Finally && exceptionContext.IsLabelWithinTry(node.Label))
			{
				var handlerBlock = BasicBlocks.GetByLabel(exceptionContext.HandlerStart);

				ctx.AppendInstruction(IRInstruction.Jmp, handlerBlock);

				return;
			}

			// 2) else, find the next finally handler (if any), check if it should be called, if so, call it
			var nextFinallyContext = FindNextEnclosingFinallyContext(exceptionContext);

			if (nextFinallyContext != null)
			{
				var handlerBlock = BasicBlocks.GetByLabel(nextFinallyContext.HandlerStart);

				var nextBlock = Split(ctx);

				// compare leaveTargetRegister > handlerBlock.End, then goto finally handler
				ctx.AppendInstruction(IRInstruction.IntegerCompareBranch, ConditionCode.GreaterThan, null, Operand.CreateConstant(TypeSystem, handlerBlock.Label), leaveTargetRegister, nextBlock.Block);
				ctx.AppendInstruction(IRInstruction.Jmp, handlerBlock);

				ctx = nextBlock;
			}

			// find all the available targets within the method from this node's location
			var targets = new List<BasicBlock>();

			// using the end of the protected as the location
			var location = exceptionContext.TryEnd;

			foreach (var targetBlock in leaveTargets)
			{
				var source = targetBlock.Item2;
				var target = targetBlock.Item1;

				// target must be after end of exception context
				if (target.Label <= location)
					continue;

				// target must be found within try or handler
				if (exceptionContext.IsLabelWithinTry(source.Label) || exceptionContext.IsLabelWithinHandler(source.Label))
				{
					targets.AddIfNew(target);
				}
			}

			if (targets.Count == 0)
			{
				// this is an unreachable location

				// clear this block --- should only have on instruction
				ctx.Empty();

				var currentBlock = ctx.Block;
				var previousBlock = currentBlock.PreviousBlocks[0];

				var otherBranch = (previousBlock.NextBlocks[0] == currentBlock) ? previousBlock.NextBlocks[1] : previousBlock.NextBlocks[0];

				ReplaceBranchTargets(previousBlock, currentBlock, otherBranch);

				// the optimizer will remove the branch comparison

				return;
			}

			if (targets.Count == 1)
			{
				ctx.AppendInstruction(IRInstruction.Jmp, targets[0]);
				return;
			}
			else
			{
				var newBlocks = CreateNewBlockContexts(targets.Count - 1);

				ctx.AppendInstruction(IRInstruction.IntegerCompareBranch, ConditionCode.Equal, null, leaveTargetRegister, Operand.CreateConstant(TypeSystem, targets[0].Label), targets[0]);
				ctx.AppendInstruction(IRInstruction.Jmp, newBlocks[0].Block);

				for (int b = 1; b < targets.Count - 2; b++)
				{
					newBlocks[b - 1].AppendInstruction(IRInstruction.IntegerCompareBranch, ConditionCode.Equal, null, leaveTargetRegister, Operand.CreateConstant(TypeSystem, targets[b].Label), targets[b]);
					newBlocks[b - 1].AppendInstruction(IRInstruction.Jmp, newBlocks[b + 1].Block);
				}

				newBlocks[targets.Count - 2].AppendInstruction(IRInstruction.Jmp, targets[targets.Count - 1]);
			}
		}

		private List<Tuple<BasicBlock, BasicBlock>> CollectLeaveTargets()
		{
			var leaveTargets = new List<Tuple<BasicBlock, BasicBlock>>();

			foreach (var block in BasicBlocks)
			{
				var context = new Context(block.Last);

				while (context.IsEmpty || context.IsBlockEndInstruction || context.Instruction == IRInstruction.Flow || context.Instruction == IRInstruction.GotoLeaveTarget)
				{
					context.GotoPrevious();
				}

				if (context.Instruction == IRInstruction.SetLeaveTarget)
				{
					leaveTargets.Add(new Tuple<BasicBlock, BasicBlock>(context.BranchTargets[0], block));
				}
			}

			return leaveTargets;
		}
	}
}
