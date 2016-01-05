// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Common;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// </summary>
	public class ExceptionStage : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			var exceptionVirtualRegisters = new Dictionary<BasicBlock, Operand>();

			var exceptionType = TypeSystem.GetTypeByName("System", "Exception");

			var exceptionRegister = Operand.CreateCPURegister(exceptionType, Architecture.ExceptionRegister);

			var leaveTargetRegister = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, Architecture.LeaveTargetRegister);

			var nullOperand = Operand.GetNull(TypeSystem);

			// collect leave targets
			var leaveTargets = CollectLeaveTargets();

			for (int i = 0; i < BasicBlocks.Count; i++)
			{
				var block = BasicBlocks[i];

				for (var node = block.First; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					if (node.Instruction == IRInstruction.Throw)
					{
						var method = PlatformInternalRuntimeType.FindMethodByName("ExceptionHandler");
						var ctx = new Context(node);

						ctx.SetInstruction(IRInstruction.Move, exceptionRegister, node.Operand1);

						//ctx.AppendInstruction(IRInstruction.KillAllExcept, null, exceptionRegister);
						ctx.AppendInstruction(IRInstruction.Call, null, Operand.CreateSymbolFromMethod(TypeSystem, method));
						ctx.InvokeMethod = method;
						continue;
					}
					else if (node.Instruction == IRInstruction.FinallyStart)
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
						continue;
					}
					else if (node.Instruction == IRInstruction.FinallyEnd)
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
						continue;
					}
					else if (node.Instruction == IRInstruction.ExceptionStart)
					{
						var exceptionVirtualRegister = node.Result;
						var ctx = new Context(node);

						ctx.SetInstruction(IRInstruction.KillAll);
						ctx.AppendInstruction(IRInstruction.Gen, exceptionRegister);
						ctx.AppendInstruction(IRInstruction.Move, exceptionVirtualRegister, exceptionRegister);
						continue;
					}
					else if (node.Instruction == IRInstruction.SetLeaveTarget)
					{
						var target = node.BranchTargets[0];

						var ctx = new Context(node);

						ctx.SetInstruction(IRInstruction.Move, leaveTargetRegister, Operand.CreateConstant(TypeSystem, target.Label));
					}
					else if (node.Instruction == IRInstruction.GotoLeaveTarget)
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

							continue;
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

						//if (targets.Count == 0)
						//{
						//	MethodCompiler.Stop();
						//}

						Debug.Assert(targets.Count != 0);

						if (targets.Count == 1)
						{
							ctx.AppendInstruction(IRInstruction.Jmp, targets[0]);
							continue;
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

						continue;
					}
					else if (node.Instruction == IRInstruction.Flow)
					{
						node.Empty();
					}
					else if (node.Instruction == IRInstruction.TryStart)
					{
						node.Empty();
					}
					else if (node.Instruction == IRInstruction.TryEnd)
					{
						node.Empty();
					}
					else if (node.Instruction == IRInstruction.ExceptionEnd)
					{
						node.Empty();
						continue;
					}
				}
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
