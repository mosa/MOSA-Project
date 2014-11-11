/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework.IR;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// </summary>
	public class ExceptionStage : BaseMethodCompilerStage
	{
		protected Dictionary<BasicBlock, Operand> exceptionRegisters;

		protected override void Run()
		{
			if (!HasProtectedRegions)
				return;

			var exceptionRegister = Operand.CreateCPURegister(TypeLayout.TypeSystem.BuiltIn.Pointer, Architecture.ExceptionRegister);

			var nullOperand = Operand.GetNull(TypeSystem);

			CreateExceptionReturnOperands();

			for (int i = 0; i < BasicBlocks.Count; i++)
			{
				var block = BasicBlocks[i];

				for (var ctx = new Context(InstructionSet, block); !ctx.IsBlockEndInstruction; ctx.GotoNext())
				{
					if (ctx.IsEmpty)
						continue;

					if (ctx.Instruction == IRInstruction.Throw)
					{
						var method = PlatformInternalRuntimeType.FindMethodByName("ExceptionHandler");

						ctx.SetInstruction(IRInstruction.Move, exceptionRegister, ctx.Operand1);
						ctx.AppendInstruction(IRInstruction.Call, null, Operand.CreateSymbolFromMethod(TypeSystem, method));
						ctx.MosaMethod = method;
					}
					else if (ctx.Instruction == IRInstruction.CallFinally)
					{
						var target = ctx.BranchTargets[0];

						ctx.SetInstruction(IRInstruction.KillAll);
						ctx.AppendInstruction(IRInstruction.Move, exceptionRegister, nullOperand);
						ctx.AppendInstruction(IRInstruction.InternalCall);
						ctx.SetBranch(target);
					}
					else if (ctx.Instruction == IRInstruction.FinallyStart)
					{
						var header = FindImmediateExceptionHandler(ctx);
						var headerBlock = BasicBlocks.GetByLabel(header.HandlerStart);

						var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.Pointer);

						ctx.SetInstruction(IRInstruction.KillAll);
						ctx.AppendInstruction(IRInstruction.Gen, exceptionRegister);
						ctx.AppendInstruction(IRInstruction.Move, exceptionRegisters[headerBlock], exceptionRegister);
					}
					else if (ctx.Instruction == IRInstruction.FinallyEnd)
					{
						var header = FindImmediateExceptionHandler(ctx);
						var headerBlock = BasicBlocks.GetByLabel(header.HandlerStart);

						var newBlocks = CreateNewBlocksWithContexts(2);

						ctx.SetInstruction(IRInstruction.Move, exceptionRegister, exceptionRegisters[headerBlock]);
						ctx.AppendInstruction(IRInstruction.IntegerCompareBranch, ConditionCode.Equal, null, exceptionRegister, nullOperand);
						ctx.SetBranch(newBlocks[0].BasicBlock);
						ctx.AppendInstruction(IRInstruction.Jmp, newBlocks[1].BasicBlock);
						LinkBlocks(ctx, newBlocks[0]);
						LinkBlocks(ctx, newBlocks[1]);

						newBlocks[0].AppendInstruction(IRInstruction.InternalReturn);

						var method = PlatformInternalRuntimeType.FindMethodByName("ExceptionHandler");

						newBlocks[1].AppendInstruction(IRInstruction.Call, null, Operand.CreateSymbolFromMethod(TypeSystem, method));
						newBlocks[1].MosaMethod = method;
					}
					else if (ctx.Instruction == IRInstruction.ExceptionStart)
					{
						ctx.SetInstruction(IRInstruction.KillAll);
						ctx.AppendInstruction(IRInstruction.Gen, exceptionRegister);
						ctx.AppendInstruction(IRInstruction.Move, ctx.Result, exceptionRegister);
					}
					else if (ctx.Instruction == IRInstruction.ExceptionEnd)
					{
						int target = ctx.BranchTargets[0];
						ctx.SetInstruction(IRInstruction.Jmp);
						ctx.SetBranch(target);
					}
				}
			}
		}

		protected void CreateExceptionReturnOperands()
		{
			exceptionRegisters = new Dictionary<BasicBlock, Operand>();

			foreach (var handler in MethodCompiler.Method.ExceptionHandlers)
			{
				var block = BasicBlocks.GetByLabel(handler.HandlerStart);

				var register = AllocateVirtualRegister(TypeSystem.BuiltIn.Pointer);

				exceptionRegisters.Add(block, register);
			}
		}
	}
}