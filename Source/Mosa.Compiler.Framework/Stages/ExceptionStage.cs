// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Exception Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseMethodCompilerStage" />
	public class ExceptionStage : BaseCodeTransformationStage
	{
		private Operand exceptionRegister;

		private Operand nullOperand;

		private Operand leaveTargetRegister;

		private MosaType exceptionType;

		private Dictionary<BasicBlock, Operand> exceptionVirtualRegisters;
		private List<Tuple<BasicBlock, BasicBlock>> leaveTargets;

		private delegate void Dispatch(Context context);

		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(IRInstruction.Throw, ThrowInstruction);
			AddVisitation(IRInstruction.FinallyStart, FinallyStartInstruction);
			AddVisitation(IRInstruction.FinallyEnd, FinallyEndInstruction);
			AddVisitation(IRInstruction.ExceptionStart, ExceptionStartInstruction);
			AddVisitation(IRInstruction.SetLeaveTarget, SetLeaveTargetInstruction);
			AddVisitation(IRInstruction.GotoLeaveTarget, GotoLeaveTargetInstruction);
			AddVisitation(IRInstruction.Flow, Empty);
			AddVisitation(IRInstruction.TryStart, Empty);
			AddVisitation(IRInstruction.TryEnd, Empty);
			AddVisitation(IRInstruction.ExceptionEnd, Empty);
		}

		protected override void Initialize()
		{
			base.Initialize();

			exceptionType = TypeSystem.GetTypeByName("System", "Exception");
			exceptionRegister = Operand.CreateCPURegister(exceptionType, Architecture.ExceptionRegister);
			leaveTargetRegister = Operand.CreateCPURegister(Is32BitPlatform ? TypeSystem.BuiltIn.I4 : TypeSystem.BuiltIn.I8, Architecture.LeaveTargetRegister);

			nullOperand = Operand.GetNullObject(TypeSystem);

			exceptionVirtualRegisters = new Dictionary<BasicBlock, Operand>();
			leaveTargets = new List<Tuple<BasicBlock, BasicBlock>>();
		}

		protected override void Setup()
		{
			exceptionVirtualRegisters = new Dictionary<BasicBlock, Operand>();
			leaveTargets = new List<Tuple<BasicBlock, BasicBlock>>();

			// collect leave targets
			leaveTargets = CollectLeaveTargets();
		}

		protected override void Finish()
		{
			exceptionVirtualRegisters = null;
			leaveTargets = null;
		}

		private static void Empty(Context context)
		{
			context.Empty();
		}

		private void SetLeaveTargetInstruction(Context context)
		{
			var target = context.BranchTargets[0];

			context.SetInstruction(Select(leaveTargetRegister, IRInstruction.Move32, IRInstruction.Move64), leaveTargetRegister, CreateConstant(target.Label));
		}

		private void ExceptionStartInstruction(Context context)
		{
			var exceptionVirtualRegister = context.Result;

			context.SetInstruction(IRInstruction.KillAll);
			context.AppendInstruction(IRInstruction.Gen, exceptionRegister);
			context.AppendInstruction(Select((Operand)exceptionVirtualRegister, IRInstruction.Move32, IRInstruction.Move64), (Operand)exceptionVirtualRegister, exceptionRegister);
		}

		private void FinallyEndInstruction(Context context)
		{
			var header = FindImmediateExceptionContext(context.Label);
			var headerBlock = BasicBlocks.GetByLabel(header.HandlerStart);

			var exceptionVirtualRegister = exceptionVirtualRegisters[headerBlock];

			var newBlocks = CreateNewBlockContexts(1, context.Label);

			var nextBlock = Split(context);

			context.SetInstruction(Select(IRInstruction.CompareIntBranch32, IRInstruction.CompareIntBranch64), ConditionCode.NotEqual, null, exceptionVirtualRegister, nullOperand, newBlocks[0].Block);
			context.AppendInstruction(IRInstruction.Jmp, nextBlock.Block);

			var method = PlatformInternalRuntimeType.FindMethodByName("ExceptionHandler");

			newBlocks[0].AppendInstruction(Select(exceptionRegister, IRInstruction.Move32, IRInstruction.Move64), exceptionRegister, exceptionVirtualRegister);
			newBlocks[0].AppendInstruction(IRInstruction.CallStatic, null, Operand.CreateSymbolFromMethod(method, TypeSystem));

			MethodScanner.MethodInvoked(method, Method);
		}

		private void FinallyStartInstruction(Context context)
		{
			// Remove from header blocks
			BasicBlocks.RemoveHeaderBlock(context.Block);

			var exceptionVirtualRegister = context.Result;
			var leaveTargetVirtualRegister = context.Result2;

			exceptionVirtualRegisters.Add(context.Block, exceptionVirtualRegister);

			context.SetInstruction(IRInstruction.KillAll);
			context.AppendInstruction(IRInstruction.Gen, exceptionRegister);
			context.AppendInstruction(IRInstruction.Gen, leaveTargetRegister);

			context.AppendInstruction(Select(exceptionVirtualRegister, IRInstruction.Move32, IRInstruction.Move64), exceptionVirtualRegister, exceptionRegister);
			context.AppendInstruction(Select(leaveTargetVirtualRegister, IRInstruction.Move32, IRInstruction.Move64), leaveTargetVirtualRegister, leaveTargetRegister);
		}

		private void ThrowInstruction(Context context)
		{
			var method = PlatformInternalRuntimeType.FindMethodByName("ExceptionHandler");

			context.SetInstruction(Select(exceptionRegister, IRInstruction.Move32, IRInstruction.Move64), exceptionRegister, context.Operand1);
			context.AppendInstruction(IRInstruction.CallStatic, null, Operand.CreateSymbolFromMethod(method, TypeSystem));

			MethodScanner.MethodInvoked(method, Method);
		}

		private void GotoLeaveTargetInstruction(Context context)
		{
			// clear exception register
			// FIXME: This will need to be preserved for filtered exceptions; will need a flag to know this - maybe an upper bit of leaveTargetRegister
			context.SetInstruction(Select(exceptionRegister, IRInstruction.Move32, IRInstruction.Move64), exceptionRegister, nullOperand);

			var label = context.Label;
			var exceptionContext = FindImmediateExceptionContext(label);

			// 1) currently within a try block with a finally handler --- call it.
			if (exceptionContext.ExceptionHandlerType == ExceptionHandlerType.Finally && exceptionContext.IsLabelWithinTry(context.Label))
			{
				var handlerBlock = BasicBlocks.GetByLabel(exceptionContext.HandlerStart);

				context.AppendInstruction(IRInstruction.Jmp, handlerBlock);

				return;
			}

			// 2) else, find the next finally handler (if any), check if it should be called, if so, call it
			var nextFinallyContext = FindNextEnclosingFinallyContext(exceptionContext);

			if (nextFinallyContext != null)
			{
				var handlerBlock = BasicBlocks.GetByLabel(nextFinallyContext.HandlerStart);

				var nextBlock = Split(context);

				// compare leaveTargetRegister > handlerBlock.End, then goto finally handler
				context.AppendInstruction(Select(IRInstruction.CompareIntBranch32, IRInstruction.CompareIntBranch64), ConditionCode.GreaterThan, null, CreateConstant(handlerBlock.Label), leaveTargetRegister, nextBlock.Block); // TODO: Constant should be 64bit
				context.AppendInstruction(IRInstruction.Jmp, handlerBlock);

				context = nextBlock;
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
				context.Empty();

				var currentBlock = context.Block;
				var previousBlock = currentBlock.PreviousBlocks[0];

				var otherBranch = (previousBlock.NextBlocks[0] == currentBlock) ? previousBlock.NextBlocks[1] : previousBlock.NextBlocks[0];

				ReplaceBranchTargets(previousBlock, currentBlock, otherBranch);

				// the optimizer will remove the branch comparison
			}
			else if (targets.Count == 1)
			{
				context.AppendInstruction(IRInstruction.Jmp, targets[0]);
			}
			else
			{
				var newBlocks = CreateNewBlockContexts(targets.Count - 1, label);

				context.AppendInstruction(Select(IRInstruction.CompareIntBranch32, IRInstruction.CompareIntBranch64), ConditionCode.Equal, null, leaveTargetRegister, CreateConstant(targets[0].Label), targets[0]); // TODO: Constant should be 64bit
				context.AppendInstruction(IRInstruction.Jmp, newBlocks[0].Block);

				for (int b = 1; b < targets.Count - 2; b++)
				{
					newBlocks[b - 1].AppendInstruction(Select(IRInstruction.CompareIntBranch32, IRInstruction.CompareIntBranch64), ConditionCode.Equal, null, leaveTargetRegister, CreateConstant(targets[b].Label), targets[b]); // TODO: Constant should be 64bit
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
				var node = block.BeforeLast;

				while (node.IsEmptyOrNop
					|| node.Instruction == IRInstruction.Flow
					|| node.Instruction == IRInstruction.GotoLeaveTarget)
				{
					node = node.Previous;
				}

				if (node.Instruction == IRInstruction.SetLeaveTarget)
				{
					leaveTargets.Add(new Tuple<BasicBlock, BasicBlock>(node.BranchTargets[0], block));
				}
			}

			return leaveTargets;
		}
	}
}
