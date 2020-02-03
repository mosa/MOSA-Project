// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;
using System.Text;
using static Mosa.Compiler.Framework.CompilerHooks;

namespace Mosa.Compiler.Framework.Trace
{
	/// <summary>
	/// Logs all instructions.
	/// </summary>
	public static class InstructionTrace
	{
		public static void Run(MethodCompiler methodCompiler, BaseMethodCompilerStage stage, NotifyTraceLogHandler handler)
		{
			Run(
				stage.FormattedStageName,
				methodCompiler.Method,
				methodCompiler.BasicBlocks,
				methodCompiler.MethodData.Version,
				handler
			);
		}

		public static void Run(string stage, MosaMethod method, BasicBlocks basicBlocks, int version, NotifyTraceLogHandler handler)
		{
			var traceLog = new TraceLog(TraceType.MethodInstructions, method, stage, version);

			traceLog?.Log($"{method.FullName} [v{version}] after stage {stage}:");
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

			handler.Invoke(traceLog);
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
