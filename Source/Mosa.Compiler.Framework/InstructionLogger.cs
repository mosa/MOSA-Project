// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
				methodCompiler.BasicBlocks
			);
		}

		public static void Run(CompilerTrace compilerTrace, string stage, MosaMethod method, BasicBlocks basicBlocks)
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

					LogInstructions(traceLog, block.First);

					traceLog.Log("  Next: " + ListBlocks(block.NextBlocks));

					traceLog.Log();
				}
			}
			else
			{
				traceLog.Log("No instructions.");
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

				text.AppendFormat(next.ToString());
			}

			return text.ToString();
		}

		/// <summary>
		/// Logs the instructions in the given enumerable to the trace.
		/// </summary>
		/// <param name="traceLog">The trace log.</param>
		/// <param name="node">The context.</param>
		private static void LogInstructions(TraceLog traceLog, InstructionNode node)
		{
			for (; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.IsEmpty)
					continue;

				traceLog.Log(node.ToString());

				if (node.IsBlockEndInstruction)
					return;
			}
		}
	}
}
