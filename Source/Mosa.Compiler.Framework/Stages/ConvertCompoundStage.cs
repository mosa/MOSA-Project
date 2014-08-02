﻿/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Ki (kiootic) <kiootic@gmail.com>
 */

using Mosa.Compiler.Framework.IR;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// This stage identifies move/load/store/call instructions with compound types
	/// (i.e. user defined value types with size greater than sans enums) operands and
	/// convert them into respective compound instructions, which will be expanded into
	/// multiple native instructions by platform layer.
	/// </summary>
	public class ConvertCompoundStage : BaseMethodCompilerStage
	{
		private Dictionary<Operand, Operand> repl = new Dictionary<Operand, Operand>();

		protected override void Run()
		{
			for (int index = 0; index < BasicBlocks.Count; index++)
				for (Context ctx = new Context(InstructionSet, BasicBlocks[index]); !ctx.IsBlockEndInstruction; ctx.GotoNext())
					if (!ctx.IsEmpty)
						ProcessInstruction(ctx.Clone());

			for (int index = 0; index < BasicBlocks.Count; index++)
				for (Context ctx = new Context(InstructionSet, BasicBlocks[index]); !ctx.IsBlockEndInstruction; ctx.GotoNext())
					if (!ctx.IsEmpty)
						ReplaceOperands(ctx.Clone());

			repl.Clear();
		}

		private void ProcessInstruction(Context ctx)
		{
			if (ctx.Instruction is Load)
			{
				if (ctx.MosaType != null &&
					TypeLayout.IsCompoundType(ctx.MosaType) && !ctx.MosaType.IsUI8 && !ctx.MosaType.IsR8)
				{
					if (ctx.Result.IsVirtualRegister && !repl.ContainsKey(ctx.Result))
					{
						repl[ctx.Result] = MethodCompiler.StackLayout.AddStackLocal(ctx.MosaType);
					}
					ctx.ReplaceInstructionOnly(IRInstruction.CompoundLoad);
				}
			}
			else if (ctx.Instruction is Store)
			{
				if (ctx.MosaType != null &&
					TypeLayout.IsCompoundType(ctx.MosaType) && !ctx.MosaType.IsUI8 && !ctx.MosaType.IsR8)
				{
					if (ctx.Operand3.IsVirtualRegister && !repl.ContainsKey(ctx.Operand3))
					{
						repl[ctx.Operand3] = MethodCompiler.StackLayout.AddStackLocal(ctx.Result.Type);
					}
					ctx.ReplaceInstructionOnly(IRInstruction.CompoundStore);
				}
			}
			else if (ctx.Instruction is Move)
			{
				if (ctx.Result.Type.Equals(ctx.Operand1.Type) &&
					TypeLayout.IsCompoundType(ctx.Result.Type) && !ctx.Result.Type.IsUI8 && !ctx.Result.Type.IsR8)
				{
					if (ctx.Result.IsVirtualRegister && !repl.ContainsKey(ctx.Result))
					{
						repl[ctx.Result] = MethodCompiler.StackLayout.AddStackLocal(ctx.Result.Type);
					}
					if (ctx.Operand1.IsVirtualRegister && !repl.ContainsKey(ctx.Operand1))
					{
						repl[ctx.Operand1] = MethodCompiler.StackLayout.AddStackLocal(ctx.Operand1.Type);
					}
					ctx.ReplaceInstructionOnly(IRInstruction.CompoundMove);
				}
			}
			else if (ctx.Instruction is Call)
			{
				if (ctx.Result != null &&
					TypeLayout.IsCompoundType(ctx.Result.Type) && !ctx.Result.Type.IsUI8 && !ctx.Result.Type.IsR8)
				{
					if (ctx.Result.IsVirtualRegister && !repl.ContainsKey(ctx.Result))
					{
						repl[ctx.Result] = MethodCompiler.StackLayout.AddStackLocal(ctx.Result.Type);
					}
				}
			}
		}

		private void ReplaceOperands(Context ctx)
		{
			if (ctx.Result != null && repl.ContainsKey(ctx.Result))
				ctx.Result = repl[ctx.Result];

			if (ctx.Result2 != null && repl.ContainsKey(ctx.Result2))
				ctx.Result2 = repl[ctx.Result2];

			int count = ctx.OperandCount;
			for (int i = 0; i < count; i++)
			{
				var operand = ctx.GetOperand(i);
				if (operand != null && repl.ContainsKey(operand))
					ctx.SetOperand(i, repl[operand]);
			}
		}
	}
}