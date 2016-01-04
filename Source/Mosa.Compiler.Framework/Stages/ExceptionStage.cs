// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
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

			var finallyReturnBlockRegister = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, Architecture.FinallyReturnBlockRegister);

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
						var finallyReturnBlockVirtualRegister = node.Result2;
						var ctx = new Context(node);

						exceptionVirtualRegisters.Add(node.Block, exceptionVirtualRegister);

						ctx.SetInstruction(IRInstruction.KillAll);
						ctx.AppendInstruction(IRInstruction.Gen, exceptionRegister);
						ctx.AppendInstruction(IRInstruction.Gen, finallyReturnBlockRegister);

						ctx.AppendInstruction(IRInstruction.Move, exceptionVirtualRegister, exceptionRegister);
						ctx.AppendInstruction(IRInstruction.Move, finallyReturnBlockVirtualRegister, finallyReturnBlockRegister);
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
					else if (node.Instruction == IRInstruction.LeaveTarget)
					{
						var target = node.BranchTargets[0];

						var ctx = new Context(node);

						ctx.SetInstruction(IRInstruction.Move, finallyReturnBlockRegister, Operand.CreateConstant(TypeSystem, target.Label));
					}
					else if (node.Instruction == IRInstruction.Leave)
					{
						var label = node.Label;
						var exceptionContext = FindImmediateExceptionContext(label);

						var nextFinallyContext = (exceptionContext.ExceptionHandlerType == ExceptionHandlerType.Finally && exceptionContext.IsLabelWithinTry(node.Label))
							? exceptionContext : FindNextEnclosingFinallyContext(exceptionContext);

						var ctx = new Context(node);

						// clear exception register
						ctx.SetInstruction(IRInstruction.Move, exceptionRegister, nullOperand);

						if (nextFinallyContext != null)
						{
							var handlerBlock = BasicBlocks.GetByLabel(nextFinallyContext.HandlerStart);

							var nextBlock = Split(ctx);

							// compare finallyReturnBlockRegister > handlerBlock.End, then goto handler
							ctx.AppendInstruction(IRInstruction.IntegerCompareBranch, ConditionCode.GreaterThan, null, Operand.CreateConstant(TypeSystem, handlerBlock.Label), finallyReturnBlockRegister, nextBlock.Block);
							ctx.AppendInstruction(IRInstruction.Jmp, handlerBlock);

							ctx = nextBlock;
						}

						var targets = new List<BasicBlock>();

						foreach (var target in leaveTargets)
						{
							// Leave target must be after end of exception context
							// Leave target must be found within try or handler

							if (target.Item1.Label > exceptionContext.TryEnd
								&& (exceptionContext.IsLabelWithinTry(target.Item2.Label) || exceptionContext.IsLabelWithinHandler(target.Item2.Label)))
							{
								targets.Add(target.Item1);
							}
						}

						Debug.Assert(targets.Count != 0);

						if (targets.Count == 1)
						{
							ctx.AppendInstruction(IRInstruction.Jmp, targets[0]);
							continue;
						}
						else
						{
							var newBlocks = CreateNewBlockContexts(targets.Count - 1);

							ctx.AppendInstruction(IRInstruction.IntegerCompareBranch, ConditionCode.Equal, null, finallyReturnBlockRegister, Operand.CreateConstant(TypeSystem, targets[0].Label), targets[0]);
							ctx.AppendInstruction(IRInstruction.Jmp, newBlocks[0].Block);

							for (int b = 1; b < targets.Count - 2; b++)
							{
								newBlocks[b - 1].AppendInstruction(IRInstruction.IntegerCompareBranch, ConditionCode.Equal, null, finallyReturnBlockRegister, Operand.CreateConstant(TypeSystem, targets[b].Label), targets[b]);
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

				while (context.IsEmpty || context.IsBlockEndInstruction || context.Instruction == IRInstruction.Flow || context.Instruction == IRInstruction.Leave)
				{
					context.GotoPrevious();
				}

				if (context.Instruction == IRInstruction.LeaveTarget)
				{
					leaveTargets.Add(new Tuple<BasicBlock, BasicBlock>(context.BranchTargets[0], block));
				}
			}

			return leaveTargets;
		}
	}
}
