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
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Runtime.InternalLog
{
	/// <summary>
	/// Logs all incoming instructions.
	/// </summary>
	public sealed class InstructionLogger : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{
		private IInstructionLogFilter filter = new ConfigurableInstructionLogFilter();
		private IInstructionLogListener listener = null;

		#region IPipelineStage

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name { get { return @"InstructionLogger"; } }

		#endregion// IPipelineStage

		#region IMethodCompilerStage Members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		public void Run()
		{
			if (listener == null)
				return;

			// Previous stage
			IPipelineStage prevStage = methodCompiler.GetPreviousStage(typeof(IMethodCompilerStage));

			if (!filter.IsMatch(methodCompiler.Method, prevStage))
				return;

			StringBuilder text = new StringBuilder();

			// Line number
			int index = 1;

			text.AppendLine(String.Format("IR representation of method {0} after stage {1}", methodCompiler.Method, prevStage.Name));

			if (this.basicBlocks.Count > 0)
			{
				foreach (BasicBlock block in basicBlocks)
				{
					text.AppendFormat("Block #{0} - label L_{1:X4}", index, block.Label);

					foreach (BasicBlock prev in block.PreviousBlocks)
						text.AppendFormat("  Prev: L_{0:X4}", prev.Label);

					LogInstructions(text, new Context(InstructionSet, block));

					foreach (BasicBlock next in block.NextBlocks)
						text.AppendFormat("  Next: L_{0:X4}", next.Label);

					index++;
				}
			}
			else
			{
				LogInstructions(text, new Context(InstructionSet, 0));
			}
		}

		#endregion // IMethodCompilerStage Members

		#region Internals

		/// <summary>
		/// Logs the instructions in the given enumerable to the trace.
		/// </summary>
		/// <param name="ctx">The context.</param>
		private void LogInstructions(StringBuilder text, Context ctx)
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

		#endregion // Internals
	}
}
