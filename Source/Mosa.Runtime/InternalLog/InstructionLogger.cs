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

using Mosa.Runtime.Metadata;
using Mosa.Runtime.TypeSystem;
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Runtime.InternalLog
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

		public static void Run(IInternalLog internalLog, IPipelineStage stage, RuntimeMethod method, InstructionSet instructionSet, List<BasicBlock> basicBlocks)
		{
			if (internalLog == null)
				return;

			if (internalLog.InstructionLogListener == null)
				return;

			if (!internalLog.InstructionLogFilter.IsMatch(method, stage))
				return;

			StringBuilder text = new StringBuilder();

			// Line number
			int index = 1;

			text.AppendLine(String.Format("IR representation of method {0} after stage {1}", method, stage.Name));

			if (basicBlocks.Count > 0)
			{
				foreach (BasicBlock block in basicBlocks)
				{
					text.AppendFormat("Block #{0} - label L_{1:X4}", index, block.Label);

					foreach (BasicBlock prev in block.PreviousBlocks)
						text.AppendFormat("  Prev: L_{0:X4}", prev.Label);

					LogInstructions(text, new Context(instructionSet, block));

					foreach (BasicBlock next in block.NextBlocks)
						text.AppendFormat("  Next: L_{0:X4}", next.Label);

					index++;
				}
			}
			else
			{
				LogInstructions(text, new Context(instructionSet, 0));
			}
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

				text.AppendFormat("L_{0:X4}: {1}", ctx.Label, ctx.Instruction.ToString(ctx));
			}
		}

	}
}
