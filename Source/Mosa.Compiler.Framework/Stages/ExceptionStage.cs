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
					}
					else if (node.Instruction == IRInstruction.CallFinally)
					{
						var target = node.BranchTargets[0];
						var finallyReturn = node.BranchTargets[1];
						var ctx = new Context(node);

						ctx.SetInstruction(IRInstruction.KillAll);
						ctx.AppendInstruction(IRInstruction.Move, exceptionRegister, nullOperand);
						ctx.AppendInstruction(IRInstruction.Move, finallyReturnBlockRegister, Operand.CreateConstant(TypeSystem, finallyReturn.Label));
						ctx.AppendInstruction(IRInstruction.Jmp, target);
					}
					else if (node.Instruction == IRInstruction.FinallyStart)
					{
						// Remove from header blocks
						BasicBlocks.RemoveHeaderBlock(node.Block);

						var exceptionVirtualRegister = node.Result;
						var finallyReturnBlockVirtualRegister = node.Result2;
						var ctx = new Context(node);

						exceptionVirtualRegisters.Add(node.Block, exceptionVirtualRegister);
						finallyReturnVirtualRegisters.Add(node.Block, finallyReturnBlockRegister);

						ctx.SetInstruction(IRInstruction.KillAll);
						ctx.AppendInstruction(IRInstruction.Gen, exceptionRegister);
						ctx.AppendInstruction(IRInstruction.Gen, finallyReturnBlockRegister);

						ctx.AppendInstruction(IRInstruction.Move, exceptionVirtualRegister, exceptionRegister);
						ctx.AppendInstruction(IRInstruction.Move, finallyReturnBlockVirtualRegister, finallyReturnBlockRegister);
					}
					else if (node.Instruction == IRInstruction.FinallyEnd)
					{
						var header = FindImmediateExceptionHandler(node);
						var headerBlock = BasicBlocks.GetByLabel(header.HandlerStart);

						var exceptionVirtualRegister = exceptionVirtualRegisters[headerBlock];
						var finallyReturnBlockVirtualRegister = finallyReturnVirtualRegisters[headerBlock];

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
					else if (node.Instruction == IRInstruction.FinallyReturn)
					{
						var targets = node.BranchTargets;

						var header = FindImmediateExceptionHandler(node);
						var headerBlock = BasicBlocks.GetByLabel(header.HandlerStart);

						var finallyReturnBlockVirtualRegister = finallyReturnVirtualRegisters[headerBlock];

						Debug.Assert(targets.Count != 0);

						if (targets.Count == 1)
						{
							node.SetInstruction(IRInstruction.Jmp, targets[0]);
						}
						else
						{
							var newBlocks = CreateNewBlockContexts(targets.Count - 1);
							var ctx = new Context(node);

							ctx.SetInstruction(IRInstruction.IntegerCompareBranch, ConditionCode.Equal, null, finallyReturnBlockVirtualRegister, Operand.CreateConstant(TypeSystem, targets[0].Label), targets[0]);
							ctx.AppendInstruction(IRInstruction.Jmp, newBlocks[0].Block);

							for (int b = 1; b < targets.Count - 2; b++)
							{
								newBlocks[b - 1].AppendInstruction(IRInstruction.IntegerCompareBranch, ConditionCode.Equal, null, finallyReturnBlockVirtualRegister, Operand.CreateConstant(TypeSystem, targets[b].Label), targets[b]);
								newBlocks[b - 1].AppendInstruction(IRInstruction.Jmp, newBlocks[b + 1].Block);
							}

							newBlocks[targets.Count - 2].AppendInstruction(IRInstruction.Jmp, targets[targets.Count - 1]);
						}
					}
					else if (node.Instruction == IRInstruction.ExceptionStart)
					{
						var exceptionVirtualRegister = node.Result;
						var ctx = new Context(node);

						ctx.SetInstruction(IRInstruction.KillAll);
						ctx.AppendInstruction(IRInstruction.Gen, exceptionRegister);
						ctx.AppendInstruction(IRInstruction.Move, exceptionVirtualRegister, exceptionRegister);
					}
					else if (node.Instruction == IRInstruction.ExceptionEnd)
					{
						node.SetInstruction(IRInstruction.Jmp, node.BranchTargets[0]);
					}
					else if (node.Instruction == IRInstruction.Flow)
					{
						node.Empty();
					}
				}
			}
		}
	}
}