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
			if (!HasProtectedRegions)
				return;

			exceptionVirtualRegisters = new Dictionary<BasicBlock, Operand>();
			finallyReturnVirtualRegisters = new Dictionary<BasicBlock, Operand>();

			exceptionType = TypeSystem.GetTypeByName("System", "Exception");

			var exceptionRegister = Operand.CreateCPURegister(exceptionType, Architecture.ExceptionRegister);

			var finallyReturnBlockRegister = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, Architecture.FinallyReturnBlockRegister);

			var nullOperand = Operand.GetNull(TypeSystem);

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
						ctx.AppendInstruction(IRInstruction.Call);
						ctx.SetBranch(target);
					}
					else if (ctx.Instruction == IRInstruction.FinallyStart)
					{
						var exceptionVirtualRegister = ctx.Result;
						var finallyReturnBlockVirtualRegister = ctx.Result2;

						//var header = FindImmediateExceptionHandler(ctx);
						//var headerBlock = BasicBlocks.GetByLabel(header.HandlerStart);

						exceptionVirtualRegisters.Add(ctx.BasicBlock, exceptionRegister);
						finallyReturnVirtualRegisters.Add(ctx.BasicBlock, finallyReturnBlockRegister);

						ctx.SetInstruction(IRInstruction.KillAll);
						ctx.AppendInstruction(IRInstruction.Gen, exceptionRegister);
						ctx.AppendInstruction(IRInstruction.Gen, finallyReturnBlockRegister);

						ctx.AppendInstruction(IRInstruction.Move, exceptionVirtualRegister, exceptionRegister);
						ctx.AppendInstruction(IRInstruction.Move, finallyReturnBlockVirtualRegister, finallyReturnBlockRegister);
					}
					else if (ctx.Instruction == IRInstruction.FinallyEnd)
					{
						var header = FindImmediateExceptionHandler(ctx);
						var headerBlock = BasicBlocks.GetByLabel(header.HandlerStart);

						var exceptionVirtualRegister = exceptionVirtualRegisters[headerBlock];
						var finallyReturnBlockVirtualRegister = finallyReturnVirtualRegisters[headerBlock];

						var newBlocks = CreateNewBlocksWithContexts(2);

						ctx.SetInstruction(IRInstruction.Move, exceptionRegister, exceptionVirtualRegister);
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
						var exceptionVirtualRegister = ctx.Result;

						ctx.SetInstruction(IRInstruction.KillAll);
						ctx.AppendInstruction(IRInstruction.Gen, exceptionRegister);
						ctx.AppendInstruction(IRInstruction.Move, exceptionVirtualRegister, exceptionRegister);
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

		//protected void CreateExceptionReturnOperands()
		//{
		//	exceptionVirtualRegisters = new Dictionary<BasicBlock, Operand>();
		//	finallyReturnVirtualRegister = new Dictionary<BasicBlock, Operand>();

		//	foreach (var handler in MethodCompiler.Method.ExceptionHandlers)
		//	{
		//		var block = BasicBlocks.GetByLabel(handler.HandlerStart);

		//		MosaType specificExceptionType = exceptionType;

		//		if (handler.HandlerType == ExceptionHandlerType.Exception)
		//		{
		//			specificExceptionType = handler.Type;
		//		}

		//		var register = AllocateVirtualRegister(exceptionType);

		//		exceptionVirtualRegisters.Add(block, register);

		//		if (handler.HandlerType == ExceptionHandlerType.Finally)
		//		{
		//			finallyReturnVirtualRegister.Add(block, AllocateVirtualRegister(TypeSystem.BuiltIn.I4));
		//		}
		//	}
		//}
	}
}