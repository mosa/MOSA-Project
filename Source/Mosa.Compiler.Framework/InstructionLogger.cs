/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Compiler.Framework;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Compiler.InternalTrace
{
	/// <summary>
	/// Logs all instructions.
	/// </summary>
	public static class InstructionLogger
	{
		public static void Run(IMethodCompiler methodCompiler, IPipelineStage stage)
		{
			Run(
				methodCompiler.InternalLog,
				stage,
				methodCompiler.Method,
				methodCompiler.InstructionSet,
				methodCompiler.BasicBlocks
			);
		}

		public static void Run(IInternalTrace internalLog, IPipelineStage stage, RuntimeMethod method, InstructionSet instructionSet, IList<BasicBlock> basicBlocks)
		{
			if (internalLog == null)
				return;

			if (internalLog.InstructionTraceListener == null)
				return;

			if (!internalLog.InstructionTraceFilter.IsMatch(method, stage.Name))
				return;

			StringBuilder text = new StringBuilder();

			// Line number
			int index = 1;

			text.AppendLine(String.Format("IR representation of method {0} after stage {1}:", method, stage.Name));
			text.AppendLine();

			if (basicBlocks.Count > 0)
			{
				foreach (BasicBlock block in basicBlocks)
				{
					text.AppendFormat("Block #{0} - Label L_{1:X4}", index, block.Label);
					text.AppendLine();

					text.AppendFormat("  Prev: ");
					text.AppendLine(ListBlocks(block.PreviousBlocks));

					LogInstructions(text, new Context(instructionSet, block));

					text.AppendFormat("  Next: ");
					text.AppendLine(ListBlocks(block.NextBlocks));

					text.AppendLine();
					index++;
				}
			}
			else
			{
				LogInstructions(text, new Context(instructionSet, 0));
			}

			internalLog.InstructionTraceListener.NotifyNewInstructionTrace(method, stage.Name, text.ToString());
		}

		private static string ListBlocks(IList<BasicBlock> blocks)
		{
			StringBuilder text = new StringBuilder();

			foreach (BasicBlock next in blocks)
			{
				if (text.Length != 0)
					text.Append(", ");

				text.AppendFormat("L_{0:X4}", next.Label);
			}

			return text.ToString();
		}

		/// <summary>
		/// Logs the instructions in the given enumerable to the trace.
		/// </summary>
		/// <param name="ctx">The context.</param>
		private static void LogInstructions(StringBuilder text, Context ctx)
		{
			for (; !ctx.EndOfInstruction; ctx.GotoNext())
			{
				if (ctx.Instruction == null)
					continue;

				if (ctx.Ignore)
					text.Append("; ");

				if (ctx.Marked)
					text.AppendFormat("L_{0:X4}* {1}", ctx.Label, ctx.Instruction.ToString(ctx));
				else
					text.AppendFormat("L_{0:X4}: {1}", ctx.Label, ctx.Instruction.ToString(ctx));

				text.AppendLine();
			}

		}

	}
}
