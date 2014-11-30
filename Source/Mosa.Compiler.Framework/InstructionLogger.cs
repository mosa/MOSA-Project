/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Compiler.Trace
{
	/// <summary>
	/// Logs all instructions.
	/// </summary>
	public static class InstructionLogger
	{
		public static void Run(BaseMethodCompiler methodCompiler, IPipelineStage stage)
		{
			Run(
				methodCompiler.Trace,
				methodCompiler.FormatStageName(stage),
				methodCompiler.Method,
				methodCompiler.InstructionSet,
				methodCompiler.BasicBlocks
			);
		}

		public static void Run(CompilerTrace compilerTrace, string stage, MosaMethod method, InstructionSet instructionSet, BasicBlocks basicBlocks)
		{
			if (compilerTrace == null)
				return;

			if (!compilerTrace.TraceFilter.IsMatch(method, stage))
				return;

			var traceLog = new TraceLog(TraceType.InstructionList, method, stage, true);

			traceLog.Log(String.Format("IR representation of method {0} after stage {1}:", method.FullName, stage));
			traceLog.Log();

			if (basicBlocks.Count > 0)
			{
				foreach (var block in basicBlocks)
				{
					traceLog.Log(String.Format("Block #{0} - Label L_{1:X4}", block.Sequence, block.Label)
						+ (basicBlocks.IsHeaderBlock(block) ? " [Header]" : string.Empty));

					traceLog.Log("  Prev: " + ListBlocks(block.PreviousBlocks));

					LogInstructions(traceLog, new Context(instructionSet, block));

					traceLog.Log("  Next: " + ListBlocks(block.NextBlocks));

					traceLog.Log();
				}
			}
			else
			{
				LogInstructions(traceLog, new Context(instructionSet, 0));
			}

			compilerTrace.NewTraceLog(traceLog);
		}

		private static string ListBlocks(IList<BasicBlock> blocks)
		{
			var text = new StringBuilder();

			foreach (var next in blocks)
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
		/// <param name="traceLog">The trace log.</param>
		/// <param name="ctx">The context.</param>
		private static void LogInstructions(TraceLog traceLog, Context ctx)
		{
			for (; ctx.Index >= 0; ctx.GotoNext())
			{
				if (ctx.IsEmpty) // || ctx.IsBlockStartInstruction || ctx.IsBlockEndInstruction)
					continue;

				var sb = new StringBuilder();

				sb.AppendFormat("L_{0:X4}", ctx.Label);

				if (ctx.Marked)
					sb.Append("*");
				else
					sb.Append(" ");

				sb.AppendFormat("{0}", ctx.Instruction.ToString(ctx));

				traceLog.Log(sb.ToString());

				if (ctx.IsBlockEndInstruction)
					return;
			}
		}
	}
}