// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Compiler.Framework.Trace
{
	/// <summary>
	/// Logs all instructions.
	/// </summary>
	public static class InstructionLogger
	{
		private const int TraceLevel = 6;

		public static void Run(MethodCompiler methodCompiler, BaseMethodCompilerStage stage)
		{
			Run(
				methodCompiler.Trace,
				stage.FormattedStageName,
				methodCompiler.Method,
				methodCompiler.BasicBlocks
			);
		}

		public static void Run(CompilerTrace compilerTrace, string stage, MosaMethod method, BasicBlocks basicBlocks)
		{
			if (!compilerTrace.IsTraceable(TraceLevel))
				return;

			var traceLog = new TraceLog(TraceType.MethodInstructions, method, stage);

			traceLog?.Log($"{method.FullName} after stage {stage}:");
			traceLog?.Log();

			if (basicBlocks.Count > 0)
			{
				foreach (var block in basicBlocks)
				{
					traceLog?.Log($"Block #{block.Sequence} - Label L_{block.Label:X4}" + (block.IsHeadBlock ? " [Header]" : string.Empty));
					traceLog?.Log($"  Prev: {ListBlocks(block.PreviousBlocks)}");

					LogInstructions(traceLog, block.First);

					traceLog?.Log($"  Next: {ListBlocks(block.NextBlocks)}");
					traceLog?.Log();
				}
			}
			else
			{
				traceLog?.Log("No instructions.");
			}

			compilerTrace.PostTraceLog(traceLog);
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
