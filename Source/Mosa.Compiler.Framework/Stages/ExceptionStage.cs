/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// </summary>
	public class ExceptionStage : BaseMethodCompilerStage
	{
		protected Dictionary<BasicBlock, Operand> exceptionVirtualRegisters;
		protected Dictionary<BasicBlock, Operand> finallyReturnVirtualRegisters;

		protected MosaType exceptionType;

		protected override void Run()
		{
			exceptionVirtualRegisters = new Dictionary<BasicBlock, Operand>();
			finallyReturnVirtualRegisters = new Dictionary<BasicBlock, Operand>();

			exceptionType = TypeSystem.GetTypeByName("System", "Exception");

			var exceptionRegister = Operand.CreateCPURegister(exceptionType, Architecture.ExceptionRegister);

			var finallyReturnBlockRegister = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, Architecture.FinallyReturnBlockRegister);

			var nullOperand = Operand.GetNull(TypeSystem);

			for (int i = 0; i < BasicBlocks.Count; i++)
			{
				var block = BasicBlocks[i];

				for (var ctx = new Context(block); !ctx.IsBlockEndInstruction; ctx.GotoNext())
				{
					if (ctx.IsEmpty)
						continue;

					if (ctx.Instruction == IRInstruction.Throw)
					{
						var method = PlatformInternalRuntimeType.FindMethodByName("ExceptionHandler");

						ctx.SetInstruction(IRInstruction.Move, exceptionRegister, ctx.Operand1);
						//ctx.AppendInstruction(IRInstruction.KillAllExcept, null, exceptionRegister);
						ctx.AppendInstruction(IRInstruction.Call, null, Operand.CreateSymbolFromMethod(TypeSystem, method));
						ctx.InvokeMethod = method;
					}
					else if (ctx.Instruction == IRInstruction.CallFinally)
					{
						var target = ctx.BranchTargets[0];
						var finallyReturn = ctx.BranchTargets[1];

						ctx.SetInstruction(IRInstruction.KillAll);
						ctx.AppendInstruction(IRInstruction.Move, exceptionRegister, nullOperand);
						ctx.AppendInstruction(IRInstruction.Move, finallyReturnBlockRegister, Operand.CreateConstant(TypeSystem, finallyReturn.Label));
						ctx.AppendInstruction(IRInstruction.Jmp);
						ctx.AddBranchTarget(target);
					}
					else if (ctx.Instruction == IRInstruction.FinallyStart)
					{
						// Remove from header blocks
						BasicBlocks.RemoveHeaderBlock(ctx.BasicBlock);

						var exceptionVirtualRegister = ctx.Result;
						var finallyReturnBlockVirtualRegister = ctx.Result2;

						exceptionVirtualRegisters.Add(ctx.BasicBlock, exceptionVirtualRegister);
						finallyReturnVirtualRegisters.Add(ctx.BasicBlock, finallyReturnBlockRegister);

						ctx.SetInstruction(IRInstruction.KillAll);
						ctx.AppendInstruction(IRInstruction.Gen, exceptionRegister);
						ctx.AppendInstruction(IRInstruction.Gen, finallyReturnBlockRegister);

						ctx.AppendInstruction(IRInstruction.Move, exceptionVirtualRegister, exceptionRegister);
						ctx.AppendInstruction(IRInstruction.Move, finallyReturnBlockVirtualRegister, finallyReturnBlockRegister);
					}
					else if (ctx.Instruction == IRInstruction.FinallyEnd)
					{
						var header = FindImmediateExceptionHandler(ctx.Node);
						var headerBlock = BasicBlocks.GetByLabel(header.HandlerStart);

						var exceptionVirtualRegister = exceptionVirtualRegisters[headerBlock];
						var finallyReturnBlockVirtualRegister = finallyReturnVirtualRegisters[headerBlock];

						var newBlocks = CreateNewBlockContexts(1);
						var nextBlock = Split(ctx);

						ctx.SetInstruction(IRInstruction.IntegerCompareBranch, ConditionCode.NotEqual, null, exceptionVirtualRegister, nullOperand, newBlocks[0].BasicBlock);
						ctx.AppendInstruction(IRInstruction.Jmp, nextBlock.BasicBlock);

						var method = PlatformInternalRuntimeType.FindMethodByName("ExceptionHandler");

						newBlocks[0].AppendInstruction(IRInstruction.Move, exceptionRegister, exceptionVirtualRegister);
						newBlocks[0].AppendInstruction(IRInstruction.Call, null, Operand.CreateSymbolFromMethod(TypeSystem, method));
						newBlocks[0].InvokeMethod = method;
					}
					else if (ctx.Instruction == IRInstruction.FinallyReturn)
					{
						var targets = ctx.BranchTargets;

						var header = FindImmediateExceptionHandler(ctx.Node);
						var headerBlock = BasicBlocks.GetByLabel(header.HandlerStart);

						var finallyReturnBlockVirtualRegister = finallyReturnVirtualRegisters[headerBlock];

						Debug.Assert(targets.Count != 0);

						if (targets.Count == 1)
						{
							ctx.SetInstruction(IRInstruction.Jmp, targets[0]);
						}
						else
						{
							var newBlocks = CreateNewBlockContexts(targets.Count - 1);

							ctx.SetInstruction(IRInstruction.IntegerCompareBranch, ConditionCode.Equal, null, finallyReturnBlockVirtualRegister, Operand.CreateConstant(TypeSystem, targets[0].Label), targets[0]);
							ctx.AppendInstruction(IRInstruction.Jmp, newBlocks[0].BasicBlock);

							for (int b = 1; b < targets.Count - 2; b++)
							{
								newBlocks[b - 1].AppendInstruction(IRInstruction.IntegerCompareBranch, ConditionCode.Equal, null, finallyReturnBlockVirtualRegister, Operand.CreateConstant(TypeSystem, targets[b].Label), targets[b]);
								newBlocks[b - 1].AppendInstruction(IRInstruction.Jmp, newBlocks[b + 1].BasicBlock);
							}

							newBlocks[targets.Count - 2].AppendInstruction(IRInstruction.Jmp, targets[targets.Count - 1]);
						}
					}
					else if (ctx.Instruction == IRInstruction.ExceptionStart)
					{
						var exceptionVirtualRegister = ctx.Result;

						ctx.SetInstruction(IRInstruction.KillAll);
						ctx.AppendInstruction(IRInstruction.Gen, exceptionRegister);
						ctx.AppendInstruction(IRInstruction.Move, exceptionVirtualRegister, exceptionRegister);
					}
					else if (ctx.Instruction == IRInstruction.ExceptionEnd)
					{
						ctx.SetInstruction(IRInstruction.Jmp, ctx.BranchTargets[0]);
					}
				}
			}
		}
	}
}
