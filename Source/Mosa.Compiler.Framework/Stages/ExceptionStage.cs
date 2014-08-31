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

			CreateExceptionReturnOperands();

			foreach (var block in BasicBlocks)
			{
				for (var ctx = new Context(InstructionSet, block); !ctx.IsBlockEndInstruction; ctx.GotoNext())
				{
					if (ctx.IsEmpty)
						continue;

					if (ctx.Instruction == IRInstruction.CallFinally)
					{
						var target = ctx.BranchTargets[0];

						//var zero = Operand.CreateConstantUnsignedInt(TypeSystem, (uint)0x0);

						ctx.SetInstruction(IRInstruction.KillAll);
						//ctx.AppendInstruction(IRInstruction.Move, exceptionRegister, zero);
						ctx.AppendInstruction(IRInstruction.InternalCall);
						ctx.SetBranch(target);
					}
					else if (ctx.Instruction == IRInstruction.FinallyStart)
					{
						var header = FindImmediateExceptionEntry(ctx);
						var headerBlock = BasicBlocks.GetByLabel(header.HandlerStart);

						var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.Pointer);

						ctx.SetInstruction(IRInstruction.Gen, exceptionRegister);
						ctx.AppendInstruction(IRInstruction.Move, exceptionRegisters[headerBlock], exceptionRegister);
					}
					else if (ctx.Instruction == IRInstruction.FinallyEnd)
					{
						var header = FindImmediateExceptionEntry(ctx);
						var headerBlock = BasicBlocks.GetByLabel(header.HandlerStart);

						ctx.SetInstruction(IRInstruction.Move, exceptionRegister, exceptionRegisters[headerBlock]);
						ctx.AppendInstruction(IRInstruction.InternalReturn);
					}
					else if (ctx.Instruction == IRInstruction.ExceptionStart)
					{
						ctx.SetInstruction(IRInstruction.Move, ctx.Result, exceptionRegister);
					}
					else if (ctx.Instruction == IRInstruction.ExceptionEnd)
					{
						ctx.AppendInstruction(IRInstruction.InternalReturn);
					}
				}
			}
		}

		protected void CreateExceptionReturnOperands()
		{
			exceptionRegisters = new Dictionary<BasicBlock, Operand>();

			foreach (var entry in MethodCompiler.Method.ExceptionBlocks)
			{
				var block = BasicBlocks.GetByLabel(entry.HandlerStart);

				var register = AllocateVirtualRegister(TypeSystem.BuiltIn.Pointer);

				exceptionRegisters.Add(block, register);
			}
		}
	}
}